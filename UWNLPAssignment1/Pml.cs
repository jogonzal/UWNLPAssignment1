namespace UWNLPAssignment1
{
	public static class PmlExtensions
	{
		public static double Pml(this CorpusParsingResult result, string word)
		{
			int countForUnigram = result.GetCountForUnigram(word);

			return 1.0*countForUnigram/result.TotalUnigrams;
		}

		public static double Pml(this CorpusParsingResult result, string word, string word2)
		{
			int countForBigram = result.GetCountForBigram(word, word2);
			int totalCountForUnigrams = result.TotalBigrams;

			return 1.0 * countForBigram / totalCountForUnigrams;
		}

		public static double Pml(this CorpusParsingResult result, string word, string word2, string word3)
		{
			int countForTrigram = result.GetCountForTrigram(word, word2, word3);

			return 1.0 * countForTrigram / result.TotalTrigrams;
		}
	}
}
