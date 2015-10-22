using System;
using System.Collections.Generic;

namespace UWNLPAssignment1
{
	class Program
	{
		static void Main(string[] args)
		{
			PrintInstructions();

			int option;
			while (!int.TryParse(Console.ReadLine(), out option) || option < 1 || option > 5)
			{
				Console.WriteLine("Invalid option!");
				PrintInstructions();
			}

			switch (option)
			{
				case 1:
					EvaluateProblem1ModelOnAll3Corpora();
					break;
				case 2:
					EvaluateLinearModelOnAll3Corpora();
					break;
				case 3:
					FindBestLambdasForLinearModel();
					break;
				case 4:
					EvaluateCrossDomainPerplexitiesOnLinearModel();
					break;
				case 5:
					BonusQuestion();
					break;
			}

			Console.WriteLine("Done! Yay!");
			Console.ReadLine();
		}

		private static void PrintInstructions()
		{
			Console.WriteLine("Options:");
			Console.WriteLine("1\tProblem2.Question1: Evaluate Problem1Model on all 3 corpora");
			Console.WriteLine("2\tProblem2.Question2: Evaluate linear model on all 3 corpora");
			Console.WriteLine("3\tProblem2.Question4: Find best lambdas for linear model");
			Console.WriteLine("4\tProblem2.Question5: Perplexities on cross domain");
			Console.WriteLine("5\tBONUS: Modify model based on small fraction of target domain");
		}

		#region Linear Model

		private static void EvaluateCrossDomainPerplexitiesOnLinearModel()
		{
			ReadCorpusResult brownReadResults = ReadCorpusFile.Read(RealCorpus.Brown);
			ReadCorpusResult gutenbergResults = ReadCorpusFile.Read(RealCorpus.Gutenberg);
			ReadCorpusResult reutersResults = ReadCorpusFile.Read(RealCorpus.Reuters);

			CorpusParsingResult brownCorpus = CorpusParsing.ParseCorpus(brownReadResults, true);
			CorpusParsingResult gutenbergCorpus = CorpusParsing.ParseCorpus(gutenbergResults, true);
			CorpusParsingResult reutersCorpus = CorpusParsing.ParseCorpus(reutersResults, true);

			CalculateLinearModelPerplexityOnTestCorpus(gutenbergCorpus, brownReadResults, false);
			CalculateLinearModelPerplexityOnTestCorpus(reutersCorpus, gutenbergResults, false);
			CalculateLinearModelPerplexityOnTestCorpus(brownCorpus, reutersResults, false);
		}

		private static void EvaluateLinearModelOnAll3Corpora()
		{
			ReadCorpusResult brownReadResults = ReadCorpusFile.Read(RealCorpus.Brown);
			ReadCorpusResult gutenbergResults = ReadCorpusFile.Read(RealCorpus.Gutenberg);
			ReadCorpusResult reutersResults = ReadCorpusFile.Read(RealCorpus.Reuters);

			CorpusParsingResult brownCorpus = CorpusParsing.ParseCorpus(brownReadResults, true);
			CorpusParsingResult gutenbergCorpus = CorpusParsing.ParseCorpus(gutenbergResults, true);
			CorpusParsingResult reutersCorpus = CorpusParsing.ParseCorpus(reutersResults, true);

			CalculateLinearModelPerplexityOnTestCorpus(brownCorpus, brownReadResults, false);
			CalculateLinearModelPerplexityOnTestCorpus(gutenbergCorpus, gutenbergResults, false);
			CalculateLinearModelPerplexityOnTestCorpus(reutersCorpus, reutersResults, false);
		}

