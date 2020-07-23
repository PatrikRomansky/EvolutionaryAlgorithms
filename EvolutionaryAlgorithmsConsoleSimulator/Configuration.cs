﻿using EvolutionaryAlgorithms.Algorithms;
using EvolutionaryAlgorithms.Individuals;
using EvolutionaryAlgorithms.Populations;
using EvolutionaryAlgorithms.ProblemsConfig;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace EVAConsoleImageSimulator
{
    public class Configuration
    {
        public static IEVA[] SetEVA(IProblemConfig[] problemConfig)
        {
            // Supported EVA types
            Console.WriteLine("Select:");
            Console.WriteLine("1. GA");
            Console.WriteLine("2. ES");

            IEVA[] eva;

            Console.Write("EVA: ");

            var index = Console.ReadLine();

            switch (index)
            {
                case ("1"):
                    eva = SetGA(problemConfig);
                    break;
                case ("2"):
                    eva = SetES(problemConfig);
                    break;
                default:
                    Console.WriteLine("Wrong selection.");
                    eva = SetEVA(problemConfig);
                    break;
            }

            return eva;
        }

        public static IEVA[] SetGA(IProblemConfig[] problemConfig)
        {
            Console.WriteLine("Custom Genetic algorithm.");
            var termination = ParameterSetter.SetTermination(problemConfig[0]);
            var crossover = ParameterSetter.SetXover(problemConfig[0]);
            var xoverProb = ParameterSetter.SetFloat("Xover Probability", 0, 1);

            var mutation = ParameterSetter.SetMutation(problemConfig[0]);
            var mutationProb = ParameterSetter.SetFloat("Mutation Probability", 0, 1);

            var selection = ParameterSetter.SetSelection(problemConfig[0]);
            var elitizmus = ParameterSetter.SetElite(problemConfig[0]);

            var executor = problemConfig[0].CreateExecutor();
            var popSize = ParameterSetter.SetPositiveIntParameter("Pop size", 2);

            IEVA[] evas = new IEVA[problemConfig.Length];

            int i = 0;
            foreach (var config in problemConfig)
            {
                // creates population with curr. popSize
                var population = new Population(popSize, config.CreateIndividual, config.CreateEmptyIndividual);

                // new operators
                var fitness = config.CreateFitness();

                evas[i++] = new GeneticAlgorithm(population, fitness, selection, crossover, mutation, elitizmus, termination, executor, mutationProb, xoverProb);

            }

            return evas;
        }

        public static IEVA[] SetES(IProblemConfig[] problemConfig)
        {
            Console.WriteLine("Evolutionary Strategy.");
            var evaName = ParameterSetter.SetEvaES();

            // CMAES - internal population size setter
            var popSize = 20;

            IEVA[] evas = new IEVA[problemConfig.Length];

            int i = 0;
            foreach (var config in problemConfig)
            {
                // creates population with curr. popSize
                var population = new Population(popSize, config.CreateIndividual, config.CreateEmptyIndividual);
                // new operators
                var fitness = config.CreateFitness();
                // Delegate.CreateDelegate(consoleProblemConfig.CreateIndividual);
                evas[i++] = Activator.CreateInstance(evaName, fitness, population) as IEVA;
            }

            return evas;
        }

        public void SetGenerationInfoLog(IEVA eva, IProblemConfig consoleProblemConfig, int logRate)
        {
            eva.CurrentGenerationInfo += delegate
            {
                var bestIndividual = eva.BestIndividual;
                Console.WriteLine("Generations: {0}", eva.CurrentGenerationsNumber);
                Console.WriteLine("Fitness: {0,10}", bestIndividual.Fitness);
                Console.WriteLine("Time: {0}", eva.TimeEvolving);

                var speed = eva.TimeEvolving.TotalSeconds / eva.CurrentGenerationsNumber;
                Console.WriteLine("Speed (gen/sec): {0:0.0000}", speed);

                var best = consoleProblemConfig.ShowBestIndividual(bestIndividual, logRate);

                var fit = eva.CurrentGenerationsNumber + ";" + (1 / bestIndividual.Fitness);
                var elapsedTimeSpeed = eva.CurrentGenerationsNumber + ";" + speed;
                var allInfo = eva.CurrentGenerationsNumber + ";" + (1 / bestIndividual.Fitness) + ";" + eva.TimeEvolving + ";" + speed;
                consoleProblemConfig.SetGenerationInfo(fit, elapsedTimeSpeed, allInfo);
            };
        }

        public static string CreateDirectory(string path, string subName)
        {
            var inputImageFile = path;
            var folder = Path.Combine(Path.GetDirectoryName(inputImageFile), subName);
            var filePath = inputImageFile.Split('/');
            var fileName = filePath[filePath.Length - 1].Split('.')[0];
            var m_destFolder = folder + "_" + fileName + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
            Directory.CreateDirectory(m_destFolder);

            return m_destFolder;
        }
    }
}
