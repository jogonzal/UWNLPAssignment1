using System;
using System.Linq;
using System.Text;

namespace UWNLPAssignment1
{
	public static class PrintExtensions
	{
		public static string PrettyPrint(this CorpusParsingResult result)
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine("=============================================");
			sb.AppendLine("PRETTY PRINT FOR PARSING RESULT");
			sb.AppendFormat("{0}\tSentences", result.Sentences.Count);
			sb.AppendLine();
			sb.AppendFormat("{0}\tWords", result.TotalWordCount);
			sb.AppendLine();
			sb.AppendFormat("{0}\tUnique words", result.UniqueWords.Count);
			sb.AppendLine();
			sb.AppendFormat("{0}\tUnigrams", result.TotalUnigrams);
			sb.AppendLine();
			sb.AppendFormat("{0}\tBigrams", result.TotalBigrams);
			sb.AppendLine();
			sb.AppendFormat("{0}\tTrigrams", result.TotalTrigrams);
			sb.AppendLine();
			sb.AppendLine("=============================================");

			return sb.ToString();
		}

		public static string PrettyPrintUnigrams(this CorpusParsingResult result)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("=============================================");
			sb.AppendFormat("UNIGRAM RESULTS ({0}){1}", result.TotalUnigrams, Environment.NewLine);

			var sortedUnigrams = result.Unigrams.OrderBy(u => u.Key);

			foreach (var unigram in sortedUnigrams)
			{
				string word = unigram.Key;
				int occurrences = unigram.Value;
				sb.AppendFormat("{0}\t{1}{2}", occurrences, word, Environment.NewLine);
			}

			sb.AppendLine("=============================================");
			return sb.ToString();
		}

		public static string PrettyPrintBigrams(this CorpusParsingResult result)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("=============================================");
			sb.AppendFormat("BIGRAM RESULTS ({0}){1}", result.TotalBigrams, Environment.NewLine);

			var sortedBigrams = result.Bigrams.OrderBy(u => u.Key.Item1);

			foreach (var bigram in sortedBigrams)
			{
				string word = bigram.Key.Item1;
				string word2 = bigram.Key.Item2;
				int occurrences = bigram.Value;
				sb.AppendFormat("{0}\t{1}\t{2}{3}", occurrences, word, word2, Environment.NewLine);
			}

			sb.AppendLine("=============================================");
			return sb.ToString();
		}

		public static string PrettyPrintTrigrams(this CorpusParsingResult result)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("=============================================");
			sb.AppendFormat("TRIGRAM RESULTS ({0}){1}", result.TotalTrigrams, Environment.NewLine);

			var sortedTrigrams = result.Trigrams.OrderBy(u => u.Key.Item1);

			foreach (var trigram in sortedTrigrams)
			{
				string word = trigram.Key.Item1;
				string word2 = trigram.Key.Item2;
				string word3 = trigram.Key.Item3;
				int occurrences = trigram.Value;
				sb.AppendFormat("{0}\t{1}\t{2}\t{3}{4}", occurrences, word, word2, word3, Environment.NewLine);
			}

			sb.AppendLine("=============================================");
			return sb.ToString();
		}
	}
}
