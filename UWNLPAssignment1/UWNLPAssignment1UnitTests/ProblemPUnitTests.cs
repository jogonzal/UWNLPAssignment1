using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UWNLPAssignment1;

namespace UWNLPAssignment1UnitTests
{
	[TestClass]
	public class ProblemPUnitTests
	{
		// The dog is pretty. The dog is cool
		private static readonly CorpusParsingResult Result = CorpusParsing.ParseCorpus(SampleCorpus.TwoDogSentences);

		[TestMethod]
		public void ProblemP_DeterminePBucket()
		{
			Problem1Model problemP = new Problem1Model(Result);

			problemP.DeterminePBucket("the", "dog", "is").Should().Be(Problem1Model.PBucket.P1);
			problemP.DeterminePBucket("dog", "dog", "is").Should().Be(Problem1Model.PBucket.P2);
			problemP.DeterminePBucket("the", "the", "the").Should().Be(Problem1Model.PBucket.P3);
			problemP.DeterminePBucket("the", "dog", "dog").Should().Be(Problem1Model.PBucket.P3);
		}
	}
}
