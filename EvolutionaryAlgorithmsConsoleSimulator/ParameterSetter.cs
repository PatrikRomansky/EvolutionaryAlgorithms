using EvolutionaryAlgorithms.Algorithms;
using EvolutionaryAlgorithms.ElitistPrivileges;
using EvolutionaryAlgorithms.Operators.Mutations;
using EvolutionaryAlgorithms.Operators.Xovers;
using EvolutionaryAlgorithms.ProblemsConfig;
using EvolutionaryAlgorithms.Selections;
using EvolutionaryAlgorithms.Terminations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace EVAConsoleImageSimulator
{

    /// <summary>
    /// Static class for set parameters.
    /// </summary>
    public static class ParameterSetter
    {
        /// <summary>
        /// Reading a parameter that should be int (greater than min).
        /// Loading until it succeeds.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="min">Minimum value.</param>
        /// <returns>Parameter value.</returns>
        public static int SetPositiveIntParameter(string name, int min)
        {
            Console.Write(name + " (>=" + min + "): ");
            int parameter;
            try
            {
                var stringNum = Console.ReadLine();

                parameter = int.Parse(stringNum);

            }
            catch
            {
                Console.WriteLine(name + " is positive integer.");
                parameter = SetPositiveIntParameter(name, min);
            }

            if (parameter < min)
            {
                Console.WriteLine(name + " must be greater than or equal to" + min + ".");
                parameter = SetPositiveIntParameter(name, min);
            }

            return parameter;
        }

        /// <summary>
        /// Reading a parameter that should be bool.
        /// Loading until it succeeds.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <returns>Parameter value.</returns>
        public static bool SetBool(string name)
        {
            Console.Write(name + "[True/False]: ");
            bool parameter;
            try
            {
                var stringNum = Console.ReadLine();

                parameter = bool.Parse(stringNum);

            }
            catch
            {
                Console.WriteLine(name + " is boolean{True, False}.");
                parameter = SetBool(name);
            }
            return parameter;
        }
            
        /// <summary>
        /// Reading a parameter that should be float.
        /// Loading until it succeeds.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>Parameter value.</returns>
        public static float SetFloat(string name, float min, float max)
        {
            Console.Write(name + " [" + min + "..." + max + "]: ");
            
            float parameter;
            try
            {
                var stringNum = Console.ReadLine();

                parameter = float.Parse(stringNum);

            }
            catch
            {
                Console.WriteLine("Wrong " + name + " parameter.");
                parameter = SetFloat(name, min, max);
            }

            if (parameter < min || parameter > max)
            {
                Console.WriteLine(name + " must be in interval [" + min + "..." + max + "].");
                parameter = SetFloat(name, min, max);
            }

            return parameter;
        }

        /// <summary>
        /// Reading a parameter that should be float.
        /// Loading until it succeeds.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <returns>Parameter value.</returns>
        public static string SetFileNameParameter(string fileType)
        {
            string result;
            Console.Write("Input file name (" + fileType +"): ");

            var inputFile = Console.ReadLine();
            inputFile = inputFile.Replace("\\", "/");

            if (File.Exists(inputFile))
            {
                result = inputFile;

            }
            else
            {
                Console.WriteLine("File not exist");
                result = SetFileNameParameter(fileType);
            }

            return result;

        }

        /// <summary>
        /// Selects all non-abstract classes implementing the interface TInterface.
        /// </summary>
        /// <typeparam name="TInterface">Interfface.</typeparam>
        /// <returns>Non-abstract classe.</returns>
        public static Dictionary<string, Type> SelectType<TInterface>()
        {
            var result = new Dictionary<string, Type>();

            var type = typeof(TInterface);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) & p.IsClass & !p.IsAbstract).ToArray();

            foreach (var t in types)
            {
                result.Add(t.Name, t);
            }

            return result;
        }

        /// <summary>
        /// Displays the controllers and the user can choose the controller.
        /// The selection is made by a serial number.
        /// </summary>
        /// <returns>Controller type.</returns>
        public static Type SetProblemCOofig()
        {
            Type problemConfig;

            try
            {
                Dictionary<string, Type> possibleProblemConfigs = new Dictionary<string, Type>();

                // Possible controllers
                possibleProblemConfigs = SelectType<IProblemConfig>();
                Console.WriteLine("Select problem's config: ");

                // show controllers
                int i = 1;
                foreach (var c in possibleProblemConfigs.Keys.ToArray())
                    Console.WriteLine((i++) + ". " + c);

                Console.Write("Problem's config number: ");

                // choose number of controller
                var StringNUm = Console.ReadLine();
                int number;
                number = int.Parse(StringNUm);

                problemConfig = possibleProblemConfigs.Values.ToList()[number - 1];
            }
            catch
            {
                Console.WriteLine("Select problem's config.");
                problemConfig = SetProblemCOofig();
            }

            return problemConfig;
        }

        /// <summary>
        /// Displays the operators and the user can choose the one of them.
        /// The selection is made by a serial number.
        /// </summary>
        /// <param name="operators">Possible operators for the selected type of operator and controller./param>
        /// <param name="operatorName">Operator type name.</param>
        /// <returns></returns>
        private static string PrintSelectedOperators(string[] operators, string operatorName)
        {
            Console.WriteLine(operatorName);
            int i = 1;

            foreach (var o in operators)
                Console.WriteLine((i++) + ". " + o);

            Console.Write("Selected: ");
            string slectedOperatorName;
            try
            {
                var stringNum = Console.ReadLine();
                int number;
                number = int.Parse(stringNum);
                slectedOperatorName = operators[number - 1];
            }
            catch
            {
                Console.WriteLine("Wrong " + operatorName + ".");
                slectedOperatorName = PrintSelectedOperators(operators, operatorName);
            }

            return slectedOperatorName;
        }

        /// <summary>
        /// Displays the crossovers and the user can choose the one of them.
        /// The selection is made by a serial number.
        /// </summary>
        /// <param name="problemConfig">Problem controller.</param>
        /// <returns>Selected crossover.</returns>
        public static IXover SetXover(IProblemConfig problemConfig)
        {
            var operators = problemConfig.PossibleXovers();

            var name = PrintSelectedOperators(operators, "Crossover");

            return problemConfig.CreateXover(name);
        }

        /// <summary>
        /// Displays the elitizmuses and the user can choose the one of them.
        /// The selection is made by a serial number.
        /// </summary>
        /// <param name="problemConfig">Problem controller.</param>
        /// <returns>Selected elitizmus.</returns>
        public static IElite SetElite(IProblemConfig problemConfig)
        {
            var operators = problemConfig.PossibleElities();

            var name = PrintSelectedOperators(operators, "Elitizmus");


            var paramter = SetFloat("Elite percentage", 0, 1);

            return problemConfig.CreateElite(name, paramter);
        }

        /// <summary>
        /// Displays the mutations and the user can choose the one of them.
        /// The selection is made by a serial number.
        /// </summary>
        /// <param name="problemConfig">Problem controller.</param>
        /// <returns>Selected mutation.</returns>
        public static IMutation SetMutation(IProblemConfig problemConfig)
        {
            var mutations = problemConfig.PossibleMutations();

            var mutName = PrintSelectedOperators(mutations, "Mutation");


            return problemConfig.CreateMutation(mutName);
        }

        /// <summary>
        /// Displays the selections and the user can choose the one of them.
        /// </summary>
        /// <param name="problemConfig">Problem controller.</param>
        /// <returns>Selected selection.</returns>
        public static ISelection SetSelection(IProblemConfig problemConfig)
        {
            var operators = problemConfig.PossibleSelections();

            var name = PrintSelectedOperators(operators, "Selection");

            return problemConfig.CreateSelection(name);
        }

        /// <summary>
        /// Displays the terminations and the user can choose the one of them.
        /// </summary>
        /// <param name="problemConfig">Problem controller.</param>
        /// <returns>Selected termination.</returns>
        public static ITermination SetTermination(IProblemConfig problemConfig)
        {
            var operators = problemConfig.PossibleTerminations();

            // possible terminations
            var name = PrintSelectedOperators(operators, "Termination");

            // end point of the condition
            var parameter = SetPositiveIntParameter("Termination parameter", 1);

            return problemConfig.CreateTermination(name, parameter);
        }

        /// <summary>
        /// Displays the evolutionary strategies and the user can choose the ES.
        /// The selection is made by a serial number.
        /// </summary>
        /// <returns>ES type.</returns>
        public static Type SetEvaES()
        {
            Type evaES;

            try
            {
                Dictionary<string, Type> possibleES = new Dictionary<string, Type>();

                // Possible controllers
                possibleES = SelectType<EVA>();
                Console.WriteLine("ES: ");

                // show controllers
                int i = 1;
                foreach (var c in possibleES.Keys.ToArray())
                    Console.WriteLine((i++) + ". " + c);

                Console.Write("ES number: ");

                // choose number of controller
                var StringNUm = Console.ReadLine();
                int number;
                number = int.Parse(StringNUm);

                evaES = possibleES.Values.ToList()[number - 1];
            }
            catch
            {
                Console.WriteLine("Chosse ES.");
                evaES = SetEvaES();
            }

            return evaES;
        }
    }
}
