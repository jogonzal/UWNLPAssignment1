﻿using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UWNLPAssignment1;

namespace UWNLPAssignment1UnitTests
{
	[TestClass]
	public class SimpleCorpusUnitTests
	{
		readonly CorpusParsingResult _twoDogSentencesCorpus = CorpusParsing.ParseCorpus(SampleCorpus.TwoDogSentences, false);
		readonly CorpusParsingResult _numberSentences = CorpusParsing.ParseCorpus(SampleCorpus.SmallNumberSentences, false);
		readonly CorpusParsingResult _loremIpsum = CorpusParsing.ParseCorpus(SampleCorpus.LoremIpsum, true);

		[TestMethod]
		public void ParseNumberCorpus_AllStatsObtained()
		{
			// Verify
			_numberSentences.Sentences.Should().HaveCount(5);
			_numberSentences.UniqueWords.Count.Should().Be(8);
			_numberSentences.GetCountForUnigram("4").Should().Be(3);
			_numberSentences.GetCountForUnigram(Constants.Start).Should().Be(5);
		}

		[TestMethod]
		public void SimpleSentenceCorpus_AllStatsObtained()
		{
			// Verify
			_twoDogSentencesCorpus.Sentences.Should().HaveCount(2);
			_twoDogSentencesCorpus.UniqueWords.Count.Should().Be(7);

			// Unigrams
			_twoDogSentencesCorpus.GetCountForUnigram("dog").Should().Be(2);
			_twoDogSentencesCorpus.GetCountForUnigram(Constants.Stop).Should().Be(2);
			_twoDogSentencesCorpus.GetCountForUnigram("is").Should().Be(2);
			_twoDogSentencesCorpus.GetCountForUnigram("cool").Should().Be(1);
			_twoDogSentencesCorpus.GetCountForUnigram(Constants.Start).Should().Be(2);

			// Bigrams
			_twoDogSentencesCorpus.GetCountForBigram("dog", "is").Should().Be(2);
			_twoDogSentencesCorpus.GetCountForBigram("is", "pretty").Should().Be(1);
			_twoDogSentencesCorpus.GetCountForBigram("is", "cool").Should().Be(1);
			_twoDogSentencesCorpus.GetCountForBigram("is", Constants.Stop).Should().Be(0);
			_twoDogSentencesCorpus.GetCountForBigram("pretty", Constants.Stop).Should().Be(1);
			_twoDogSentencesCorpus.GetCountForBigram("pretty", "dog").Should().Be(0);
			_twoDogSentencesCorpus.GetCountForBigram(Constants.Start, Constants.Start).Should().Be(2);
			_twoDogSentencesCorpus.GetCountForBigram(Constants.Start, "the").Should().Be(2);

			// Trigrams
			_twoDogSentencesCorpus.GetCountForTrigram("dog", "is", "cool").Should().Be(1);
			_twoDogSentencesCorpus.GetCountForTrigram("the", "dog", "is").Should().Be(2);
			_twoDogSentencesCorpus.GetCountForTrigram("is", "cool", Constants.Stop).Should().Be(1);
			_twoDogSentencesCorpus.GetCountForTrigram("pretty", "is", "dog").Should().Be(0);
			_twoDogSentencesCorpus.GetCountForTrigram(Constants.Start, Constants.Start, "dog").Should().Be(0);
			_twoDogSentencesCorpus.GetCountForTrigram(Constants.Start, Constants.Start, "the").Should().Be(2);

			// Count
			_twoDogSentencesCorpus.TotalUnigrams.Should().Be(10);
			_twoDogSentencesCorpus.TotalBigrams.Should().Be(10);
			_twoDogSentencesCorpus.TotalTrigrams.Should().Be(10);
		}

		[TestMethod]
		public void Problem1Model_WellDefinedProbability()
		{
			ILanguageModel problem1Model = new Problem1Model(_twoDogSentencesCorpus);
			TestWellDefinedProbability(problem1Model, false);
		}

		[TestMethod]
		public void LinearModel_WellDefinedProbability()
		{
			ILanguageModel linearModel = new LinearModel(_twoDogSentencesCorpus);
			TestWellDefinedProbability(linearModel, false);
		}

		[TestMethod]
		public void TrigramModel_WellDefinedProbability()
		{
			ILanguageModel linearModel = new TrigramModel(_twoDogSentencesCorpus);
			TestWellDefinedProbability(linearModel, false);
		}

		[TestMethod]
		public void BigramModel_WellDefinedProbability()
		{
			ILanguageModel linearModel = new BigramModel(_twoDogSentencesCorpus);
			TestWellDefinedProbability(linearModel, false);
		}

		[TestMethod]
		public void UnigramModel_WellDefinedProbability()
		{
			ILanguageModel linearModel = new UnigramModel(_twoDogSentencesCorpus);
			TestWellDefinedProbability(linearModel, false);
		}

		[TestMethod]
		public void UnksAddedCorrectly()
		{
			_loremIpsum.UniqueWords.Keys.Where(k => k == Constants.Unknown).Should().HaveCount(c => c > 0);
			_loremIpsum.GetCountForUnigram(Constants.Unknown).Should().BeGreaterThan(0);
		}

		private void TestWellDefinedProbability(ILanguageModel model, bool testIfWellDefined)
		{
			// Verify the function for P is well defined for trigrams that exist
			foreach (var wordminus2 in _twoDogSentencesCorpus.UniqueWords.Keys)
			{
				if (wordminus2 == Constants.Stop)
				{
					continue;
				}

				foreach (var wordminus1 in _twoDogSentencesCorpus.UniqueWords.Keys)
				{
					if (wordminus1 == Constants.Stop || (wordminus2 != Constants.Start && wordminus1 == Constants.Start))
					{
						continue;
					}

					double total = 0;
					foreach (var word in _twoDogSentencesCorpus.UniqueWords.Keys.Where(w => w != Constants.Start))
					{
						double pml = model.P(wordminus2, wordminus1, word);
						if (pml > 0)
						{
							total += pml;
						}
					}
					Debug.WriteLine("Next! Sum was {0}", total);
					if (testIfWellDefined)
					{
						total.Should().Be(1);
					}
				}
			}
		}
	}
}
