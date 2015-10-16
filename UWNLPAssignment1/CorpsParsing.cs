using System;
using System.Collections.Generic;

namespace UWNLPAssignment1
{
	public class CorpusParsingResult
	{
		public List<Sentence> Sentences { get; set; }

		public int[,,] Trigrams { get; set; }

		public int[,] Bigrams { get; set; }

		public int[] Unigrams { get; set; }

		public Dictionary<string, int> UniqueWordsIndex { get; set; }
	}

	public static class CorpsParsing
	{
		public static CorpusParsingResult ParseCorpus(string corpus)
		{
			Dictionary<string, int> uniqueWordsIndex = new Dictionary<string, int>();
			String[] sentenceStrings = corpus.Split(new []{'.', '\n', '\t'}, StringSplitOptions.RemoveEmptyEntries);
			List<Sentence> sentences = new List<Sentence>(sentenceStrings.Length);
			int countOfWords = 0;
			foreach (var sentenceString in sentenceStrings)
			{
				var words = sentenceString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

				if (words.Length > 0)
				{
					// Add the word to the index so we can number it later
					foreach (var word in words)
					{
						if (!uniqueWordsIndex.ContainsKey(word))
						{
							uniqueWordsIndex.Add(word, countOfWords++);
						}
					}

					sentences.Add(new Sentence()
						{
							Words = words
						});
				}
			}

			// Create one array for the uni index
			int[] unigrams = new int[countOfWords];

			// Pass through all sentences and through all words, populating unigram
			foreach (var sentence in sentences)
			{
				foreach (var word in sentence.Words)
				{
					int wordIndex = uniqueWordsIndex[word];
					unigrams[wordIndex]++;
				}
			}

			return new CorpusParsingResult()
			{
				Sentences = sentences,
				UniqueWordsIndex = uniqueWordsIndex,
				Unigrams = unigrams
			};
		}
	}
}
