using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWNLPAssignment1
{
	public static class PrintExtensions
	{
		public static string PrettyPrint(this CorpusParsingResult result)
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine("=============================================");
			sb.AppendLine("PRETTY PRINT FOR PARSING RESULT");
			sb.AppendFormat("\tSentences:\t{0}", result.Sentences.Count);
			sb.AppendLine();
			sb.AppendFormat("\tWords:\t{0}", result.TotalWordCount);
			sb.AppendLine();
			sb.AppendFormat("\tUnique words:\t{0}", result.UniqueWordCount);
			sb.AppendLine();
			sb.AppendLine("=============================================");

			return sb.ToString();
		}

		public static string PrintUnigrams(this CorpusParsingResult result)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("=============================================");
			sb.AppendLine("UNIGRAM RESULTS");
			foreach (var word in result.UniqueWordsIndex)
			{
				int occurrences = result.Unigrams[word.Value];
				sb.AppendFormat("{0}:\t\t{1}{2}", word.Key, occurrences, Environment.NewLine);
			}
			sb.AppendLine("=============================================");
			return sb.ToString();
		}
	}
}
