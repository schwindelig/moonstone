﻿using System;

namespace moonstone.core.exceptions
{
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
}