namespace Test
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using IntelligentInclude;
    using Properties;

    internal class Program
    {
        private static void Main()
        {
            var folder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            var filePathSource01 = Path.Combine(folder, @"source01.txt");
            var filePathIncluded01 = Path.Combine(folder, @"included01.txt");
            var filePathIncluded02 = Path.Combine(folder, @"included02.txt");

            File.WriteAllText(filePathSource01, Resources.source01, Encoding.UTF8);
            File.WriteAllText(filePathIncluded01, Resources.included01, Encoding.UTF8);
            File.WriteAllText(filePathIncluded02, Resources.included02, Encoding.UTF8);

            var param = new IntelligentIncludeParameter
            {
                Log = Console.WriteLine,
                ResolvePlaceholder =
                    delegate(string placeholder)
                    {
                        return @".\a\..\";
                        //return string.Empty;
                    }
            };

            IntelligentInclude.Process(Path.Combine(folder, @"source*.txt"), true, param);
        }
    }
}