using System;
using System.Collections.Generic;

namespace UWNLPAssignment1
{
	public class CorpusParsingResult
	{
		public CorpusParsingResult(Dictionary<string, int> uniqueWordsIndex, string[] indexToWord)
		{
			UniqueWordsIndex = uniqueWordsIndex;
			IndexToWord = indexToWord;
		}

		public string GetWordForIndex(int i)
		{
			return IndexToWord[i];
		}

		public int GetIndexForWord(string i)
		{
			return UniqueWordsIndex[i];
		}

		public List<Sentence> Sentences { get; set; }

		public int[,,] Trigrams { get; set; }

		public int[,] Bigrams { get; set; }

		public int[] Unigrams { get; set; }

		public Dictionary<string, int> UniqueWordsIndex { get; set; }

		public int UniqueWordCount { get; set; }

		public int TotalWordCount { get; set; }

		private string[] IndexToWord { get; set; }
	}

	public static class CorpusParsing
	{
		public static CorpusParsingResult ParseCorpus(string corpus)
		{
			// Make everything lowercase
			corpus = corpus.ToLowerInvariant();

			Dictionary<string, int> uniqueWordsIndex = new Dictionary<string, int>();
			String[] sentenceStrings = corpus.Split(new []{'.', '\n', '\t'}, StringSplitOptions.RemoveEmptyEntries);
			List<Sentence> sentences = new List<Sentence>(sentenceStrings.Length);
			int uniqueWordCount = 0, totalWordCount = 0;

			// Add STOP to unique words
			uniqueWordsIndex.Add(Constants.Stop, uniqueWordCount++);

			foreach (var sentenceString in sentenceStrings)
			{
				var words = sentenceString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

				if (words.Length > 0)
				{
					// Add the word to the index so we can number it later
					foreach (var word in words)
					{
						totalWordCount++;
						if (!uniqueWordsIndex.ContainsKey(word))
						{
							uniqueWordsIndex.Add(word, uniqueWordCount++);
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

			// Create one array for the uni index
			int[] unigrams = new int[uniqueWordCount];

			// Create one bi-dimensional array for bigrams
			int[,] bigrams = new int[uniqueWordCount , uniqueWordCount];

			// Create one tri-dimensional array for the trigram
			int[,,] trigrams = new int[uniqueWordCount, uniqueWordCount, uniqueWordCount];

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
					int wordIndex = uniqueWordsIndex[word];
					unigrams[wordIndex]++;

					if (previousWord != null)
					{
						// Bigram
						int previousWordIndex = uniqueWordsIndex[previousWord];
						bigrams[previousWordIndex, wordIndex]++;

						if (previousPreviousWord != null)
						{
							// Trigram
							int previousPreviousWordIndex = uniqueWordsIndex[previousPreviousWord];
							trigrams[previousPreviousWordIndex, previousWordIndex, wordIndex]++;
						}
					}

					// Move to next
					previousPreviousWord = previousWord;
					previousWord = word;
				}
			}

			string[] indexToWord = new string[uniqueWordCount];
			foreach (var word in uniqueWordsIndex)
			{
				indexToWord[word.Value] = word.Key;
			}

			return new CorpusParsingResult(uniqueWordsIndex, indexToWord)
			{
				Sentences = sentences,
				Unigrams = unigrams,
				TotalWordCount = totalWordCount,
				UniqueWordCount = uniqueWordCount,
				Bigrams = bigrams,
				Trigrams = trigrams,
			};
		}
	}
}
