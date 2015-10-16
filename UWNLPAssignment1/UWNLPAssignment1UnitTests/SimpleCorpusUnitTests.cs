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
			result.UniqueWordCount.Should().Be(7);
			int indexFor4 = result.UniqueWordsIndex["4"];
			result.Unigrams[indexFor4].Should().Be(3);

			VerifyDictionaryArrayAreInSync(result);
		}

		private void VerifyDictionaryArrayAreInSync(CorpusParsingResult result)
		{
			foreach (var word in result.UniqueWordsIndex)
			{
				result.GetWordForIndex(word.Value).Should().Be(word.Key);
			}
		}

		[TestMethod]
		public void SimpleSentenceCorpus_AllStatsObtained()
		{
			// Act
			CorpusParsingResult result = CorpsParsing.ParseCorpus(Corpus.TwoDogSentences);

			// Verify
			result.Sentences.Should().HaveCount(2);
			result.UniqueWordCount.Should().Be(6);
			int indexForThe = result.UniqueWordsIndex["the"];
			int indexForDog = result.UniqueWordsIndex["dog"];
			int indexForStop = result.UniqueWordsIndex[Constants.Stop];
			int indexForPretty = result.UniqueWordsIndex["pretty"];
			int indexForIs = result.UniqueWordsIndex["is"];
			int indexForCool = result.UniqueWordsIndex["cool"];

			// Unigrams
			result.Unigrams[indexForDog].Should().Be(2);
			result.Unigrams[indexForStop].Should().Be(2);
			result.Unigrams[indexForIs].Should().Be(2);
			result.Unigrams[indexForCool].Should().Be(1);

			// Bigrams
			result.Bigrams[indexForDog, indexForIs].Should().Be(2);
			result.Bigrams[indexForIs, indexForPretty].Should().Be(1);
			result.Bigrams[indexForIs, indexForCool].Should().Be(1);
			result.Bigrams[indexForIs, indexForStop].Should().Be(0);
			result.Bigrams[indexForPretty, indexForStop].Should().Be(1);
			result.Bigrams[indexForPretty, indexForDog].Should().Be(0);

			// Trigrams
			result.Trigrams[indexForDog, indexForIs, indexForCool].Should().Be(1);
			result.Trigrams[indexForThe, indexForDog, indexForIs].Should().Be(2);
			result.Trigrams[indexForIs, indexForCool, indexForStop].Should().Be(1);
			result.Trigrams[indexForPretty, indexForIs, indexForDog].Should().Be(0);

			VerifyDictionaryArrayAreInSync(result);
		}
	}
}
