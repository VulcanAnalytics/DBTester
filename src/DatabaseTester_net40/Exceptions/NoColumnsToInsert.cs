using System;

namespace VulcanAnalytics.DBTester.Exceptions
{
    public class NoColumnsToInsert : Exception
    {
        public NoColumnsToInsert() { }
        public NoColumnsToInsert(string message) : base(message) { }
        public NoColumnsToInsert(string message, Exception inner) : base(message, inner) { }
        protected NoColumnsToInsert(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
