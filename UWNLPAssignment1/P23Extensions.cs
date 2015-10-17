namespace UWNLPAssignment1
{
	public static class P23Extensions
	{
		public static double P2(this CorpusParsingResult result, string wordminus2, string wordminus1, string word)
		{
			double pmlBigramAbove = result.Pml(wordminus1, word);

			double sumBelow = 0;
			// Iterate summing everything in "B(wi-1)", which is all the words that DON'T have bigrams with wi-1
			foreach (var potentialWordWithoutTrigram in result.UniqueWords)
			{
				if (result.GetCountForTrigram(wordminus2, wordminus1, potentialWordWithoutTrigram) == 0)
				{
					string wordWithoutTrigram = potentialWordWithoutTrigram;
					sumBelow += result.Pml(wordminus1, wordWithoutTrigram);
				}
			}

			return pmlBigramAbove / sumBelow;
		}

		public static double P3(this CorpusParsingResult result, string wordminus2, string wordminus1, string word)
		{
			double pmlBigramAbove = result.Pml(word);

			double sumBelow = 0;
			// Iterate summing everything in "B(wi-1)", which is all the words that DON'T have bigrams with wi-1
			foreach (var potentialWordWithoutBigram in result.UniqueWords)
			{
				if (result.GetCountForBigram(wordminus1, potentialWordWithoutBigram) == 0)
				{
					string wordWithoutBigram = potentialWordWithoutBigram;
					sumBelow += result.Pml(wordWithoutBigram);
				}
			}

			return pmlBigramAbove/sumBelow;
		}
	}
}
