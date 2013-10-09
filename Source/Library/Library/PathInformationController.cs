namespace IntelligentInclude.Library
{
    using System;
    using System.Globalization;
    using System.IO;

    internal static class PathInformationController
    {
        public static PathInformation CreatePathInformation(string rawPath, LogDelegate log)
        {
            if (String.IsNullOrEmpty(rawPath))
            {
                throw new IntelligentIncludeException("[ERROR] Empty path parameter specified.");
            }
            else
            {
                rawPath = rawPath.Replace('/', '\\');

                if (Directory.Exists(rawPath))
                {
                    IntelligentInclude.DoLog(log,string.Format("Detected '{0}' as folder path, processing all files (*.*).", rawPath));
                    return new PathInformation { Filename = @"*.*", Folder = rawPath };
                }
                else if (rawPath.EndsWith(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture)) ||
                         rawPath.EndsWith(Path.AltDirectorySeparatorChar.ToString(CultureInfo.InvariantCulture)))
                {
                    throw new IntelligentIncludeException(string.Format("[ERROR] Specified directory '{0}' does not exist.", rawPath));
                }
                else
                {
                    var folder = Path.GetDirectoryName(rawPath);
                    var fn = Path.GetFileName(rawPath);

                    if (String.IsNullOrEmpty(folder)) folder = @".";

                    if (Directory.Exists(folder))
                    {
                        IntelligentInclude.DoLog(log,string.Format("Detected '{0}' as folder path, processing file(s) '{1}'.", folder, fn));
                        return new PathInformation { Filename = fn, Folder = folder };
                    }
                    else if (String.IsNullOrEmpty(folder))
                    {
                        throw new IntelligentIncludeException(string.Format("[ERROR] Folder for '{0}' cannot be detected (is NULL).", rawPath));
                    }
                    else
                    {
                        throw new IntelligentIncludeException(string.Format("[ERROR] Detected '{0}' as folder path, but does not exist.", folder));
                    }
                }
            }
        }

        public static void Process(PathInformation pathInformation, bool recurse, LogDelegate log)
        {
            var files = Directory.GetFiles(
                pathInformation.Folder,
                pathInformation.Filename,
                recurse
                    ? SearchOption.AllDirectories
                    : SearchOption.TopDirectoryOnly);

            IntelligentInclude.DoLog(log,string.Format("Processing {0} file(s)...", files.Length));

            var index = 0;
            foreach (var file in files)
            {
                IntelligentInclude.DoLog(log,string.Format("Processing file {0} of {1}: '{2}'.", ++index, files.Length, file));
                doProcessFile(file, log);
            }

            IntelligentInclude.DoLog(log,string.Format("Finished processing {0} file(s).", files.Length));
        }

        private static void doProcessFile(string filePath, LogDelegate log)
        {
            try
            {
                new FileProcessor(filePath, log).Process(log);
            }
            catch (Exception x)
            {
                throw new IntelligentIncludeException(string.Format("[ERROR] Error while processing file '{0}': {1}", filePath, x.Message), x);
            }
        }
    }
}