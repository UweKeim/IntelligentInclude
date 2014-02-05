namespace IntelligentInclude.Library
{
    using System;
    using System.Globalization;
    using System.IO;

    internal static class PathInformationController
    {
        public static PathInformation CreatePathInformation(string rawPath, IntelligentIncludeParameter parameter)
        {
            if (string.IsNullOrEmpty(rawPath))
            {
                throw new IntelligentIncludeException(
                    IntelligentIncludeException.ExceptionReason.EmptyPathSpecified,
                    "[ERROR] Empty path parameter specified.");
            }
            else
            {
                rawPath = FilePathMaker.ExpandPlaceholder(parameter, rawPath);
                rawPath = rawPath.Replace('/', '\\');

                if (Directory.Exists(rawPath))
                {
                    IntelligentInclude.DoLog(parameter,
                        string.Format("Detected '{0}' as folder path, processing all files (*.*).", rawPath));
                    return new PathInformation { Filename = @"*.*", Folder = rawPath };
                }
                else if (rawPath.EndsWith(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture)) ||
                         rawPath.EndsWith(Path.AltDirectorySeparatorChar.ToString(CultureInfo.InvariantCulture)))
                {
                    throw new IntelligentIncludeException(
                        IntelligentIncludeException.ExceptionReason.SpecifiedDirectoryDoesNotExist,
                        string.Format("[ERROR] Specified directory '{0}' does not exist.", rawPath));
                }
                else
                {
                    var folder = Path.GetDirectoryName(rawPath);
                    var fn = Path.GetFileName(rawPath);

                    if (String.IsNullOrEmpty(folder)) folder = @".";

                    if (Directory.Exists(folder))
                    {
                        IntelligentInclude.DoLog(parameter,
                            string.Format("Detected '{0}' as folder path, processing file(s) '{1}'.", folder, fn));
                        return new PathInformation { Filename = fn, Folder = folder };
                    }
                    else if (String.IsNullOrEmpty(folder))
                    {
                        throw new IntelligentIncludeException(
                    IntelligentIncludeException.ExceptionReason.FolderCannotBeDetermined,
                            string.Format("[ERROR] Folder for '{0}' cannot be detected (is NULL).", rawPath));
                    }
                    else
                    {
                        throw new IntelligentIncludeException(
                    IntelligentIncludeException.ExceptionReason.NonExistingFolderPathDetected,
                            string.Format("[ERROR] Detected '{0}' as folder path, but does not exist.", folder));
                    }
                }
            }
        }

        public static void Process(PathInformation pathInformation, bool recurse, IntelligentIncludeParameter parameter)
        {
            var files = Directory.GetFiles(
                pathInformation.Folder,
                pathInformation.Filename,
                recurse
                    ? SearchOption.AllDirectories
                    : SearchOption.TopDirectoryOnly);

            IntelligentInclude.DoLog(parameter, string.Format("Processing {0} file(s)...", files.Length));

            var index = 0;
            foreach (var file in files)
            {
                IntelligentInclude.DoLog(parameter,
                    string.Format("Processing file {0} of {1}: '{2}'.", ++index, files.Length, file));
                doProcessFile(file, parameter);
            }

            IntelligentInclude.DoLog(parameter, string.Format("Finished processing {0} file(s).", files.Length));
        }

        private static void doProcessFile(string filePath, IntelligentIncludeParameter parameter)
        {
            try
            {
                new FileProcessor(filePath, parameter).Process(parameter);
            }
            catch (IntelligentIncludeException)
            {
                throw;
            }
            catch (Exception x)
            {
                throw new IntelligentIncludeException(
                    IntelligentIncludeException.ExceptionReason.GenericErrorProcessingFile,
                    string.Format("[ERROR] Error while processing file '{0}': {1}", filePath, x.Message), x);
            }
        }
    }
}