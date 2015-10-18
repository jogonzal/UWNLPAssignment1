namespace UWNLPAssignment1
{
	public static class Configs
	{
		/// <summary>
		/// Used to adjust Pml' to have an extra probability mass to distribute
		/// </summary>
		public const double Beta = 0.75;

		// These 3 constants are used for the linear model - they describe the probability of the trigram, bigram and unigram in that order
		/// <summary>
		/// Trigram weight
		/// </summary>
		public const double Lambda1 = 0.34;

		/// <summary>
		/// Bigram weight
		/// </summary>
		public const double Lambda2 = 0.33;

		/// <summary>
		/// Unigram weight
		/// </summary>
		public const double Lambda3 = 0.33;

		/// <summary>
		/// The "expected" percentage of unks in the test model
		/// </summary>
		public const double PercentageOfUnks = 0.1;
	}
}