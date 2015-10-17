using System;

namespace UWNLPAssignment1
{
	public class ProblemP
	{
		private readonly CorpusParsingResult _result;

		public ProblemP(CorpusParsingResult result)
		{
			_result = result;
		}

		internal enum PBucket
		{
			P1, P2, P3
		}

		internal PBucket DeterminePBucket(string wordminus2, string wordminus1, string word)
		{
			if (_result.GetCountForTrigram(wordminus2, wordminus1, word) > 0)
			{
				return PBucket.P1;
			}
			else if (_result.GetCountForBigram(wordminus1, word) > 0)
			{
				return PBucket.P2;
			}
			else
			{
				return PBucket.P3;
			}
		}

		public double P(string wordminus2, string wordminus1, string word)
		{
			throw new NotImplementedException();
		}
	}
}