		private static readonly List<Tuple<double, double, double>> Possibilities = new List<Tuple<double, double, double>>
		{
			new Tuple<double, double, double>(0.3, 0.3, 0.4),
			new Tuple<double, double, double>(0.4, 0.3, 0.3),
			new Tuple<double, double, double>(0.3, 0.4, 0.3),
			new Tuple<double, double, double>(0.2, 0.4, 0.4),
			new Tuple<double, double, double>(0.4, 0.4, 0.2),
			new Tuple<double, double, double>(0.2, 0.4, 0.4),
			new Tuple<double, double, double>(0.1, 0.5, 0.4),
			new Tuple<double, double, double>(0.1, 0.4, 0.5),
			new Tuple<double, double, double>(0.5, 0.4, 0.1),
			new Tuple<double, double, double>(0.6, 0.2, 0.2),
			new Tuple<double, double, double>(0.2, 0.6, 0.2),
			new Tuple<double, double, double>(0.2, 0.2, 0.6),
			new Tuple<double, double, double>(0.8, 0.1, 0.1),
			new Tuple<double, double, double>(0.1, 0.8, 0.1),
			new Tuple<double, double, double>(0.1, 0.1, 0.8),
		};

		private static void FindBestLambdasForLinearModel()
		{
			ReadCorpusResult brownReadResults = ReadCorpusFile.Read(RealCorpus.Brown);
			ReadCorpusResult gutenbergResults = ReadCorpusFile.Read(RealCorpus.Gutenberg);
			ReadCorpusResult reutersResults = ReadCorpusFile.Read(RealCorpus.Reuters);

			CorpusParsingResult brownCorpus = CorpusParsing.ParseCorpus(brownReadResults, true);
			CorpusParsingResult gutenbergCorpus = CorpusParsing.ParseCorpus(gutenbergResults, true);
			CorpusParsingResult reutersCorpus = CorpusParsing.ParseCorpus(reutersResults, true);

			Console.WriteLine(brownCorpus.PrettyPrint());
			Console.WriteLine(gutenbergCorpus.PrettyPrint());
			Console.WriteLine(reutersCorpus.PrettyPrint());

			double minimumPerplexity = 90000000;
			Tuple<double, double, double> minimumCombination = new Tuple<double, double, double>(0, 0, 0);
			// From a set of linear combinations, find the best one
			foreach (var possibility in Possibilities)
			{
				Configs.Lambda1 = possibility.Item1;
				Configs.Lambda2 = possibility.Item2;
				Configs.Lambda3 = possibility.Item3;

				PrintLinearModelConfigs();

				double currentPerplexity =
					CalculateLinearModelPerplexityOnTestCorpus(reutersCorpus, reutersResults, true)
					+ CalculateLinearModelPerplexityOnTestCorpus(gutenbergCorpus, gutenbergResults, true)
					+ CalculateLinearModelPerplexityOnTestCorpus(brownCorpus, brownReadResults, true);


				Console.WriteLine("{0},{1},{2} = {3}", possibility.Item1, possibility.Item2, possibility.Item3, currentPerplexity);

				if (currentPerplexity < minimumPerplexity)
				{
					minimumPerplexity = currentPerplexity;
					minimumCombination = possibility;
				}
			}

			Console.Write("Best match is {0},{1},{2} = {3}", minimumCombination.Item1, minimumCombination.Item2, minimumCombination.Item3, minimumPerplexity);
		}

		private static void PrintLinearModelConfigs()
		{
			Console.WriteLine("Running linear model with properties {0} {1} {2}", Configs.Lambda1, Configs.Lambda2, Configs.Lambda3);
		}

		private static double CalculateLinearModelPerplexityOnTestCorpus(CorpusParsingResult trainingCorpus, ReadCorpusResult evaluatingCorpus, bool development)
		{
			Console.WriteLine("Calculating Perplexity after training on {0}", trainingCorpus.CorpusName);
			Console.WriteLine("Calculating perplexity for {0}", evaluatingCorpus.CorpusName);

			Console.WriteLine("{0}\tProblem", evaluatingCorpus.CorpusName.ToString());

			ILanguageModel model = new LinearModel(trainingCorpus);
			Console.WriteLine("{0}\tModel", model.GetModelName());
			StringParsingResult testCorpus = CorpusParsing.ParseString(development ? evaluatingCorpus.Development : evaluatingCorpus.Evaluation);
			double perplexity = CalculatePerplexityWrapper(model, trainingCorpus, testCorpus);

			Console.WriteLine("============================================================");

			return perplexity;
		}

