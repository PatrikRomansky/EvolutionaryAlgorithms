using EvolutionaryAlgorithms.Algorithms;
using EvolutionaryAlgorithms.Algorithms.Executors;
using EvolutionaryAlgorithms.ElitistPrivileges;
using EvolutionaryAlgorithms.Fitnesses;
using EvolutionaryAlgorithms.Individuals;
using EvolutionaryAlgorithms.Operators.Mutations;
using EvolutionaryAlgorithms.Operators.Xovers;
using EvolutionaryAlgorithms.Selections;
using EvolutionaryAlgorithms.Terminations;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;


namespace EvolutionaryAlgorithms.ProblemsConfig.ImageProblemsConfig
{
    /// <summary>
    /// Base abstract class for image problem controller.
    /// </summary>
    public abstract class ImageProblemConfig : IImageProblemConfig
    {
        // img dize
        protected int width, height, rawWidth, rawHeight;

        // result folder
        protected string m_destFolder;
        protected int ID;

        protected float scale;

        // operators
        protected Dictionary<string, Type> xovers;
        protected Dictionary<string, Type> mutations;
        protected Dictionary<string, Type> selections;
        protected Dictionary<string, Type> terminations;
        protected Dictionary<string, Type> elitizmuses;

        protected IExecutor executor;
        protected IFitness fitness;

        protected IEVA GA { get; set; }

        /// <summary>
        /// Configure the Genetic Algorithm.
        /// </summary>
        /// <param name="ga">The genetic algorithm.</param>
        public virtual void ConfigGATermination(IEVA ga)
        {
            GA = ga;
            ga.TerminationReached += (sender, args) =>
            {   /**/    
                using (var collection = new MagickImageCollection())
                {
                    var files = Directory.GetFiles(m_destFolder, "*.png");

                    foreach (var image in files)
                    {
                        collection.Add(image);
                        collection[0].AnimationDelay = 100;
                    }

                    var settings = new QuantizeSettings();
                    settings.Colors = 256;
                    collection.Quantize(settings);

                    collection.Optimize();
                    collection.Write(Path.Combine(m_destFolder, "result.gif"));
                }
                /**/
                Saveinfo();
            };
        }

        /// <summary>
        /// Creates the individual.
        /// </summary>
        /// <returns>The individual.</returns>
        public abstract IIndividual CreateIndividual();

        /// <summary>
        /// Creates the individual.
        /// </summary>
        /// <returns>The individual.</returns>
        public abstract IIndividual CreateEmptyIndividual();

