﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}