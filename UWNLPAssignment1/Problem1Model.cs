using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace UWNLPAssignment1
{
	public class Problem1Model : ILanguageModel
	{
		private readonly CorpusParsingResult _result;

		public Problem1Model(CorpusParsingResult result)
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
					pml = P2Redefined(wordminus2, wordminus1, word);
					break;
				case PBucket.P3:
					pml = P3Redefined(wordminus2, wordminus1, word);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			//Debug.Print("{0}  {1}  {2}\t{3}\t{4}", bucket, pml, wordminus2, wordminus1, word);

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
			double originalP2 = P2(wordminus2, wordminus1, word);

			double totalSumForP1Redefined = GetTotalSumForP1Redefined(wordminus2, wordminus1);
			double alpha = (1 - totalSumForP1Redefined) * 0.5;

			return alpha * originalP2;
		}

		private double P3Redefined(string wordminus2, string wordminus1, string word)
		{
			double originalP3 = P3(wordminus2, wordminus1, word);

			double totalSumForP1Redefined = GetTotalSumForP1Redefined(wordminus2, wordminus1);
			double alpha = (1 - totalSumForP1Redefined) * 0.5;

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
			foreach (var wordIteration in _result.UniqueWords.Keys)
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
			for (int i = 0; i < _result.UniqueWords.Count; i++)
			{
				var wordIteration = _result.UniqueWords.Keys.ElementAt(i);
				if (DeterminePBucket(wordminus2, wordminus1, wordIteration) == PBucket.P2)
				{
					totalSum += P2(wordminus2, wordminus1, wordIteration);
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
			for (int i = 0; i < _result.UniqueWords.Count; i++)
			{
				var wordIteration = _result.UniqueWords.Keys.ElementAt(i);
				if (DeterminePBucket(wordminus2, wordminus1, wordIteration) == PBucket.P3)
				{
					totalSum += P3(wordminus2, wordminus1, wordIteration);
				}
			}

			_totalSumForP3Cache[tuple] = totalSum;

			return totalSum;
		}

		public double P2(string wordminus2, string wordminus1, string word)
		{
			double pmlBigramAbove = _result.Pml(wordminus1, word);

			var sumBelow = SumForP2Factor(wordminus2, wordminus1);

			return pmlBigramAbove / sumBelow;
		}

		private readonly Dictionary<Tuple<string, string>, double> _sumForP2FactorCache = new Dictionary<Tuple<string, string>, double>(); 

		private double SumForP2Factor(string wordminus2, string wordminus1)
		{
			double sumBelow;
			var key = new Tuple<string, string>(wordminus2, wordminus1);
			if (_sumForP2FactorCache.TryGetValue(key, out sumBelow))
			{
				return sumBelow;
			}

			sumBelow = 0;
			// Iterate summing everything in "B(wi-1)", which is all the words that DON'T have bigrams with wi-1
			foreach (var wordWithoutTrigram in GetBigramsWithoutTrigrams(wordminus2, wordminus1))
			{
				sumBelow += _result.Pml(wordminus1, wordWithoutTrigram);
			}

			_sumForP2FactorCache[key] = sumBelow;

			return sumBelow;
		}

		private Dictionary<Tuple<string, string>, List<string>> _bigramsWithoutBigrams = new Dictionary<Tuple<string, string>, List<string>>();

		private List<string> GetBigramsWithoutTrigrams(string wordminus2, string wordminus1)
		{
			List<string> wordsWithoutTrigrams;
			//var key = new Tuple<string, string>(wordminus2, wordminus1);
			//if (_bigramsWithoutBigrams.TryGetValue(key, out wordsWithoutTrigrams))
			//{
			//	return wordsWithoutTrigrams;
			//}

			wordsWithoutTrigrams = new List<string>();

			// Iterate summing everything in "B(wi-1)", which is all the words that DON'T have bigrams with wi-1
			foreach (string potentialWordWithoutBigram in _result.UniqueWords.Keys)
			{
				if (_result.GetCountForTrigram(wordminus2, wordminus1, potentialWordWithoutBigram) == 0)
				{
					wordsWithoutTrigrams.Add(potentialWordWithoutBigram);
				}
			}

			//_bigramsWithoutBigrams[key] = wordsWithoutTrigrams;

			return wordsWithoutTrigrams;
		}

		public double P3(string wordminus2, string wordminus1, string word)
		{
			double pmlBigramAbove = _result.Pml(word);

			var sumBelow = SumForP3Factor(wordminus1);

			return pmlBigramAbove / sumBelow;
		}

		private readonly Dictionary<string, double> _sumForP3FactorCache = new Dictionary<string, double>(); 

		private double SumForP3Factor(string wordminus1)
		{
			double sumBelow;
			var key = wordminus1;
			if (_sumForP3FactorCache.TryGetValue(key, out sumBelow))
			{
				return sumBelow;
			}

			sumBelow = 0;
			// Iterate summing everything in "B(wi-1)", which is all the words that DON'T have bigrams with wi-1
			foreach (var wordWithoutBigram in GetUnigramsWithoutBigrams(wordminus1))
			{
				sumBelow += _result.Pml(wordWithoutBigram);
			}

			_sumForP3FactorCache[key] = sumBelow;

			return sumBelow;
		}

		private Dictionary<string, List<string>> _unigramsWithoutBigrams = new Dictionary<string, List<string>>();

		private List<string> GetUnigramsWithoutBigrams(string wordminus1)
		{
			List<string> wordsWithoutBigrams;
			//var key = wordminus1;
			//if (_unigramsWithoutBigrams.TryGetValue(wordminus1, out wordsWithoutBigrams))
			//{
			//	return wordsWithoutBigrams;
			//}

			wordsWithoutBigrams = new List<string>();

			// Iterate summing everything in "B(wi-1)", which is all the words that DON'T have bigrams with wi-1
			foreach (var potentialWordWithoutBigram in _result.UniqueWords.Keys)
			{
				if (_result.GetCountForBigram(wordminus1, potentialWordWithoutBigram) == 0)
				{
					wordsWithoutBigrams.Add(potentialWordWithoutBigram);
				}
			}

			//_unigramsWithoutBigrams[key] = wordsWithoutBigrams;

			return wordsWithoutBigrams;
		}

		public string GetModelName()
		{
			return "Problem1Model";
		}
	}
}
