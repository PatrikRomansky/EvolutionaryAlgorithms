using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Distributions;
using EvolutionaryAlgorithms.Individuals;
using EvolutionaryAlgorithms.Randomization;

namespace EvolutionaryAlgorithms.Algorithms.EvolutionaryStrategies.CMA_ES
{
    /// <summary>
    /// CMA-ES subclass.
    /// </summary>
    public class CMA
    {
        /// <summary>
        /// A number of dimensions
        /// </summary>
        public int Dimensions { get => Parameters.nDim; }

        /// <summary>
        /// Generation number which is monotonically incremented when multi-variate gaussian distribution is updated.
        /// </summary>
        public int CurrentGenerationsNumber { get; private set; }

        public int PopSize { get => Parameters.PopSize; }

        /// <summary>
        /// CMA parameters
        /// </summary>
        CMAParameters Parameters;

        /// <summary>
        /// CMA-ES stochastic optimizer class with ask-and-tell interface.
        /// </summary>
        /// <param name="mean">Initial mean vector of multi-variate gaussian distributions.</param>
        /// <param name="sigma">Initial standard deviation of covariance matrix.</param>
        /// <param name="bounds">(Optional) Lower and upper domain boundaries for each parameter, (n_dim, 2)-dim matrix.</param>
        /// <param name="nMaxResampling">(Optional) A maximum number of resampling parameters (default: 100).
        /// If all sampled parameters are infeasible, the last sampled one  will be clipped with lower and upper bounds.</param>
        public CMA(IIndividual mean, double sigma, Matrix<double> bounds = null, int nMaxResampling = 100)
        {
            Parameters = new CMAParameters(mean, sigma,  bounds, nMaxResampling);
            CurrentGenerationsNumber = 0;
        }

        /// <summary>
        /// Returns the next search vector based on the current covariance matrix.
        /// </summary>
        /// <returns>The next search vector.</returns>
        public Vector<double> Ask()
        {
            for (int i = 0; i < Parameters.n_max_resampling; i++)
            {
                Vector<double> x = Parameters.SampleSolution();
                if (Parameters.IsFeasible(x))
                    return x;
            }
            Vector<double> xNew = Parameters.SampleSolution();
            xNew = Parameters.RepairInfeasibleParams(xNew);
            return xNew;
        }

