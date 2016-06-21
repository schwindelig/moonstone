using System;

namespace moonstone.core.exceptions.serviceExceptions
{
    [Serializable]
    public class CreateCategoryException : Exception
    {
        public CreateCategoryException()
        {
        }

        public CreateCategoryException(string message) : base(message)
        {
        }

        public CreateCategoryException(string message, Exception inner) : base(message, inner)
        {
        }

        protected CreateCategoryException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class LoginException : Exception
    {
        public LoginException()
        {
        }

        public LoginException(string message) : base(message)
        {
        }

        public LoginException(string message, Exception inner) : base(message, inner)
        {
        }

        protected LoginException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class SetCultureException : Exception
    {
        public SetCultureException()
        {
        }

        public SetCultureException(string message) : base(message)
        {
        }

        public SetCultureException(string message, Exception inner) : base(message, inner)
        {
        }

        protected SetCultureException(
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

    [Serializable]
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException()
        {
        }

        public UserNotFoundException(string message) : base(message)
        {
        }

        public UserNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected UserNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class UserNotInGroupException : Exception
    {
        public UserNotInGroupException()
        {
        }

        public UserNotInGroupException(string message) : base(message)
        {
        }

        public UserNotInGroupException(string message, Exception inner) : base(message, inner)
        {
        }

        protected UserNotInGroupException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}