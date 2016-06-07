using System;

namespace moonstone.core.exceptions
{
    [Serializable]
    public class ReadFileException : Exception
    {
        public ReadFileException()
        {
        }

        public ReadFileException(string message) : base(message)
        {
        }

        public ReadFileException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ReadFileException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class EmptyFileException : Exception
    {
        public EmptyFileException()
        {
        }

        public EmptyFileException(string message) : base(message)
        {
        }

        public EmptyFileException(string message, Exception inner) : base(message, inner)
        {
        }

        protected EmptyFileException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class ParseException : Exception
    {
        public ParseException()
        {
        }

        public ParseException(string message) : base(message)
        {
        }

        public ParseException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ParseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}