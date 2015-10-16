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
			CorpusParsingResult result = CorpsParsing.ParseCorpus(Corpus.SmallNumberSentences);

			// Verify
			result.Sentences.Should().HaveCount(5);
			result.UniqueWordsIndex.Should().HaveCount(6);
			int indexFor4 = result.UniqueWordsIndex["4"];
			result.Unigrams[indexFor4].Should().Be(3);
		}
	}
}
