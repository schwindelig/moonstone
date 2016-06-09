using System;

namespace moonstone.core.exceptions
{
    [Serializable]
    public class CreateUserException : Exception
    {
        public CreateUserException()
        {
        }

        public CreateUserException(string message) : base(message)
        {
        }

        public CreateUserException(string message, Exception inner) : base(message, inner)
        {
        }

        protected CreateUserException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class DeleteUserException : Exception
    {
        public DeleteUserException()
        {
        }

        public DeleteUserException(string message) : base(message)
        {
        }

        public DeleteUserException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DeleteUserException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class QueryUsersException : Exception
    {
        public QueryUsersException()
        {
        }

        public QueryUsersException(string message) : base(message)
        {
        }

        public QueryUsersException(string message, Exception inner) : base(message, inner)
        {
        }

        protected QueryUsersException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class UpdateUserException : Exception
    {
        public UpdateUserException()
        {
        }

        public UpdateUserException(string message) : base(message)
        {
        }

        public UpdateUserException(string message, Exception inner) : base(message, inner)
        {
        }

        protected UpdateUserException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}