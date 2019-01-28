using System;

namespace VulcanAnalytics.DBTester.Exceptions
{
    public class ObjectNotFound : Exception
    {
        public ObjectNotFound() { }
        public ObjectNotFound(string message) : base(message) { }
        public ObjectNotFound(string message, Exception inner) : base(message, inner) { }
        protected ObjectNotFound(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
