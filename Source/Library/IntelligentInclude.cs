namespace IntelligentInclude
{
    using Library;

    public static class IntelligentInclude
    {
        public static void Process(string rawPath, bool isRecursive, IntelligentIncludeParameter parameter = null)
        {
            rawPath = FilePathMaker.ExpandPlaceholder(parameter, rawPath);
            var pathInformation = PathInformationController.CreatePathInformation(rawPath, parameter);
            if (pathInformation == null)
            {
                throw new IntelligentIncludeException(IntelligentIncludeException.ExceptionReason.CannotCalculatePath,
                    "No path information could be calculated from raw path.");
            }
            else
            {
                PathInformationController.Process(pathInformation, isRecursive, parameter);
            }
        }

        internal static string DoResolvePlaceholder(IntelligentIncludeParameter parameter, string placeholder)
        {
            return parameter != null && parameter.ResolvePlaceholder != null
                ? parameter.ResolvePlaceholder(placeholder)
                : null;
        }

        internal static void DoLog(IntelligentIncludeParameter parameter, string text)
        {
            if (parameter != null && parameter.Log != null)
            {
                parameter.Log(text);
            }
        }
    }
}