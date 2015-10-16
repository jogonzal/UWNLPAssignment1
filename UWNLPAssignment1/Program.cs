using System;

namespace UWNLPAssignment1
{
	class Program
	{
		static void Main(string[] args)
		{
			var fileReadResults = ReadCorpusFile.Read(RealCorpus.Brown);

			var result = CorpusParsing.ParseCorpus(fileReadResults.Training);
			Console.WriteLine(result.PrettyPrint());
			Console.WriteLine(result.PrettyPrintUnigrams());
			Console.WriteLine(result.PrettyPrintBigrams());
			Console.WriteLine(result.PrettyPrintTrigrams());
			Console.ReadLine();
		}
	}
}
