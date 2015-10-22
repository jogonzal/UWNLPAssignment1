using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UWNLPAssignment1
{
	public class CorpusParsingResult
	{
		public int GetCountForUnigram(string i)
		{
			return Unigrams[i];
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

		public Dictionary<string, int> UniqueWords { get; set; }

		public int TotalWordCount { get; set; }

		public int TotalBigrams { get; set; }

		public int TotalUnigrams { get; set; }

		public int TotalTrigrams { get; set; }

		public RealCorpus CorpusName { get; set; }
	}

	public class StringParsingResult
	{
		public List<Sentence> Sentences { get; set; }

		public int TotalWordCount { get; set; }

		public Dictionary<string, int> UniqueWords { get; set; }
	}

	public static class CorpusParsing
	{
		public static CorpusParsingResult ParseCorpus(ReadCorpusResult readCorpus, bool unkEnabled, string postTrainWith = null)
		{
			// If we have a post training corpus, then use it several times (count it more than original training)
			StringParsingResult parsingResult;
			if (postTrainWith != null)
			{
				StringBuilder sb = new StringBuilder(readCorpus.Training);
				for (int i = 0; i < Configs.X; i++)
				{
					sb.Append(postTrainWith);
				}
				parsingResult = ParseString(sb.ToString());
			}
			else
			{
				parsingResult = ParseString(readCorpus.Training);
			}

			if (unkEnabled)
			{
				AddUnksToParsingResult(parsingResult);
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
			foreach (var sentence in parsingResult.Sentences)
			{
				// Consider start
				string previousWord = Constants.Start;
				string previousPreviousWord = Constants.Start;

				// Add start as a unigram and bigram before starting
				unigrams[Constants.Start] = unigrams.ContainsKey(Constants.Start) ? unigrams[Constants.Start] + 1 : 1;
				var startKey = new Tuple<string, string>(Constants.Start, Constants.Start);
				bigrams[startKey] = bigrams.ContainsKey(startKey) ? bigrams[startKey] + 1 : 1;

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
						// Get the word, or UNK if that's the case
						if (parsingResult.UniqueWords.ContainsKey(sentence.Words[i]))
						{
							word = sentence.Words[i];
						}
						else
						{
							word = Constants.Unknown;
						}
					}

					// Unigram
					var unigramKey = word;
					unigrams[unigramKey] = unigrams.ContainsKey(unigramKey) ? unigrams[unigramKey] + 1 : 1;
					totalUnigrams++;

					// Bigram
					var bigramKey = new Tuple<string, string>(previousWord, word);
					bigrams[bigramKey] = bigrams.ContainsKey(bigramKey) ? bigrams[bigramKey] + 1 : 1;
					totalBigrams++;

					// Trigram
					var trigramKey = new Tuple<string, string, string>(previousPreviousWord, previousWord, word);
					trigrams[trigramKey] = trigrams.ContainsKey(trigramKey) ? trigrams[trigramKey] + 1 : 1;
					totalTrigrams++;

					// Move to next
					previousPreviousWord = previousWord;
					previousWord = word;
				}
			}

			return new CorpusParsingResult()
			{
				Sentences = parsingResult.Sentences,
				Unigrams = unigrams,
				TotalWordCount = parsingResult.TotalWordCount,
				Bigrams = bigrams,
				Trigrams = trigrams,
				UniqueWords = parsingResult.UniqueWords,
				TotalUnigrams = totalUnigrams,
				TotalBigrams = totalBigrams,
				TotalTrigrams = totalTrigrams,
				CorpusName = readCorpus.CorpusName
			};
		}

		private static void AddUnksToParsingResult(StringParsingResult parsingResult)
		{
			List<KeyValuePair<string, int>> listOfWordsWithOneOccurrence = parsingResult.UniqueWords.Where(w => w.Value == 1).ToList();

			int numberOfUnksToAdd = Convert.ToInt32(listOfWordsWithOneOccurrence.Count * Configs.PercentageOfUnks);

			// The way we'll make a word an UNK is we'll remove it from the known words list. BAM!
			// We'll only remove words that appear once
			while (numberOfUnksToAdd > 0)
			{
				parsingResult.UniqueWords.Remove(listOfWordsWithOneOccurrence[numberOfUnksToAdd].Key);
				numberOfUnksToAdd--;
			}

			// Removed enough UNKs
			// Add Unk to known words
			parsingResult.UniqueWords.Add(Constants.Unknown, numberOfUnksToAdd);
		}

		public static StringParsingResult ParseString(string corpus)
		{
			// Make everything lowercase + give spaces to punctuation symbols to allow for them to form unique words
			StringBuilder sb = new StringBuilder(corpus);
			sb.Replace(",", " , ");
			sb.Replace(";", " ; ");
			sb.Replace(":", " : ");
			sb.Replace("-", " - ");
			corpus = sb.ToString().ToLowerInvariant();

			var uniqueWords = new Dictionary<string, int>();
			String[] sentenceStrings = corpus.Split(new[] { '.', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
			List<Sentence> sentences = new List<Sentence>(sentenceStrings.Length);
			int totalWordCount = 0;

			// Add START and STOP to unique words
			uniqueWords.Add(Constants.Start, 1);
			uniqueWords.Add(Constants.Stop, 1);

			foreach (var sentenceString in sentenceStrings)
			{
				var words = sentenceString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

				if (words.Length > 0)
				{
					// Add the word to the index so we can number it later
					foreach (var word in words)
					{
						totalWordCount++;
						if (!uniqueWords.ContainsKey(word))
						{
							uniqueWords.Add(word, 1);
						}
						else
						{
							uniqueWords[word]++;
						}
					}

					// Don't forget about STOP AND START!
					totalWordCount += 2;

					sentences.Add(new Sentence()
					{
						Words = words
					});
				}
			}

			return new StringParsingResult()
			{
				TotalWordCount = totalWordCount,
				Sentences = sentences,
				UniqueWords = uniqueWords
			};
		}
	}
}
