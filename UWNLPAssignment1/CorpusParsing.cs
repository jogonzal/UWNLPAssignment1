using System;
using System.Collections.Generic;

namespace UWNLPAssignment1
{
	public class CorpusParsingResult
	{
		public int GetCountForUnigram(string i)
		{
			int result;
			if (Unigrams.TryGetValue(i, out result))
			{
				return result;
			}
			return 0;
		}

		public int GetCountForBigram(string i, string j)
		{
			int result;
			if (Bigrams.TryGetValue(new Tuple<string, string>(i, j), out result))
			{
				return result;
			}
			return 0;
		}

		public int GetCountForTrigram(string i, string j, string k)
		{
			int result;
			if (Trigrams.TryGetValue(new Tuple<string, string, string>(i, j, k), out result))
			{
				return result;
			}
			return 0;
		}

		public List<Sentence> Sentences { get; set; }

		public Dictionary<Tuple<string, string, string>, int> Trigrams { get; set; }

		public Dictionary<Tuple<string, string>, int> Bigrams { get; set; }

		public Dictionary<string, int> Unigrams { get; set; }

		public HashSet<string> UniqueWords { get; set; }

		public int UniqueWordCount { get; set; }

		public int TotalWordCount { get; set; }

		public int TotalBigrams { get; set; }

		public int TotalUnigrams { get; set; }

		public int TotalTrigrams { get; set; }
	}

	public static class CorpusParsing
	{
		public static CorpusParsingResult ParseCorpus(string corpus)
		{
			// Make everything lowercase
			corpus = corpus.ToLowerInvariant();

			HashSet<string> uniqueWords = new HashSet<string>();
			String[] sentenceStrings = corpus.Split(new []{'.', '\n', '\t'}, StringSplitOptions.RemoveEmptyEntries);
			List<Sentence> sentences = new List<Sentence>(sentenceStrings.Length);
			int uniqueWordCount = 0, totalWordCount = 0;

			// Add STOP to unique words
			uniqueWords.Add(Constants.Stop);
			uniqueWordCount++;

			foreach (var sentenceString in sentenceStrings)
			{
				var words = sentenceString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

				if (words.Length > 0)
				{
					// Add the word to the index so we can number it later
					foreach (var word in words)
					{
						totalWordCount++;
						if (!uniqueWords.Contains(word))
						{
							uniqueWords.Add(word);
							uniqueWordCount++;
						}
					}

					// Don't forget about STOP!
					totalWordCount++;

					sentences.Add(new Sentence()
						{
							Words = words
						});
				}
			}

			// Keep track of the count of unigrams, bigrams, trigrams
			int totalUnigrams = 0, totalBigrams = 0, totalTrigrams = 0;

			// Create one bi-dimensional array for bigrams
			var unigrams = new Dictionary<string, int>();
	
			// Create one bi-dimensional array for bigrams
			var bigrams = new Dictionary<Tuple<string, string>, int>();

			// Create one tri-dimensional array for the trigram
			var trigrams = new Dictionary<Tuple<string, string, string>, int>();

			// Pass through all sentences and through all words, populating unigram
			foreach (var sentence in sentences)
			{
				string previousWord = null;
				string previousPreviousWord = null;

				for (int i = 0; i < sentence.Words.Length + 1; i++)
				{
					// Consider STOP
					string word;
					if (i == sentence.Words.Length)
					{
						word = Constants.Stop;
					}
					else
					{
						word = sentence.Words[i];	
					}

					// Unigram
					var unigramKey = word;
					unigrams[unigramKey] = unigrams.ContainsKey(unigramKey) ? unigrams[unigramKey] + 1 : 1;
					totalUnigrams++;

					if (previousWord != null)
					{
						// Bigram
						var bigramKey = new Tuple<string, string>(previousWord, word);
						bigrams[bigramKey] = bigrams.ContainsKey(bigramKey) ? bigrams[bigramKey] + 1 : 1;
						totalBigrams++;

						if (previousPreviousWord != null)
						{
							// Trigram
							var trigramKey = new Tuple<string, string, string>(previousPreviousWord, previousWord, word);
							trigrams[trigramKey] = trigrams.ContainsKey(trigramKey) ? trigrams[trigramKey] + 1 : 1;
							totalTrigrams++;
						}
					}

					// Move to next
					previousPreviousWord = previousWord;
					previousWord = word;
				}
			}

			return new CorpusParsingResult()
			{
				Sentences = sentences,
				Unigrams = unigrams,
				TotalWordCount = totalWordCount,
				UniqueWordCount = uniqueWordCount,
				Bigrams = bigrams,
				Trigrams = trigrams,
				UniqueWords = uniqueWords,
				TotalUnigrams = totalUnigrams,
				TotalBigrams = totalBigrams,
				TotalTrigrams = totalTrigrams
			};
		}
	}
}
