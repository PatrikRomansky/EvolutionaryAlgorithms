using Accord.Math;
using EvolutionaryAlgorithms.Individuals;
using EvolutionaryAlgorithms.Randomization;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Linq;

namespace EvolutionaryAlgorithms.Algorithms.EvolutionaryStrategies.CMA_ES
{
    public class CMAParameters
    {
        public int mu { get; private set; }
        public double mu_eff { get; private set; }
        public double cc { get; private set; }
        public double c1 { get; private set; }
        public double cmu { get; private set; }
        public double c_sigma { get; private set; }
        public double d_sigma { get; private set; }
        public int cm { get; private set; }
        public double chi_n { get; private set; }
        public Vector<double> _weights { get; private set; }


        public Vector<double> p_sigma { get; set; }
        public Vector<double> pc { get; set; }
        public Vector<double> _mean { get; set; }
        public Matrix<double> C { get; set; }
        public double sigma { get; set; }
        public Vector<double> _D { get; set; }
        public Matrix<double> _B { get; set; }
        public Matrix<double> bounds { get; set; }


        public int n_max_resampling { get; private set; }
        public double epsilon { get; private set; }


        public int PopSize { get; private set; }

        public int nDim{ get; private set; }

        /// <summary>
        /// Validation sigma parameter.
        /// Sigma must be non-zero positive value.
        /// </summary>
        /// <param name="sigma">Parameter</param>
        private void ValidateSigma(double sigma)
        {
            if (!(sigma > 0))
            {
                throw new ArgumentOutOfRangeException("sigma must be non-zero positive value");
            }
        }

        /// <summary>
        /// Validation problem dimensions.
        /// The dimension of mean must be larger than 1
        /// </summary>
        /// <param name="nDim">Dimenstion.</param>
        /// <returns>The dim.</returns>
        private int ValidateNDim(int nDim)
        {
            if (!(nDim > 1))
            {
                throw new ArgumentOutOfRangeException("The dimension of mean must be larger than 1");
            }

            return nDim;
        }

        /// <summary>
        /// Validate Rate for the rank-one upadete
        /// </summary>
        private void ValidateRateRankOneUpdate()
        {
            if (!(c1 <= 1 - cmu))
            {
                throw new Exception("invalid learning rate for the rank-one update");
            }
        }

        /// <summary>
        /// Validate Rate for the rank-mu upadete
        /// </summary>
        private void ValidateRateRankMu()
        {
            if (!(cmu <= 1 - c1))
            {
                throw new Exception("invalid learning rate for the rank-mu update");
            }
        }

        /// <summary>
        /// learning rate for cumulation for the step-size control must be less than 1.
        /// </summary>
        private void StepSizeControl()
        {
            if (!(c_sigma < 1))
            {
                throw new Exception("invalid learning rate for cumulation for the step-size control");
            }
        }

        /// <summary>
        /// The cc must be less than or equal to 1.
        /// </summary>
        private void ValidationRankOneUpdateSecond()
        {
            if (!(cc <= 1))
            {
                throw new Exception("invalid learning rate for cumulation for the rank-one update");
            }

        }

        public CMAParameters(IIndividual mean, double sigma, Matrix<double> bounds, int nMaxResampling, double epsilon = 1e-8)
        {
            ValidateSigma(sigma);

            // problem dimensions
            nDim = ValidateNDim(mean.Length);

            // population size is automatic generate by problem dimesnion eq.
            PopSize = 4 + (int)Math.Floor(3 * Math.Log(nDim));

            // inicialze mu coef.
            mu = PopSize / 2;

            Vector<double> weightsPrime = Vector<double>.Build.Dense(PopSize);

            for (int i = 0; i < PopSize; i++)
            {
                weightsPrime[i] = Math.Log((PopSize + 1) / (double)2) - Math.Log(i + 1);
            }

            Vector<double> weightsPrimeMuEff = Vector<double>.Build.Dense(weightsPrime.Take(mu).ToArray());
            mu_eff = Math.Pow(weightsPrimeMuEff.Sum(), 2) / Math.Pow(weightsPrimeMuEff.L2Norm(), 2);
            
            Vector<double> weightsPrimeMuEffMinus = Vector<double>.Build.Dense(weightsPrime.Skip(mu).ToArray());
            double muEffMinus = Math.Pow(weightsPrimeMuEffMinus.Sum(), 2) / Math.Pow(weightsPrimeMuEffMinus.L2Norm(), 2);

            int alphacCov = 2;
            c1 = alphacCov / (Math.Pow(nDim + 1.3, 2) + mu_eff);
            
            
            cmu = Math.Min(1 - c1, alphacCov * (mu_eff - 2 + (1 / mu_eff)) / (Math.Pow(nDim + 2, 2) + (alphacCov * mu_eff / 2)));

            ValidateRateRankOneUpdate();
            ValidateRateRankMu();

            double minAlpha = Math.Min(1 + (c1 / cmu), Math.Min(1 + (2 * muEffMinus / (mu_eff + 2)), (1 - c1 - cmu) / (nDim * cmu)));

            double positiveSum = weightsPrime.Where(x => x > 0).Sum();
            double negativeSum = Math.Abs(weightsPrime.Where(x => x < 0).Sum());

            Vector<double> weights = Vector<double>.Build.Dense(weightsPrime.Count);
            weightsPrime.CopyTo(weights);
            bool[] weightsIsNotNegative = weightsPrime.Select(x => x >= 0).ToArray();
            for (int i = 0; i < weights.Count; i++)
            {
                weights[i] = weightsIsNotNegative[i] ? 1 / positiveSum * weightsPrime[i] : minAlpha / negativeSum * weightsPrime[i];
            }
           
            cm = 1;

            c_sigma = (mu_eff + 2) / (nDim + mu_eff + 5);
            d_sigma = 1 + (2 * Math.Max(0, Math.Sqrt((mu_eff - 1) / (nDim + 1)) - 1)) + c_sigma;

            StepSizeControl();

            cc = (4 + (mu_eff / nDim)) / (nDim + 4 + (2 * mu_eff / nDim));

            ValidationRankOneUpdateSecond();

            chi_n = Math.Sqrt(nDim) * (1.0 - (1.0 / (4.0 * nDim)) + 1.0 / (21.0 * (Math.Pow(nDim, 2))));

            _weights = weights;

            p_sigma = Vector<double>.Build.Dense(nDim, 0);
            pc = Vector<double>.Build.Dense(nDim, 0);

            _mean = Vector<double>.Build.DenseOfArray(mean.GetGenes());
            C = Matrix<double>.Build.DenseIdentity(nDim, nDim);
            
            this.sigma = sigma;

            validateBounds(bounds);

            this.bounds = bounds;
            n_max_resampling = nMaxResampling;
            this.epsilon = epsilon;
        }


