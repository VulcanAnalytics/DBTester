using System;

namespace VulcanAnalytics.DBTester.Exceptions
{
    public class NoRowsToInsert : Exception
    {
        public NoRowsToInsert() { }
        public NoRowsToInsert(string message) : base(message) { }
        public NoRowsToInsert(string message, Exception inner) : base(message, inner) { }
        protected NoRowsToInsert(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
