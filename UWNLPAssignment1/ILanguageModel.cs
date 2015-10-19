namespace UWNLPAssignment1
{
	public interface ILanguageModel
	{
		double P(string wordminus2, string wordminus1, string word);

		string GetModelName();
	}
}
