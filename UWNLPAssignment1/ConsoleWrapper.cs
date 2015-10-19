using System;
using System.IO;

namespace UWNLPAssignment1
{
	public static class ConsoleWrapper
	{
		private static FileStream fs = new FileStream(Path.Combine(Environment.CurrentDirectory + "myfile.txt"), FileMode.OpenOrCreate, FileAccess.ReadWrite);
		private static StreamWriter sw = new StreamWriter(fs); 
		// Write to console and to a file
		public static void Log(String format, params Object[] arg)
		{
			string realString = string.Format(format, arg);
			Console.WriteLine(realString);
			sw.WriteLine(realString);
		}

		public static void Close()
		{
			sw.Close();
			fs.Close();
		}
	}
}