		#endregion

		#region Problem1Model

		private static void EvaluateProblem1ModelOnAll3Corpora()
		{
			ReadCorpusResult brownReadResults = ReadCorpusFile.Read(RealCorpus.Brown);
			ReadCorpusResult gutenbergResults = ReadCorpusFile.Read(RealCorpus.Gutenberg);
			ReadCorpusResult reutersResults = ReadCorpusFile.Read(RealCorpus.Reuters);

			CorpusParsingResult brownCorpus = CorpusParsing.ParseCorpus(brownReadResults, true);
			CorpusParsingResult gutenbergCorpus = CorpusParsing.ParseCorpus(gutenbergResults, true);
			CorpusParsingResult reutersCorpus = CorpusParsing.ParseCorpus(reutersResults, true);

			CalculateProblem1ModelPerplexityOnTestCorpus(brownCorpus, brownReadResults, false);
			CalculateProblem1ModelPerplexityOnTestCorpus(gutenbergCorpus, gutenbergResults, false);
			CalculateProblem1ModelPerplexityOnTestCorpus(reutersCorpus, reutersResults, false);
		}

		private static double CalculateProblem1ModelPerplexityOnTestCorpus(CorpusParsingResult trainingCorpus, ReadCorpusResult evaluatingCorpus, bool development)
		{
			Console.WriteLine("Calculating Perplexity after training on {0}", trainingCorpus.CorpusName);
			Console.WriteLine("Calculating perplexity for {0}", evaluatingCorpus.CorpusName);

			Console.WriteLine("{0}\tProblem", evaluatingCorpus.CorpusName.ToString());

			ILanguageModel model = new Problem1Model(trainingCorpus);
			Console.WriteLine("{0}\tModel", model.GetModelName());
			StringParsingResult testCorpus = CorpusParsing.ParseString(development ? evaluatingCorpus.Development : evaluatingCorpus.Evaluation);
			double perplexity = CalculatePerplexityWrapper(model, trainingCorpus, testCorpus);

			Console.WriteLine("============================================================");

			return perplexity;
		}

		#endregion

		private static double CalculatePerplexityWrapper(ILanguageModel model, CorpusParsingResult trainingCorpus, StringParsingResult testCorpus)
		{
			Perplexity.TestStats testStats;
			double perplexity = Perplexity.CalculatePerplexity(model, trainingCorpus, testCorpus, out testStats);

			Console.WriteLine("{0}\tPerplexity", perplexity);
			Console.WriteLine("Test stats:");
			Console.WriteLine(testStats.ToString());
			return perplexity;
		}

		private static void BonusQuestion()
		{
			ReadCorpusResult brownReadResults = ReadCorpusFile.Read(RealCorpus.Brown);
			ReadCorpusResult gutenbergResults = ReadCorpusFile.Read(RealCorpus.Gutenberg);
			ReadCorpusResult reutersResults = ReadCorpusFile.Read(RealCorpus.Reuters);

			CorpusParsingResult brownCorpus = CorpusParsing.ParseCorpus(brownReadResults, true, postTrainWith: reutersResults.Development.Substring(0, reutersResults.Development.Length / 10));
			CorpusParsingResult gutenbergCorpus = CorpusParsing.ParseCorpus(gutenbergResults, true, postTrainWith: brownReadResults.Development.Substring(0, brownReadResults.Development.Length / 10));
			CorpusParsingResult reutersCorpus = CorpusParsing.ParseCorpus(reutersResults, true, postTrainWith: gutenbergResults.Development.Substring(0, gutenbergResults.Development.Length / 10));

			CalculateLinearModelPerplexityOnTestCorpus(gutenbergCorpus, brownReadResults, false);
			CalculateLinearModelPerplexityOnTestCorpus(reutersCorpus, gutenbergResults, false);
			CalculateLinearModelPerplexityOnTestCorpus(brownCorpus, reutersResults, false);
		}
	}
}
