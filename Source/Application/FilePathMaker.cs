namespace IntelligentInclude
{
	using System.IO;

	internal sealed class FilePathMaker
	{
		public string Make(string filePath, string referenceFolderPath)
		{
			if (File.Exists(filePath))
			{
				return filePath;
			}
			else
			{
			    var both = Path.GetFullPath(Path.Combine(referenceFolderPath, filePath));
			    return File.Exists(both) ? both : string.Empty;
			}
		}
	}
}