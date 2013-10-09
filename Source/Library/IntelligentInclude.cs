namespace IntelligentInclude
{
    using Library;

    public delegate void LogDelegate(string info);

    public static class IntelligentInclude
    {
        public static void Process(string rawPath, bool isRecursive, LogDelegate log = null)
        {
            var pathInformation = PathInformationController.CreatePathInformation(rawPath, log);
            if (pathInformation == null)
            {
                throw new IntelligentIncludeException("No path information could be calculated from raw path.");
            }
            else
            {
                PathInformationController.Process(pathInformation, isRecursive, log);
            }
        }

        internal static void DoLog(LogDelegate log, string text)
        {
            if (log != null)
            {
                log(text);
            }
        }
    }
}