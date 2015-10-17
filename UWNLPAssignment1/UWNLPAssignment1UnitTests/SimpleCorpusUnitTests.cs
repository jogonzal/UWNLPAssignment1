using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UWNLPAssignment1;

namespace UWNLPAssignment1UnitTests
{
	[TestClass]
	public class NumberCorpusUnitTests
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

			// PML
			result.Pml("dog").Should().Be(1.0 * 2 / 10);
			result.Pml("dog", "is").Should().Be(1.0 * 2 / 8);
			result.Pml("dog", "is", "cool").Should().Be(1.0*1/6);
		}
	}
}
