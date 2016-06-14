﻿using System;

namespace moonstone.core.exceptions
{
    [Serializable]
    public class CreateGroupException : Exception
    {
        public CreateGroupException()
        {
        }

        public CreateGroupException(string message) : base(message)
        {
        }

        public CreateGroupException(string message, Exception inner) : base(message, inner)
        {
        }

        protected CreateGroupException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class CreateGroupUserException : Exception
    {
        public CreateGroupUserException()
        {
        }

        public CreateGroupUserException(string message) : base(message)
        {
        }

        public CreateGroupUserException(string message, Exception inner) : base(message, inner)
        {
        }

        protected CreateGroupUserException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

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
    public class QueryGroupsException : Exception
    {
        public QueryGroupsException()
        {
        }

        public QueryGroupsException(string message) : base(message)
        {
        }

        public QueryGroupsException(string message, Exception inner) : base(message, inner)
        {
        }

        protected QueryGroupsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class QueryGroupUsersException : Exception
    {
        public QueryGroupUsersException()
        {
        }

        public QueryGroupUsersException(string message) : base(message)
        {
        }

        public QueryGroupUsersException(string message, Exception inner) : base(message, inner)
        {
        }

        protected QueryGroupUsersException(
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