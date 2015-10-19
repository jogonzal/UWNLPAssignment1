namespace UWNLPAssignment1
{
	public static class SampleCorpus
	{
		private const string _smallNumberSentences = "1 2 3. 4 5 6. 1 2. 1 2 3 1 2 3 2 1 2 3 1. 4 5 4 3 2 1.";

		public static ReadCorpusResult SmallNumberSentences = new ReadCorpusResult()
		{
			CorpusName = RealCorpus.SampleCorpusNumberSentences,
			Development = _smallNumberSentences,
			Evaluation = _smallNumberSentences,
			Training = _smallNumberSentences
		};

		private const string _twoDogSentences = "The dog is pretty. The dog is cool.";

		public static ReadCorpusResult TwoDogSentences = new ReadCorpusResult()
		{
			CorpusName = RealCorpus.SampleTwoDogSentences,
			Development = _twoDogSentences,
			Evaluation = _twoDogSentences,
			Training = _twoDogSentences
		};

		private const string _loremIpsum = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";

		public static ReadCorpusResult LoremIpsum = new ReadCorpusResult()
		{
			CorpusName = RealCorpus.SampleLoremIpsum,
			Development = _loremIpsum,
			Evaluation = _loremIpsum,
			Training = _loremIpsum
		};

	}
}
