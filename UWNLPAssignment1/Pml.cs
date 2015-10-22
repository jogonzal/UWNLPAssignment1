namespace UWNLPAssignment1
{
	public static class PmlExtensions
	{
		public static double Pml(this CorpusParsingResult result, string word)
		{
			int countForUnigram = result.GetCountForUnigram(word);

			return 1.0 * countForUnigram/result.TotalUnigrams;
		}

		public static double Pml(this CorpusParsingResult result, string wordminus1, string word)
		{
			int countForBigram = result.GetCountForBigram(wordminus1, word);
			int countForUnigram = result.GetCountForUnigram(wordminus1);

			return 1.0 * countForBigram / countForUnigram;
		}

		public static double Pml(this CorpusParsingResult result, string wordminus2, string wordminus1, string word)
		{
			int countForTrigram = result.GetCountForTrigram(wordminus2, wordminus1, word);
			int countForBigram = result.GetCountForBigram(wordminus2, wordminus1);

			return 1.0 * countForTrigram / countForBigram;
		}

		public static double PmlRedefined(this CorpusParsingResult result, string wordminus2, string wordminus1, string word)
		{
			double countForTrigram = result.GetCountForTrigram(wordminus2, wordminus1, word) - Configs.Beta;
			int countForBigram = result.GetCountForBigram(wordminus2, wordminus1);

			return 1.0 * countForTrigram / countForBigram;
		}

		public static double PmlRedefined(this CorpusParsingResult result, string wordminus1, string word)
		{
			double countForTrigram = result.GetCountForBigram(wordminus1, word) - Configs.Beta;
			int countForBigram = result.GetCountForUnigram(wordminus1);

			return 1.0 * countForTrigram / countForBigram;
		}
	}
}
