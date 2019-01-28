using System;

namespace VulcanAnalytics.DBTester.Exceptions
{
    public class ChildTablesWithDataReferenceThisTable : Exception
    {
        public ChildTablesWithDataReferenceThisTable() { }
        public ChildTablesWithDataReferenceThisTable(string message) : base(message) { }
        public ChildTablesWithDataReferenceThisTable(string message, Exception inner) : base(message, inner) { }
        protected ChildTablesWithDataReferenceThisTable(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
