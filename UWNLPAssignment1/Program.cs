using System;

namespace UWNLPAssignment1
{
	class Program
	{
		static void Main(string[] args)
		{
			var result = CorpusParsing.ParseCorpus(SampleCorpus.TwoDogSentences);
			Console.WriteLine(result.PrettyPrint());
			Console.WriteLine(result.PrettyPrintUnigrams());
			Console.WriteLine(result.PrettyPrintBigrams());
			Console.WriteLine(result.PrettyPrintTrigrams());
			Console.ReadLine();

			var fileReadResults = ReadCorpusFile.Read(RealCorpus.Brown);
		}
	}
}
