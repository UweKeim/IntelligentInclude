namespace IntelligentInclude
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class IntelligentIncludeException : Exception
    {
        public IntelligentIncludeException()
        {
        }

        public IntelligentIncludeException(string message) : base(message)
        {
        }

        public IntelligentIncludeException(string message, Exception inner) : base(message, inner)
        {
        }

        protected IntelligentIncludeException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}