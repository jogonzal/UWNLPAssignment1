using System;
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
			sb.AppendFormat("{0}\tUnique words", result.UniqueWordCount);
			sb.AppendLine();
			sb.AppendLine("=============================================");

			return sb.ToString();
		}

		public static string PrettyPrintUnigrams(this CorpusParsingResult result)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("=============================================");
			sb.AppendLine("UNIGRAM RESULTS");

			for (int i = 0; i < result.UniqueWordCount; i++)
			{
				int occurrences = result.Unigrams[i];
				if (occurrences > 0)
				{
					string firstWord = result.GetWordForIndex(i);
					sb.AppendFormat("{0}\t{1}{2}", occurrences, firstWord, Environment.NewLine);
				}
			}
			sb.AppendLine("=============================================");
			return sb.ToString();
		}

		public static string PrettyPrintBigrams(this CorpusParsingResult result)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("=============================================");
			sb.AppendLine("BIGRAM RESULTS");
			
			for (int i = 0; i < result.UniqueWordCount; i++)
			{
				string firstWord = result.GetWordForIndex(i);
				for (int j = 0; j < result.UniqueWordCount; j++)
				{
					int occurrences = result.Bigrams[i, j];
					if (occurrences > 0)
					{
						string secondWord = result.GetWordForIndex(j);
						sb.AppendFormat("{0}\t{1}\t{2}{3}", occurrences, firstWord, secondWord, Environment.NewLine);
					}
				}	
			}
			sb.AppendLine("=============================================");
			return sb.ToString();
		}

		public static string PrettyPrintTrigrams(this CorpusParsingResult result)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("=============================================");
			sb.AppendLine("TRIGRAM RESULTS");

			for (int i = 0; i < result.UniqueWordCount; i++)
			{
				string firstWord = result.GetWordForIndex(i);
				for (int j = 0; j < result.UniqueWordCount; j++)
				{
					string secondWord = result.GetWordForIndex(j);
					for (int k = 0; k < result.UniqueWordCount; k++)
					{
						int occurrences = result.Trigrams[i, j, k];
						if (occurrences > 0)
						{
							string thirdWord = result.GetWordForIndex(k);
							sb.AppendFormat("{0}\t{1}\t{2}\t{3}{4}", occurrences, firstWord, secondWord, thirdWord, Environment.NewLine);
						}
					}
				}
			}
			sb.AppendLine("=============================================");
			return sb.ToString();
		}
	}
}
