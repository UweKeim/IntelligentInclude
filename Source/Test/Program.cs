namespace Test
{
	using System.IO;
	using System.Reflection;
	using System.Text;
	using Properties;

	internal class Program
	{
		private static void Main()
		{
			var folder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

			//var exePath = Path.Combine(folder, @"..\bin\ii.exe");
			var filePathSource01 = Path.Combine(folder, @"source01.txt");
			var filePathIncluded01 = Path.Combine(folder, @"included01.txt");
			var filePathIncluded02 = Path.Combine(folder, @"included02.txt");

			File.WriteAllText(filePathSource01, Resources.source01, Encoding.UTF8);
			File.WriteAllText(filePathIncluded01, Resources.included01, Encoding.UTF8);
			File.WriteAllText(filePathIncluded02, Resources.included02, Encoding.UTF8);

			//Process.Start(exePath, string.Format(@"""{0}""", filePathSource01));
			var pi = IntelligentInclude.Program.CreatePathInformation(Path.Combine(folder, @"source*.txt"));
			IntelligentInclude.Program.Process(pi, true);
		}
	}
}