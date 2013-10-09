namespace IntelligentInclude.Library
{
    using System;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;

    internal sealed class ContentProcessor
	{
		private readonly string _content;
		private readonly string _folderPath;

		private static readonly Regex RXStart = new Regex(@"^.*?#zetainclude\s+""([^""]*)"".*?$",
														   RegexOptions.IgnoreCase | RegexOptions.Multiline);
		private static readonly Regex RXEnd = new Regex(@"^.*?#endzetainclude.*?$",
														   RegexOptions.IgnoreCase | RegexOptions.Multiline);

		public ContentProcessor(
			string content,
			string folderPath)
		{
			_content = content;
			_folderPath = folderPath;
		}

		public string Process(LogDelegate log, int recursionDepth = 0)
		{
			if (recursionDepth > 30)
			{
				throw new IntelligentIncludeException(makeIndent(recursionDepth) + "[ERROR] Too much recursion.");
			}
			else
			{
				var sb = new StringBuilder();

				var sourceIndex = 0;

				var matches = RXStart.Matches(_content);
				foreach (Match startMatch in matches)
				{
					// Find end tag.
					var endMatch = RXEnd.Match(_content, startMatch.Index + startMatch.Length);

				    if (endMatch.Success)
				    {
				        // Copy before.
				        var before = _content.Substring(sourceIndex, startMatch.Index + startMatch.Length - sourceIndex);
				        sb.Append(trimEndPlusNewLine(before));

				        // Insert inside.
				        var included = makeIncluded(startMatch.Groups[1].Value, recursionDepth, log);
				        sb.Append(trimEndPlusNewLine(included));

				        // Copy after, letting the next turn do it automatically.
				        sourceIndex = endMatch.Index;
				    }
				    else
				    {
				        throw new IntelligentIncludeException(
				            string.Format(
				                makeIndent(recursionDepth) +
				                "[ERROR] No ending placeholder for '{0}' found.",
				                startMatch.Value));
				    }
				}

				// Append remaining.
				sb.Append(_content.Substring(sourceIndex));

				return sb.ToString();
			}
		}

		private static string makeIndent(int recursionDepth)
		{
			return new string('\t', recursionDepth + 1);
		}

		private static string trimEndPlusNewLine(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return Environment.NewLine;
			}
			else
			{
				return text.TrimEnd('\r', '\n') + Environment.NewLine;
			}
		}

		private string makeIncluded(string filePath, int recursionDepth, LogDelegate log)
		{
			var fullFilePath = new FilePathMaker().Make(filePath, _folderPath);
			if (!string.IsNullOrEmpty(fullFilePath) && File.Exists(fullFilePath))
			{
				//var content = File.ReadAllText(fullFilePath, Encoding.UTF8);
				var content = new FileProcessor(fullFilePath, log).Process(log, false, recursionDepth + 1);
				return content;
			}
			else
			{
			    throw new IntelligentIncludeException(
			        string.Format(
			            makeIndent(recursionDepth) +
			            "[ERROR] Calculated file path '{0}' (from '{1}' and '{2}') does not exist.",
			            fullFilePath,
			            filePath,
			            _folderPath));
			}
		}
	}
}