using System;

namespace VulcanAnalytics.DBTester.Exceptions
{
    public class ChildTablesReferenceThisTable : Exception
    {
        public ChildTablesReferenceThisTable() { }
        public ChildTablesReferenceThisTable(string message) : base(message) { }
        public ChildTablesReferenceThisTable(string message, Exception inner) : base(message, inner) { }
        protected ChildTablesReferenceThisTable(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
