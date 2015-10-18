using System;

namespace UWNLPAssignment1
{
	public static class Perplexity
	{
		public static double CalculatePerplexity(ILanguageModel model, CorpusParsingResult corpus, StringParsingResult testCorpus)
		{
			double logSumOfCorpus = 0;
			foreach (var sentence in testCorpus.Sentences)
			{
				double logOfSentence = 0;
				string previousWord = Constants.Start;
				string previousPreviousWord = Constants.Start;

				foreach (var word in sentence.Words)
				{
					string calculatedWord = word;
					if (!corpus.UniqueWords.ContainsKey(word))
					{
						calculatedWord = Constants.Unknown;
					}

					double modelP = model.P(previousPreviousWord, previousWord, calculatedWord);
					double logModelP = Math.Log(modelP);
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
			}

			double sum = logSumOfCorpus / testCorpus.TotalWordCount;
			return Math.Pow(2, -1*sum);
		}
	}
}
