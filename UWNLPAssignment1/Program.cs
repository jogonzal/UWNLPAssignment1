using System;

namespace UWNLPAssignment1
{
	class Program
	{
		static void Main(string[] args)
		{
			ReadCorpusResult fileReadResults = ReadCorpusFile.Read(RealCorpus.Brown);

			CorpusParsingResult result = CorpusParsing.ParseCorpus(fileReadResults.Training);
			Console.WriteLine(result.PrettyPrint());

			//Console.WriteLine(result.PrettyPrintUnigrams());
			//Console.WriteLine(result.PrettyPrintBigrams());
			//Console.WriteLine(result.PrettyPrintTrigrams());

			ILanguageModel model = new LinearModel(result);

			

			Console.ReadLine();
		}
	}
}
