using System;

namespace VulcanAnalytics.DBTester.Exceptions
{
    public class FailedDatabaseConnection : Exception
    {
        public FailedDatabaseConnection() { }
        public FailedDatabaseConnection(string message) : base(message) { }
        public FailedDatabaseConnection(string message, Exception inner) : base(message, inner) { }
        protected FailedDatabaseConnection(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