        /// <summary>
        /// Initialize this instance operator components.
        /// </summary>
        /// <param name="ID">Instacen ID</param>
        public void Initialize(int ID)
        {
            this.ID = ID;
            InitializeXovers();
            InitializeElities();
            InitializeMutations();
            InitializeSelections();
            InitializeTermination();
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        /// <param name="targetInpuFile">File path (target file)</param>
        public virtual void Initialize(string targetInputFile)
        {
            executor = new ParallelExecutor();

            var inputImageFile = targetInputFile;
            var folder = Path.Combine(Path.GetDirectoryName(inputImageFile), "results_" + ID);
            var filePath = inputImageFile.Split('/');
            var fileName = filePath[filePath.Length - 1].Split('.')[0];
            m_destFolder = folder + "_" + fileName + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmssmmm");

            Directory.CreateDirectory(m_destFolder);
        }

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        /// <param name="target">Problem target</param>
        /// <param name="path">File path (target file)</param>
        public abstract void Initialize(object target, string targetInputFile);


        /// <summary>
        /// Initialize Bitmap form file path.
        /// </summary>
        /// <param name="target">File path.</param>
        /// <returns>Target bitmap.</returns>
        protected virtual Bitmap InititializeSame(object target)
        {
            var inputImageFile = (String)target;
            var img = Image.FromFile(inputImageFile);

            rawWidth = img.Width;
            rawHeight = img.Height;

            width = (int)(scale * rawWidth);
            height = (int)(scale * rawHeight);

            var targetBitmap = ScaleImage(Image.FromFile(inputImageFile), width, height);

            return targetBitmap;
        }


        /// <summary>
        /// Initializes possible xovers for this instance.
        /// </summary>
        public abstract void InitializeXovers();

        /// <summary>
        /// Initializes possible elities for this instance.
        /// </summary>
        public abstract void InitializeElities();

        /// <summary>
        /// Initializes possible mutations for this instance.
        /// </summary>
        public abstract void InitializeMutations();

        /// <summary>
        /// Initializes possible selections for this instance.
        /// </summary>
        public abstract void InitializeSelections();

        /// <summary>
        /// Initializes possible terminations for this instance.
        /// </summary>
        public abstract void InitializeTermination();

        /// <summary>
        /// Creates the xover.
        /// </summary>
        /// <param name="name">Xover name.</param>
        /// <returns>The xover</returns>
        public IXover CreateXover(string name)
        {
            Type t = xovers[name];
            var a = Activator.CreateInstance(t);
            return Activator.CreateInstance(t) as IXover;
        }

        /// <summary>
        /// Creates the elite.
        /// </summary>
        /// <param name="name">Elite name.</param>
        /// <returns>The elite.</returns>
        public IElite CreateElite(string name, double perc)
        {
            return Activator.CreateInstance(elitizmuses[name], perc) as IElite;
        }

        /// <summary>
        ///  Create the executor.
        /// </summary>
        /// <returns>The executor.</returns>
        public IExecutor CreateExecutor()
        {
            return executor;
        }

        /// <summary>
        /// Creates the fitness.
        /// </summary>
        /// <returns>The fitness.</returns>
        public IFitness CreateFitness()
        {
            return fitness;
        }

        /// <summary>
        /// Creates the mutation.
        /// </summary>
        /// <param name="name">Mutation name.</param>
        /// <returns>The mutation.</returns>
        public IMutation CreateMutation(string name)
        {
            return Activator.CreateInstance(mutations[name]) as IMutation;
        }

        /// <summary>
        /// Creates the selection.
        /// </summary>
        /// <param name="name">Selection name.</param>
        /// <returns>The selection.</returns>
        public ISelection CreateSelection(string name)
        {
            return Activator.CreateInstance(selections[name]) as ISelection;
        }

        /// <summary>
        /// Creates the termination.
        /// </summary>
        /// <param name="name">Termination name</param>
        /// <param name="param"></param>
        /// <returns>The termination</returns>
        public ITermination CreateTermination(string name, object param)
        {
            ITermination termination = Activator.CreateInstance(terminations[name]) as ITermination;
            termination.InitializeTerminationCondition(param);

            return termination;
        }

        /// <summary>
        /// Gets possible xover for this instance.
        /// </summary>
        /// <returns>Possible xovers.</returns>
        public string[] PossibleXovers()
        {
            return xovers.Keys.ToArray();
        }

        /// <summary>
        /// Gets possible elities for this instance.
        /// </summary>
        /// <returns>Possible elities.</returns>
        public string[] PossibleElities()
        {
            return elitizmuses.Keys.ToArray();
        }

        /// <summary>
        /// Gets possible mutations for this instance.
        /// </summary>
        /// <returns>Possible mutations.</returns>
        public string[] PossibleMutations()
        {
            return mutations.Keys.ToArray();
        }

        /// <summary>
        /// Gets possible selections for this instance.
        /// </summary>
        /// <returns>Possible selections.</returns>
        public string[] PossibleSelections()
        {
            return selections.Keys.ToArray();
        }

        /// <summary>
        /// Initializes possible terminations for this instance.
        /// </summary>
        public string[] PossibleTerminations()
        {
            return terminations.Keys.ToArray();
        }

        /// <summary>
        /// Resize image.
        /// </summary>
        /// <param name="scale">Scale percentage.</param>
        public void InitializeScale(float scale)
        {
            this.scale = scale;
        }

        /// <summary>
        /// Draws the sample.
        /// </summary>
        /// <param name="bestIndividual">The current best individual.</param>
        /// <param name="logRate">Logger rate.</param>
        /// <returns>Best ind if exist, else null.</returns>
        public virtual object ShowBestIndividual(IIndividual bestIndividual, int logRate)
        {
            if (GA.CurrentGenerationsNumber % logRate == 0 | GA.CurrentGenerationsNumber == 1)
            {
                var best = bestIndividual as IndividualImage;

                using (var bitmap = best.BuildBitmap())
                {
                    var img = ScaleImage(bitmap, rawWidth, rawHeight);

                    //var img = bitmap;

                    var fileName = m_destFolder + "/" + GA.CurrentGenerationsNumber.ToString("D10") + "_" + best.Fitness + ".png";

                    img.Save(fileName);
                    return fileName;
                }
            }
            return null;
        }

        /// <summary>
        /// Resize image.
        /// </summary>
        /// <param name="image">Input image.</param>
        /// <param name="nnewWidth">New width.</param>
        /// <param name="newHeight">New height.</param>
        /// <returns>Scale image.</returns>
        public virtual Bitmap ScaleImage(Image image, int newWidth, int newHeight)
        {
            var destRect = new Rectangle(0, 0, newWidth, newHeight);
            var destImage = new Bitmap(newWidth, newHeight);
            /**/
            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                // best quality of resize imgs
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage as Bitmap;
            /*/
            return new Bitmap(image, new Size(newWidth, newHeight));
            /**/
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        private void LogData(string path, string fileName, List<string> data)
        {

            // encodes the output as text.
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(path + '/' + fileName + ".txt"))
            {
                foreach (string line in data)
                    file.WriteLine(line);
            }
        }

        private List<string> fits = new List<string>();
        private List<string> elapsedTimeSpeed = new List<string>();
        private List<string> allInfo = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        public void Saveinfo()
        {
            LogData(m_destFolder, "fitness", fits);
            LogData(m_destFolder, "speed", elapsedTimeSpeed);
            LogData(m_destFolder, "all", allInfo);
        }

        /// <summary>
        /// Sets information about generation into log.
        /// </summary>
        /// <param name="fitness">Fitness value in string represention.</param>
        /// <param name="speed">Speed </param>
        /// <param name="all"></param>
        public void SetGenerationInfo(string fitness, string speed, string all)
        {
            fits.Add(fitness);
            elapsedTimeSpeed.Add(speed);
            allInfo.Add(all);
        }
    }
}
