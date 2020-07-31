using EvolutionaryAlgorithms.Algorithms;
using EvolutionaryAlgorithms.ImageProcessing;
using EvolutionaryAlgorithms.Individuals;
using EvolutionaryAlgorithms.ProblemsConfig.ImageProblemsConfig;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Xsl;

namespace EVAConsoleImageSimulator
{
    class Program
    {
        /// <summary>
        /// Parallel EA.
        /// </summary>
        /// <param name="problemConfigName">Problem represention.</param>
        /// <param name="inputFileName">Input image</param>
        /// <param name="logRate">logger rate</param>
        public static void RunParallelImage(Type problemConfigName, string inputFileName, int logRate, bool parallelization)
        {

            var img = Image.FromFile(inputFileName) as Bitmap;

            var blockSize = ParameterSetter.SetPositiveIntParameter("Block size", 5);

            Bitmap[] targets = Split_Concate.SplitImg( img, blockSize);

            var consoleProblemConfigs = new IImageProblemConfig[targets.Length];
            var evas = new IEVA[targets.Length];


            var configuration = new Configuration[targets.Length];

            for (int i = 0; i < targets.Length; i++)
            {
                consoleProblemConfigs[i] = Activator.CreateInstance(problemConfigName) as IImageProblemConfig;
                consoleProblemConfigs[i].Initialize(i);

                consoleProblemConfigs[i].Initialize(targets[i], inputFileName);

                configuration[i] = new Configuration();
            }

            evas = Configuration.SetEVA(consoleProblemConfigs, parallelization);

            for (int i = 0; i < targets.Length; i++)
            {
                configuration[i].SetGenerationInfoLog(evas[i], consoleProblemConfigs[i], logRate);
                consoleProblemConfigs[i].ConfigGATermination(evas[i]);
            }

            IIndividual[] bestInds = new IIndividual[targets.Length];

            var stopwatch = Stopwatch.StartNew();

            /**/
            Parallel.For(0, evas.Length, index => {

                evas[index].Run();

                bestInds[index] = evas[index].BestIndividual;

               // configuration[index].Saveinfo(inputFileName, index);

            });

            /*/

            for (var index = 0; index < evas.Length; index++)
            {
                evas[index].Run();

                bestInds[index] = evas[index].BestIndividual;

              //  Configuration.Saveinfo(inputFileName,);

            }


            /**/
            stopwatch.Stop();


            Console.WriteLine("Total time: " + stopwatch.Elapsed);
            var result = Split_Concate.ConcateImgs(bestInds, blockSize, img.Width, img.Height);

            var m_destFolder = Configuration.CreateDirectory(inputFileName, "result_final");

            var fileName = m_destFolder + "/" + "final" + ".png";

            result.Save(fileName);
        }

        /// <summary>
        /// Notmal EA
        /// </summary>
        /// <param name="problemConfigName">Problem represention.</param>
        /// <param name="inputFileName">Input image</param>
        /// <param name="logRate">logger rate</param>
        public static void RunImage(Type problemConfigName, string inputFileName, int logRate, bool parallelization)
        {

            // Scale img
            var paramterScale = ParameterSetter.SetFloat("Scale", 0.01f, 1);


            var consoleProblemConfig = Activator.CreateInstance(problemConfigName) as IImageProblemConfig;
            consoleProblemConfig.Initialize(0);

            consoleProblemConfig.InitializeScale(paramterScale);
            consoleProblemConfig.Initialize(inputFileName);

            var eva = Configuration.SetEVA(new[] { consoleProblemConfig}, parallelization);
            var configuration = new Configuration();


            configuration.SetGenerationInfoLog(eva[0], consoleProblemConfig, logRate);
            consoleProblemConfig.ConfigGATermination(eva[0]);
            eva[0].Run();
        }

        static void Main(string[] args)
        {
            bool running = true;
            
            while (running)
            {
                Console.Clear();

                Console.WriteLine("Evolution image approximation.");

                var paralelization = ParameterSetter.SetBool("Parallelization");

                // Logger rate
                var logRate = ParameterSetter.SetPositiveIntParameter("Logger rate", 1);

                // load input image
                var inputFileName = ParameterSetter.SetFileNameParameter("image");

                var problemConfigName = ParameterSetter.SetProblemCOofig();


                if (paralelization)
                    RunParallelImage(problemConfigName, inputFileName, logRate, paralelization);
                else
                    RunImage(problemConfigName, inputFileName, logRate, paralelization);

                Console.WriteLine("EVOLVED");
                running = ParameterSetter.SetBool("RESTART");
            }
        }
    }
}
