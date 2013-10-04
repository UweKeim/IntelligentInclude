namespace IntelligentInclude
{
	using System;
	using System.IO;
	using System.Text;

	internal sealed class FileProcessor
	{
		private readonly string _filePath;
		private readonly string _fileContent;

		public FileProcessor(string filePath)
		{
			_filePath = filePath;
			_fileContent = File.ReadAllText(filePath, Encoding.UTF8);
		}

		public string Process(bool writeToFile = true, int recursionDepth = 0)
		{
			Console.WriteLine(makeIndent(recursionDepth) + "Processing file '{0}'.", _filePath);

			if (recursionDepth > 30)
			{
				Console.Error.WriteLine(makeIndent(recursionDepth) + "[ERROR] Too much recursion.");
				return _fileContent;
			}
			else
			{
				if (string.IsNullOrEmpty(_fileContent) || _fileContent.Trim().Length <= 0)
				{
					if (recursionDepth <= 0)
					{
						Console.WriteLine(makeIndent(recursionDepth) + "File contains no content. Skipping.");
					}
					return _fileContent;
				}
				else
				{
					var startIndex = _fileContent.IndexOf(@"#zetainclude", StringComparison.Ordinal);
					var stopIndex = _fileContent.IndexOf(@"#endzetainclude", StringComparison.Ordinal);

					if (startIndex < 0)
					{
						if (recursionDepth <= 0)
						{
							Console.WriteLine(makeIndent(recursionDepth) + "File contains no include START. Skipping.");
						}
						return _fileContent;
					}
					else if (stopIndex < 0)
					{
						Console.WriteLine(makeIndent(recursionDepth) + "[WARN] File contains no include END. Skipping.");
						return _fileContent;
					}
					else
					{
						var result = doProcess(_fileContent, Path.GetDirectoryName(_filePath), recursionDepth);

						if (result != null && result != _fileContent)
						{
							if (writeToFile)
							{
								File.WriteAllText(_filePath, result, Encoding.UTF8);
								Console.WriteLine(makeIndent(recursionDepth) + "Successfully modified file. Finished.");
							}

							return result;
						}
						else
						{
							if (writeToFile)
							{
								Console.WriteLine(makeIndent(recursionDepth) + "File was not modified. Finished.");
							}
							return _fileContent;
						}
					}
				}
			}
		}

		private static string makeIndent(int recursionDepth)
		{
			return new string('\t', recursionDepth + 1);
		}

		private static string doProcess(string content, string folderPath, int recursionDepth)
		{
			return new ContentProcessor(content, folderPath).Process(recursionDepth);
		}
	}
}