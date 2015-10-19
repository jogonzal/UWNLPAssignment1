using System.Diagnostics;

namespace UWNLPAssignment1
{
	public class LinearModel : ILanguageModel
	{
		private readonly CorpusParsingResult _result;

		public LinearModel(CorpusParsingResult result)
		{
			_result = result;
		}

		public double P(string wordminus2, string wordminus1, string word)
		{
			double probabilityForTrigram = 0, probabilityForBigram = 0, probabilityForUnigram = 0;

			if (_result.GetCountForBigram(wordminus2, wordminus1) > 0)
			{
				probabilityForTrigram = Configs.Lambda1 * _result.Pml(wordminus2, wordminus1, word);
			}

			if (_result.GetCountForUnigram(wordminus2) > 0)
			{
				probabilityForBigram = Configs.Lambda2 * _result.Pml(wordminus1, word);
			}

			probabilityForUnigram = Configs.Lambda3 * _result.Pml(word);

			double total = probabilityForTrigram + probabilityForBigram + probabilityForUnigram;

			//Debug.WriteLine("Total:{0}\tUni:\t{1}\tBig:\t{2}\tTri\t:{3}\t{4}\t{5}\t{6}",
			//	total, probabilityForTrigram, probabilityForBigram, probabilityForUnigram,
			//	wordminus2, wordminus1, word);

			return total;
		}

		public string GetModelName()
		{
			return "LinearModel";
		}
	}
}
