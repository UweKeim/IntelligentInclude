namespace IntelligentInclude.Library
{
    using System;
    using System.IO;
    using System.Text;

    internal sealed class FileProcessor
    {
        private readonly string _filePath;
        private readonly string _fileContent;
        private readonly LogDelegate _log;

        public FileProcessor(string filePath, LogDelegate log)
        {
            _filePath = filePath;
            _fileContent = File.ReadAllText(filePath, Encoding.UTF8);
            _log = log;
        }

        public string Process(LogDelegate log, bool writeToFile = true, int recursionDepth = 0)
        {
            IntelligentInclude.DoLog(_log, string.Format(makeIndent(recursionDepth) + "Processing file '{0}'.", _filePath));

            if (recursionDepth > 30)
            {
                throw new Exception(string.Format(makeIndent(recursionDepth) + "[ERROR] Too much recursion."));
            }
            else
            {
                if (string.IsNullOrEmpty(_fileContent) || _fileContent.Trim().Length <= 0)
                {
                    if (recursionDepth <= 0)
                    {
                        IntelligentInclude.DoLog(_log, string.Format(makeIndent(recursionDepth) + "File contains no content. Skipping."));
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
                            IntelligentInclude.DoLog(_log, string.Format(makeIndent(recursionDepth) + "File contains no include START. Skipping."));
                        }
                        return _fileContent;
                    }
                    else if (stopIndex < 0)
                    {
                        IntelligentInclude.DoLog(_log, string.Format(makeIndent(recursionDepth) + "[WARN] File contains no include END. Skipping."));
                        return _fileContent;
                    }
                    else
                    {
                        var result = doProcess(_fileContent, Path.GetDirectoryName(_filePath), recursionDepth, log);

                        if (result != null && result != _fileContent)
                        {
                            if (writeToFile)
                            {
                                File.WriteAllText(_filePath, result, Encoding.UTF8);
                                IntelligentInclude.DoLog(_log, string.Format(makeIndent(recursionDepth) + "Successfully modified file. Finished."));
                            }

                            return result;
                        }
                        else
                        {
                            if (writeToFile)
                            {
                                IntelligentInclude.DoLog(_log, string.Format(makeIndent(recursionDepth) + "File was not modified. Finished."));
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

        private static string doProcess(string content, string folderPath, int recursionDepth, LogDelegate log)
        {
            return new ContentProcessor(content, folderPath).Process(log, recursionDepth);
        }
    }
}