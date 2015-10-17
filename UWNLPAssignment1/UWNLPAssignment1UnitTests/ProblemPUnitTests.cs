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
			ProblemP problemP = new ProblemP(Result);

			problemP.DeterminePBucket("the", "dog", "is").Should().Be(ProblemP.PBucket.P1);
			problemP.DeterminePBucket("dog", "dog", "is").Should().Be(ProblemP.PBucket.P2);
			problemP.DeterminePBucket("the", "the", "the").Should().Be(ProblemP.PBucket.P3);
			problemP.DeterminePBucket("the", "dog", "dog").Should().Be(ProblemP.PBucket.P3);
		}
	}
}
