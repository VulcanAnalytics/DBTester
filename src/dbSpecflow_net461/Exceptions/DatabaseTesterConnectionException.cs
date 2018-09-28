using System;

namespace VulcanAnalytics.DBTester.Exceptions
{
    public class DatabaseTesterConnectionException : Exception
    {
        public DatabaseTesterConnectionException() { }
        public DatabaseTesterConnectionException(string message) : base(message) { }
        public DatabaseTesterConnectionException(string message, Exception inner) : base(message, inner) { }
        protected DatabaseTesterConnectionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
