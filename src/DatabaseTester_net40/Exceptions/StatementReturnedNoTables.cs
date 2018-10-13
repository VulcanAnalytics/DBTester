using System;

namespace VulcanAnalytics.DBTester.Exceptions
{
    public class StatementReturnedNoTables : Exception
    {
        public StatementReturnedNoTables() { }
        public StatementReturnedNoTables(string message) : base(message) { }
        public StatementReturnedNoTables(string message, Exception inner) : base(message, inner) { }
        protected StatementReturnedNoTables(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
