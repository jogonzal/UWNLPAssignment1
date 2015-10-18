using System;

namespace UWNLPAssignment1
{
	class Program
	{
		static void Main(string[] args)
		{
			ReadCorpusResult fileReadResults = ReadCorpusFile.Read(RealCorpus.Brown);

			CorpusParsingResult corpus = CorpusParsing.ParseCorpus(fileReadResults.Training, true);
			Console.WriteLine(corpus.PrettyPrint());

			//Console.WriteLine(result.PrettyPrintUnigrams());
			//Console.WriteLine(result.PrettyPrintBigrams());
			//Console.WriteLine(result.PrettyPrintTrigrams());

			ILanguageModel model = new LinearModel(corpus);
			var testCorpus = CorpusParsing.ParseString(fileReadResults.Evaluation);
			double perplexity = Perplexity.CalculatePerplexity(model, corpus, testCorpus);
			Console.WriteLine("{0}\tPerplexity", perplexity);

			Console.ReadLine();
		}
	}
}