        /// <summary>
        /// The covariance matrix and step size are recalculated based on the search vectors and their results.
        /// </summary>
        /// <param name="solutions">Tuple's list of search vectors and result values.</param>
        public void Tell(List<Tuple<Vector<double>, double>> solutions)
        {
            if (solutions.Count != Parameters.PopSize)
            {
                throw new ArgumentException("Must tell popsize-length solutions.");
            }

            CurrentGenerationsNumber += 1;
            Tuple<Vector<double>, double>[] sortedSolutions = solutions.OrderBy(x => x.Item2).ToArray();

            // Sample new population of search_points, for k=1, ..., popsize
            Matrix<double> B = Matrix<double>.Build.Dense(Parameters._B.RowCount, Parameters._B.ColumnCount);
            Vector<double> D = Vector<double>.Build.Dense(Parameters._D.Count);
            if (Parameters._B == null || Parameters._D == null)
            {
                Parameters.C = (Parameters.C + Parameters.C.Transpose()) / 2;
                MathNet.Numerics.LinearAlgebra.Factorization.Evd<double> evd_C = Parameters.C.Evd();
                B = evd_C.EigenVectors;
                D = Vector<double>.Build.Dense(evd_C.EigenValues.PointwiseSqrt().Select(tmp => tmp.Real).ToArray());
            }
            else
            {
               Parameters._B.CopyTo(B);
               Parameters. _D.CopyTo(D);
            }
            Parameters._B = null;
            Parameters._D = null;

            Matrix<double> x_k = Matrix<double>.Build.DenseOfRowVectors(sortedSolutions.Select(x => x.Item1));
            Matrix<double> y_k = Matrix<double>.Build.Dense(sortedSolutions.Length, Dimensions);
            for (int i = 0; i < sortedSolutions.Length; i++)
            {
                y_k.SetRow(i, (x_k.Row(i) - Parameters._mean) / Parameters.sigma);
            }

            // Selection and recombination
            Vector<double>[] kk = y_k.EnumerateRows().Take(Parameters.mu).ToArray();
            Matrix<double> y_k_T = Matrix<double>.Build.Dense(Dimensions, kk.Length);
            for (int i = 0; i < kk.Length; i++)
            {
                y_k_T.SetColumn(i, kk[i]);
            }
            Vector<double> subWeights = Vector<double>.Build.Dense(Parameters._weights.Take(Parameters.mu).ToArray());
            Matrix<double> y_w_matrix = Matrix<double>.Build.Dense(y_k_T.RowCount, y_k_T.ColumnCount);
            for (int i = 0; i < y_w_matrix.RowCount; i++)
            {
                y_w_matrix.SetRow(i, y_k_T.Row(i).PointwiseMultiply(subWeights));
            }
            Vector<double> y_w = y_w_matrix.RowSums();
            Parameters._mean += Parameters.cm * Parameters.sigma * y_w;

            Vector<double> D_bunno1_diag = 1 / D;
            Matrix<double> D_bunno1_diagMatrix = Matrix<double>.Build.Dense(D_bunno1_diag.Count, D_bunno1_diag.Count);
            for (int i = 0; i < D_bunno1_diag.Count; i++)
            {
                D_bunno1_diagMatrix[i, i] = D_bunno1_diag[i];
            }
            Matrix<double> C_2 = B * D_bunno1_diagMatrix * B;
            Parameters.p_sigma = ((1 - Parameters.c_sigma) * Parameters.p_sigma) + (Math.Sqrt(Parameters.c_sigma * (2 - Parameters.c_sigma) * Parameters.mu_eff) * C_2 * y_w);

            double norm_pSigma = Parameters.p_sigma.L2Norm();
            Parameters.sigma *= Math.Exp(Parameters.c_sigma / Parameters.d_sigma * ((norm_pSigma / Parameters.chi_n) - 1));
            double h_sigma_cond_left = norm_pSigma / Math.Sqrt(1 - Math.Pow(1 - Parameters.c_sigma, 2 * (CurrentGenerationsNumber + 1)));
            double h_sigma_cond_right = (1.4 + (2 / (double)(Dimensions + 1))) * Parameters.chi_n;
            double h_sigma = h_sigma_cond_left < h_sigma_cond_right ? 1.0 : 0.0;

            Parameters.pc = ((1 - Parameters.cc) * Parameters.pc) + (h_sigma * Math.Sqrt(Parameters.cc * (2 - Parameters.cc) * Parameters.mu_eff) * y_w);

            Vector<double> w_io = Vector<double>.Build.Dense(Parameters._weights.Count, 1);
            Vector<double> w_iee = (C_2 * y_k.Transpose()).ColumnNorms(2).PointwisePower(2);
            for (int i = 0; i < Parameters._weights.Count; i++)
            {
                if (Parameters._weights[i] >= 0)
                {
                    w_io[i] = Parameters._weights[i] * 1;
                }
                else
                {
                    w_io[i] = Parameters._weights[i] * Dimensions / (w_iee[i] + Parameters.epsilon);
                }
            }

            double delta_h_sigma = (1 - h_sigma) * Parameters.cc * (2 - Parameters.cc);
           
            if (!(delta_h_sigma <= 1))
            {
                throw new Exception("invalid value of delta_h_sigma");
            }

            Matrix<double> rank_one = Parameters.pc.OuterProduct(Parameters.pc);
            Matrix<double> rank_mu = Matrix<double>.Build.Dense(y_k.ColumnCount, y_k.ColumnCount, 0);
            for (int i = 0; i < w_io.Count; i++)
            {
                rank_mu += w_io[i] * y_k.Row(i).OuterProduct(y_k.Row(i));
            }
            Parameters.C = ((1 + (Parameters.c1 * delta_h_sigma) - Parameters.c1 - (Parameters.cmu * Parameters._weights.Sum())) * Parameters.C) + (Parameters.c1 * rank_one) + (Parameters.cmu * rank_mu);
        }
    }
}
