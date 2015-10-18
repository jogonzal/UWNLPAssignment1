using System;
using System.Diagnostics;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UWNLPAssignment1;

namespace UWNLPAssignment1UnitTests
{
	[TestClass]
	public class SimpleCorpusUnitTests
	{
		[TestMethod]
		public void ParseNumberCorpus_AllStatsObtained()
		{
			// Act
			CorpusParsingResult result = CorpusParsing.ParseCorpus(SampleCorpus.SmallNumberSentences);

			// Verify
			result.Sentences.Should().HaveCount(5);
			result.UniqueWordCount.Should().Be(8);
			result.GetCountForUnigram("4").Should().Be(3);
			result.GetCountForUnigram(Constants.Start).Should().Be(5);
		}

		[TestMethod]
		public void SimpleSentenceCorpus_AllStatsObtained()
		{
			// Act
			CorpusParsingResult result = CorpusParsing.ParseCorpus(SampleCorpus.TwoDogSentences);

			// Verify
			result.Sentences.Should().HaveCount(2);
			result.UniqueWordCount.Should().Be(7);

			// Unigrams
			result.GetCountForUnigram("dog").Should().Be(2);
			result.GetCountForUnigram(Constants.Stop).Should().Be(2);
			result.GetCountForUnigram("is").Should().Be(2);
			result.GetCountForUnigram("cool").Should().Be(1);
			result.GetCountForUnigram(Constants.Start).Should().Be(2);

			// Bigrams
			result.GetCountForBigram("dog", "is").Should().Be(2);
			result.GetCountForBigram("is", "pretty").Should().Be(1);
			result.GetCountForBigram("is", "cool").Should().Be(1);
			result.GetCountForBigram("is", Constants.Stop).Should().Be(0);
			result.GetCountForBigram("pretty", Constants.Stop).Should().Be(1);
			result.GetCountForBigram("pretty", "dog").Should().Be(0);
			result.GetCountForBigram(Constants.Start, Constants.Start).Should().Be(2);
			result.GetCountForBigram(Constants.Start, "the").Should().Be(2);

			// Trigrams
			result.GetCountForTrigram("dog", "is", "cool").Should().Be(1);
			result.GetCountForTrigram("the", "dog", "is").Should().Be(2);
			result.GetCountForTrigram("is", "cool", Constants.Stop).Should().Be(1);
			result.GetCountForTrigram("pretty", "is", "dog").Should().Be(0);
			result.GetCountForTrigram(Constants.Start, Constants.Start, "dog").Should().Be(0);
			result.GetCountForTrigram(Constants.Start, Constants.Start, "the").Should().Be(2);

			// Count
			result.TotalUnigrams.Should().Be(10);
			result.TotalBigrams.Should().Be(10);
			result.TotalTrigrams.Should().Be(10);

			Problem1Model problemP = new Problem1Model(result);

			// Verify the function for P is well defined for trigrams that exist
			foreach (var wordminus2 in result.UniqueWords)
			{
				if (wordminus2 == Constants.Stop)
				{
					continue;
				}

				foreach (var wordminus1 in result.UniqueWords)
				{
					if (wordminus1 == Constants.Stop || (wordminus2 != Constants.Start && wordminus1 == Constants.Start))
					{
						continue;
					}

					double total = 0;
					foreach (var word in result.UniqueWords)
					{
						double pml = problemP.P(wordminus2, wordminus1, word);
						if (pml > 0)
						{
							total += pml;
						}
					}
					Debug.WriteLine("Next! Sum was {0}", total);
					total.Should().Be(1);
				}
			}
		} 
	}
}
