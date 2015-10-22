using System;
using System.Collections.Generic;

namespace UWNLPAssignment1
{
	public static class Perplexity
	{
		public class TestStats
		{
			public int TotalUnksFound { get; set; }
			public int TotalSentencesFound { get; set; }
			public int TotalWordsFound { get; set; }
			public HashSet<string> UniqueUnksFound { get; set; }
			public HashSet<string> UniqueWordsFound { get; set; }

			public override string ToString()
			{
				return string.Format("TotalUnks:{0}, UniqueUnks:{1} TotalWords:{2} UniqueWords:{3} TotalSentences:{4}",
					TotalUnksFound, UniqueUnksFound.Count, TotalWordsFound, UniqueWordsFound.Count, TotalSentencesFound);
			}
		}

		public static double CalculatePerplexity(ILanguageModel model, CorpusParsingResult trainingCorpus, StringParsingResult testCorpus, out TestStats testStats)
		{
			testStats = new TestStats
			{
				UniqueUnksFound = new HashSet<string>(),
				UniqueWordsFound = new HashSet<string>()
			};

			double logSumOfCorpus = 0;
			for (int k = 0; k < testCorpus.Sentences.Count; k++)
			{
				Sentence sentence = testCorpus.Sentences[k];
				double logOfSentence = 0;
				string previousWord = Constants.Start;
				string previousPreviousWord = Constants.Start;

				testStats.TotalSentencesFound++;
				for (int i = 0; i < sentence.Words.Length; i++)
				{
					string calculatedWord = sentence.Words[i];
					if (!trainingCorpus.UniqueWords.ContainsKey(sentence.Words[i]))
					{
						calculatedWord = Constants.Unknown;
						testStats.TotalUnksFound++;
						testStats.UniqueUnksFound.Add(sentence.Words[i]);
					}
					testStats.TotalWordsFound++;
					testStats.UniqueWordsFound.Add(calculatedWord);

					double modelP = model.P(previousPreviousWord, previousWord, calculatedWord);
					double logModelP = Math.Log(modelP, 2);
					logOfSentence += logModelP;

					previousPreviousWord = previousWord;
					previousWord = calculatedWord;
				}

				if (Double.IsInfinity(logOfSentence))
				{
					throw new InvalidOperationException();
				}
				logSumOfCorpus += logOfSentence;
				if (Double.IsInfinity(logSumOfCorpus))
				{
					throw new InvalidOperationException();
				}

				if (model is Problem1Model && k % 100 == 0)
				{
					Console.WriteLine("Now at sentence {0}/{1}", k, testCorpus.Sentences.Count);
				}
		}

			double sum = logSumOfCorpus / testCorpus.TotalWordCount;
			return Math.Pow(2, -1*sum);
		}
	}
}