        /// <summary>
        /// Bounds should be (n_dim, 2)-dim matrix"
        /// </summary>
        /// <param name="bounds">Input bounds</param>
        private void validateBounds(Matrix<double> bounds)
        {
            if (!(bounds == null || (bounds.RowCount == nDim && bounds.ColumnCount == 2)))
            {
                throw new Exception("bounds should be (n_dim, 2)-dim matrix");
            }
        }

        /// <summary>
        /// Repair Infeasible Parameters.
        /// </summary>
        /// <param name="param">Input parameters.</param>
        /// <returns></returns>
        public Vector<double> RepairInfeasibleParams(Vector<double> param)
        {
            if (bounds == null)
            {
                return param;
            }
            Vector<double> newParam = param.PointwiseMaximum(bounds.Column(0));
            newParam = newParam.PointwiseMinimum(bounds.Column(1));
            return newParam;
        }

        public bool IsFeasible(Vector<double> param)
        {
            if (bounds == null)
            {
                return true;
            }
            bool isCorrectLower = true;
            bool isCorrectUpper = true;
            for (int i = 0; i < param.Count; i++)
            {
                isCorrectLower &= param[i] >= bounds[i, 0];
                isCorrectUpper &= param[i] <= bounds[i, 1];
            }
            return isCorrectLower & isCorrectUpper;
        }

        /// <summary>
        /// Gets solution CMA_ES.
        /// </summary>
        /// <returns>Solution</returns>
        public Vector<double> SampleSolution()
        {
            if (_B == null || _D == null)
            {
                C = (C + C.Transpose()) / 2;
                MathNet.Numerics.LinearAlgebra.Factorization.Evd<double> evd_C = C.Evd();
                Matrix<double> newB = evd_C.EigenVectors;
                Vector<double> newD = Vector<double>.Build.Dense(evd_C.EigenValues.PointwiseSqrt().Select(tmp => tmp.Real).ToArray());
                newD += epsilon;
                _B = newB;
                _D = newD;
                Matrix<double> D2diagonal = Matrix<double>.Build.DenseDiagonal(newD.Count, 1);
                Vector<double> Dpow2 = newD.PointwisePower(2);
                for (int i = 0; i < D2diagonal.RowCount; i++)
                {
                    D2diagonal[i, i] = Dpow2[i];
                }
                Matrix<double> BD2 = newB * D2diagonal;
                C = BD2 * newB.Transpose();
            }

            Vector<double> z = Vector<double>.Build.Dense(nDim);
            for (int i = 0; i < z.Count; i++)
            {
                z[i] = MathNet.Numerics.Distributions.Normal.Sample(FastRandom.Instance, 0, 1);
            }
            Matrix<double> Ddiagonal = Matrix<double>.Build.DenseDiagonal(_D.Count, 1);
            for (int i = 0; i < Ddiagonal.RowCount; i++)
            {
                Ddiagonal[i, i] = _D[i];
            }
            Matrix<double> y = _B * Ddiagonal * z.ToColumnMatrix();
            Vector<double> x = _mean + (sigma * y.Column(0));
            return x;
        }
    }
}
