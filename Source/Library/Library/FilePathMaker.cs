namespace IntelligentInclude.Library
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    internal static class FilePathMaker
    {
        public static string Make(IntelligentIncludeParameter parameter, string filePath, string referenceFolderPath)
        {
            filePath = ExpandPlaceholder(parameter, filePath);

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

        public static string ExpandPlaceholder(IntelligentIncludeParameter parameter, string text)
        {
            const string phs = @"${";
            const string phe = @"}";

            if (string.IsNullOrEmpty(text) || !text.Contains(phs) || !text.Contains(phe))
            {
                return text;
            }
            else
            {
                // --
                // Finden.

                var placeholders = new List<string>();

                var startPos = 0;
                var endPos = -1;

                while (startPos < text.Length)
                {
                    startPos = text.IndexOf(phs, endPos + phe.Length, StringComparison.Ordinal);
                    if (startPos < 0) break;
                    endPos = text.IndexOf(phe, startPos + phs.Length, StringComparison.Ordinal);

                    if (endPos <= startPos) break;

                    var placeholder = text.Substring(startPos + phs.Length, endPos - startPos - phs.Length - phe.Length + 1);
                    placeholders.Add(placeholder);
                }

                // --
                // Ersetzen.

                foreach (var placeholder in placeholders)
                {
                    string replacement;
                    if (parameter == null || parameter.ResolvePlaceholder == null)
                    {
                        replacement = string.Empty;
                    }
                    else
                    {
                        replacement = parameter.ResolvePlaceholder(placeholder) ?? string.Empty;
                    }

                    text = text.Replace(phs + placeholder + phe, replacement);
                }

                // --

                return text;
            }
        }
    }
}