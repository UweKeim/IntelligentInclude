namespace IntelligentInclude
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class IntelligentIncludeException : Exception
    {
        public ExceptionReason Reason { get; private set; }

        public enum ExceptionReason
        {
            None,
            CannotCalculatePath,
            RecursionTooDeep,
            NoEndingPlaceholderFound,
            CalculatedFilePathDoesNotExist,
            EmptyPathSpecified,
            SpecifiedDirectoryDoesNotExist,
            FolderCannotBeDetermined,
            NonExistingFolderPathDetected,
            GenericErrorProcessingFile
        }

        //public IntelligentIncludeException()
        //{
        //    Reason = ExceptionReason.None;
        //}

        //public IntelligentIncludeException(string message) : base(message)
        //{
        //    Reason = ExceptionReason.None;
        //}

        //public IntelligentIncludeException(string message, Exception inner) : base(message, inner)
        //{
        //    Reason = ExceptionReason.None;
        //}

        public IntelligentIncludeException(ExceptionReason reason)
        {
            Reason = reason;
        }

        public IntelligentIncludeException(ExceptionReason reason,string message) : base(message)
        {
            Reason = reason;
        }

        public IntelligentIncludeException(ExceptionReason reason,string message, Exception inner) : base(message, inner)
        {
            Reason = reason;
        }

        protected IntelligentIncludeException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}