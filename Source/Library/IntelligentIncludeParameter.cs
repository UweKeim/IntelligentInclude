namespace IntelligentInclude
{
    public delegate void LogDelegate(string info);
    public delegate string ResolvePlaceholderDelegate(string placeholder);

    public sealed class IntelligentIncludeParameter
    {
        public LogDelegate Log { get; set; }
        public ResolvePlaceholderDelegate ResolvePlaceholder { get; set; }
    }
}