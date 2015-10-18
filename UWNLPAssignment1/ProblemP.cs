using System;
using System.Collections.Generic;
using System.Diagnostics;

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
			PBucket bucket = DeterminePBucket(wordminus2, wordminus1, word);
			double pml;
			switch (bucket)
			{
				case PBucket.P1:
					pml = _result.PmlRedefined(wordminus2, wordminus1, word);
					break;
				case PBucket.P2:
					pml = P2RedefinedRedefined(wordminus2, wordminus1, word);
					break;
				case PBucket.P3:
					pml = P3RedefinedRedefined(wordminus2, wordminus1, word);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			Debug.Print("{0}  {1}  {2}\t{3}\t{4}", bucket, pml, wordminus2, wordminus1, word);

			return pml;
		}

		private double P3RedefinedRedefined(string wordminus2, string wordminus1, string word)
		{
			double p3Redefined = P3Redefined(wordminus2, wordminus1, word);

			double totalSumForP2 = GetTotalSumForP2(wordminus2, wordminus1);
			double alpha = (1 - 0.5 * totalSumForP2);

			return alpha * p3Redefined;
		}

		private double P2RedefinedRedefined(string wordminus2, string wordminus1, string word)
		{
			double p2Redefined = P2Redefined(wordminus2, wordminus1, word);

			double totalSumForP3 = GetTotalSumForP3(wordminus2, wordminus1);
			double alpha = (1 - 0.5 * totalSumForP3);

			return alpha * p2Redefined;
		}

		private double P2Redefined(string wordminus2, string wordminus1, string word)
		{
			double originalP2 = _result.P2(wordminus2, wordminus1, word);

			double totalSumForP1Redefined = GetTotalSumForP1Redefined(wordminus2, wordminus1);
			double alpha = (1 - totalSumForP1Redefined);

			return alpha * originalP2;
		}

		private double P3Redefined(string wordminus2, string wordminus1, string word)
		{
			double originalP3 = _result.P3(wordminus2, wordminus1, word);

			double totalSumForP1Redefined = GetTotalSumForP1Redefined(wordminus2, wordminus1);
			double alpha = (1 - totalSumForP1Redefined);

			return alpha * originalP3;
		}

		private readonly Dictionary<Tuple<string, string>, double> _totalSumForP1RedefinedCache = new Dictionary<Tuple<string, string>, double>(); 

		private double GetTotalSumForP1Redefined(string wordminus2, string wordminus1)
		{
			// Try to get the total sum from the cache. If not, then calculate it and add it to the cache
			var tuple = new Tuple<string, string>(wordminus2, wordminus1);
			double totalSum;
			if (_totalSumForP1RedefinedCache.TryGetValue(tuple, out totalSum))
			{
				return totalSum;
			}

			totalSum = 0;
			foreach (var wordIteration in _result.UniqueWords)
			{
				if (DeterminePBucket(wordminus2, wordminus1, wordIteration) == PBucket.P1)
				{
					totalSum += _result.PmlRedefined(wordminus2, wordminus1, wordIteration);
				}
			}

			_totalSumForP1RedefinedCache[tuple] = totalSum;

			return totalSum;
		}

		private readonly Dictionary<Tuple<string, string>, double> _totalSumForP2Cache = new Dictionary<Tuple<string, string>, double>();

		private double GetTotalSumForP2(string wordminus2, string wordminus1)
		{
			// Try to get the total sum from the cache. If not, then calculate it and add it to the cache
			var tuple = new Tuple<string, string>(wordminus2, wordminus1);
			double totalSum;
			if (_totalSumForP2Cache.TryGetValue(tuple, out totalSum))
			{
				return totalSum;
			}

			totalSum = 0;
			foreach (var wordIteration in _result.UniqueWords)
			{
				if (DeterminePBucket(wordminus2, wordminus1, wordIteration) == PBucket.P2)
				{
					totalSum += _result.P2(wordminus2, wordminus1, wordIteration);
				}
			}

			_totalSumForP2Cache[tuple] = totalSum;

			return totalSum;
		}

		private readonly Dictionary<Tuple<string, string>, double> _totalSumForP3Cache = new Dictionary<Tuple<string, string>, double>();

		private double GetTotalSumForP3(string wordminus2, string wordminus1)
		{
			// Try to get the total sum from the cache. If not, then calculate it and add it to the cache
			var tuple = new Tuple<string, string>(wordminus2, wordminus1);
			double totalSum;
			if (_totalSumForP3Cache.TryGetValue(tuple, out totalSum))
			{
				return totalSum;
			}

			totalSum = 0;
			foreach (var wordIteration in _result.UniqueWords)
			{
				if (DeterminePBucket(wordminus2, wordminus1, wordIteration) == PBucket.P3)
				{
					totalSum += _result.P3(wordminus2, wordminus1, wordIteration);
				}
			}

			_totalSumForP3Cache[tuple] = totalSum;

			return totalSum;
		}

	}
}
