using System.Diagnostics;

namespace UWNLPAssignment1
{
	public class TrigramModel : ILanguageModel
	{
		public string GetModelName()
		{
			return "TrigramModel";
		}

		private readonly CorpusParsingResult _result;

		public TrigramModel(CorpusParsingResult result)
		{
			_result = result;
		}

		public double P(string wordminus2, string wordminus1, string word)
		{
			double probabilityForTrigram = 0;

			if (_result.GetCountForBigram(wordminus2, wordminus1) > 0)
			{
				probabilityForTrigram =  _result.Pml(wordminus2, wordminus1, word);
			}

			double total = probabilityForTrigram;

			Debug.WriteLine("Total:{0}\tTri\t:{1}\t{2}\t{3}\t{4}",
				total, probabilityForTrigram, wordminus2, wordminus1, word);

			return total;
		}
	}

	public class BigramModel : ILanguageModel
	{
		public string GetModelName()
		{
			return "BigramModel";
		}

		private readonly CorpusParsingResult _result;

		public BigramModel(CorpusParsingResult result)
		{
			_result = result;
		}

		public double P(string wordminus2, string wordminus1, string word)
		{
			double probabilityForBigram = 0;

			if (_result.GetCountForUnigram(wordminus1) > 0)
			{
				probabilityForBigram = _result.Pml(wordminus1, word);
			}

			double total = probabilityForBigram;

			Debug.WriteLine("Total:{0}\tBig\t:{1}\t{2}\t{3}\t{4}",
				total, probabilityForBigram, wordminus2, wordminus1, word);

			return total;
		}
	}


	public class UnigramModel : ILanguageModel
	{
		public string GetModelName()
		{
			return "UnigramModel";
		}

		private readonly CorpusParsingResult _result;

		public UnigramModel(CorpusParsingResult result)
		{
			_result = result;
		}

		public double P(string wordminus2, string wordminus1, string word)
		{
			double probabilityForUnigram = 0;

			probabilityForUnigram = _result.Pml(word);

			double total = probabilityForUnigram;

			Debug.WriteLine("Total:{0}\tUni\t:{1}\t{2}\t{3}\t{4}",
				total, probabilityForUnigram, wordminus2, wordminus1, word);

			return total;
		}
	}
}
