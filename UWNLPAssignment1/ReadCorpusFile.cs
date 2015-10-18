using System;
using System.IO;

namespace UWNLPAssignment1
{
	public class ReadCorpusResult
	{
		public string Training { get; set; }

		public string Development { get; set; }

		public string Evaluation { get; set; }

		public RealCorpus CorpusName { get; set; }
	}

	public enum RealCorpus
	{
		Reuters,
		Brown,
		Gutenberg
	}

	public static class ReadCorpusFile
	{
		private static string GetCorpusFileName(RealCorpus corpus)
		{
			switch (corpus)
			{
				case RealCorpus.Brown:
					return "brown.txt";
				case RealCorpus.Gutenberg:
					return "gutenberg.txt";
				case RealCorpus.Reuters:
					return "reuters.txt";
				default:
					throw new InvalidOperationException();
			}
		}

		public static ReadCorpusResult Read(RealCorpus corpus)
		{
			string corpusFileName = GetCorpusFileName(corpus);
			string corpusFilePath = Path.Combine(Environment.CurrentDirectory, "corpora/" + corpusFileName);
			string corpusFileContent = File.ReadAllText(corpusFilePath);
		
			// 80% for training
			int trainingLimit = Convert.ToInt32(corpusFileContent.Length * 0.8);
			int developmentLimit = Convert.ToInt32(corpusFileContent.Length * 0.9);
			string training = corpusFileContent.Substring(0, trainingLimit);
			string development = corpusFileContent.Substring(trainingLimit, developmentLimit - trainingLimit);
			string evaluation = corpusFileContent.Substring(developmentLimit, corpusFileContent.Length - developmentLimit);

			return new ReadCorpusResult()
			{
				Training = training,
				Development = development,
				Evaluation = evaluation,
				CorpusName = corpus
			};
		}
	}
}
