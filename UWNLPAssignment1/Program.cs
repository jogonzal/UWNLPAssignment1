using System;
using System.Collections.Generic;

namespace UWNLPAssignment1
{
	class Program
	{
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

		static void Main(string[] args)
		{
			ReadCorpusResult brownReadResults = ReadCorpusFile.Read(RealCorpus.Brown);
			ReadCorpusResult gutenbergResults = ReadCorpusFile.Read(RealCorpus.Gutenberg);
			ReadCorpusResult reutersResults = ReadCorpusFile.Read(RealCorpus.Reuters);

			CorpusParsingResult brownCorpus = CorpusParsing.ParseCorpus(brownReadResults.Training, true);
			CorpusParsingResult gutenbergCorpus = CorpusParsing.ParseCorpus(gutenbergResults.Training, true);
			CorpusParsingResult reutersCorpus = CorpusParsing.ParseCorpus(reutersResults.Training, true);

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
					RunExperimentOnCorpus(reutersResults, reutersCorpus, true)
					+ RunExperimentOnCorpus(gutenbergResults, gutenbergCorpus, true)
					+ RunExperimentOnCorpus(brownReadResults, brownCorpus, true);

				
				Console.WriteLine("{0},{1},{2} = {3}", possibility.Item1, possibility.Item2, possibility.Item3, currentPerplexity);

				if (currentPerplexity < minimumPerplexity)
				{
					minimumPerplexity = currentPerplexity;
					minimumCombination = possibility;
				}
			}

			Console.Write("Best match is {0},{1},{2} = {3}", minimumCombination.Item1, minimumCombination.Item2, minimumCombination.Item3, minimumPerplexity);

			Console.ReadLine();
		}

		private static void PrintLinearModelConfigs()
		{
			Console.WriteLine("Running linear model with properties {0} {1} {2}", Configs.Lambda1, Configs.Lambda2, Configs.Lambda3);
		}

		private static double RunExperimentOnCorpus(ReadCorpusResult readCorpusResults, CorpusParsingResult corpusParsignResult, bool development)
		{
			Console.WriteLine("Running experiment on {0} - {1}", readCorpusResults.CorpusName, development ? "development" : "evaluation");

			Console.WriteLine("{0}\tProblem", readCorpusResults.CorpusName.ToString());

			ILanguageModel model = new LinearModel(corpusParsignResult);
			Console.WriteLine("{0}\tModel", model.GetModelName());
			StringParsingResult testCorpus = CorpusParsing.ParseString(development ? readCorpusResults.Development : readCorpusResults.Evaluation);
			double perplexity = Perplexity.CalculatePerplexity(model, corpusParsignResult, testCorpus);

			Console.WriteLine("{0}\tPerplexity", perplexity);
			Console.WriteLine("============================================================");

			return perplexity;
		}
	}
}
