﻿using System;
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
			result.UniqueWordCount.Should().Be(7);
			result.GetCountForUnigram("4").Should().Be(3);
		}

		[TestMethod]
		public void SimpleSentenceCorpus_AllStatsObtained()
		{
			// Act
			CorpusParsingResult result = CorpusParsing.ParseCorpus(SampleCorpus.TwoDogSentences);

			// Verify
			result.Sentences.Should().HaveCount(2);
			result.UniqueWordCount.Should().Be(6);

			// Unigrams
			result.GetCountForUnigram("dog").Should().Be(2);
			result.GetCountForUnigram(Constants.Stop).Should().Be(2);
			result.GetCountForUnigram("is").Should().Be(2);
			result.GetCountForUnigram("cool").Should().Be(1);

			// Bigrams
			result.GetCountForBigram("dog", "is").Should().Be(2);
			result.GetCountForBigram("is", "pretty").Should().Be(1);
			result.GetCountForBigram("is", "cool").Should().Be(1);
			result.GetCountForBigram("is", Constants.Stop).Should().Be(0);
			result.GetCountForBigram("pretty", Constants.Stop).Should().Be(1);
			result.GetCountForBigram("pretty", "dog").Should().Be(0);

			// Trigrams
			result.GetCountForTrigram("dog", "is", "cool").Should().Be(1);
			result.GetCountForTrigram("the", "dog", "is").Should().Be(2);
			result.GetCountForTrigram("is", "cool", Constants.Stop).Should().Be(1);
			result.GetCountForTrigram("pretty", "is", "dog").Should().Be(0);

			// Count
			result.TotalUnigrams.Should().Be(10);
			result.TotalBigrams.Should().Be(8);
			result.TotalTrigrams.Should().Be(6);

			ProblemP problemP = new ProblemP(result);

			// Verify the function for P is well defined for trigrams that exist
			foreach (var wordminus2 in result.UniqueWords)
			{
				foreach (var wordminus1 in result.UniqueWords)
				{
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
				}
			}
		} 
	}
}
