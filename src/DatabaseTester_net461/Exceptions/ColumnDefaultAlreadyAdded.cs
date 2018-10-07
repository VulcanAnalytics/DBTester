using System;

namespace VulcanAnalytics.DBTester.Exceptions
{
    public class ColumnDefaultAlreadyAdded : Exception
    {
        public ColumnDefaultAlreadyAdded() { }
        public ColumnDefaultAlreadyAdded(string message) : base(message) { }
        public ColumnDefaultAlreadyAdded(string message, Exception inner) : base(message, inner) { }
        protected ColumnDefaultAlreadyAdded(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
