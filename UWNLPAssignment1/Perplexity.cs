using System;
using System.Linq;

namespace UWNLPAssignment1
{
	public static class Perplexity
	{
		public static double CalculatePerplexity(ILanguageModel model, CorpusParsingResult trainingCorpus, StringParsingResult testCorpus)
		{
			double logSumOfCorpus = 0;
			for (int k = 0; k < testCorpus.Sentences.Count; k++)
			{
				Sentence sentence = testCorpus.Sentences[k];
				double logOfSentence = 0;
				string previousWord = Constants.Start;
				string previousPreviousWord = Constants.Start;

				for (int i = 0; i < sentence.Words.Length; i++)
				{
					string calculatedWord = sentence.Words[i];
					if (!trainingCorpus.UniqueWords.ContainsKey(sentence.Words[i]))
					{
						calculatedWord = Constants.Unknown;
					}

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

				//if (k%100 == 0)
				//{
				//	Console.WriteLine("Now at sentence {0}", k);
				//}
		}

			double sum = logSumOfCorpus / testCorpus.TotalWordCount;
			return Math.Pow(2, -1*sum);
		}
	}
}
