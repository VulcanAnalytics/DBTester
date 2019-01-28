using System.Collections;
using System.Collections.Generic;
using VulcanAnalytics.DBTester.Exceptions;

namespace VulcanAnalytics.DBTester
{
    public class ColumnDefaults : IEnumerable
    {
        private Dictionary<string,object> defaults;

        public bool IsEmpty
        {
            get
            {
                if (defaults.Count == 0)
                    return true;
                else
                    return false;
            }
        }

        public ColumnDefaults()
        {
            defaults = new Dictionary<string, object>();
        }

        public void AddDefault(KeyValuePair<string,object> columnDefault)
        {
            ThrowExceptionIfDefaultAlreadyAdded(columnDefault.Key);
            defaults.Add(columnDefault.Key,columnDefault);
        }

        public void Add(string columnName, object value)
        {
            ThrowExceptionIfDefaultAlreadyAdded(columnName);
            defaults.Add(columnName,new KeyValuePair<string, object>(columnName, value));
        }

        public void Add(KeyValuePair<string, object> columnDefault)
        {
            ThrowExceptionIfDefaultAlreadyAdded(columnDefault.Key);
            defaults.Add(columnDefault.Key, columnDefault);
        }

        public IEnumerator GetEnumerator()
        {
            return defaults.Values.GetEnumerator();
        }

        private void ThrowExceptionIfDefaultAlreadyAdded(string columnName)
        {
            if (defaults.ContainsKey(columnName))
            {
                throw new ColumnDefaultAlreadyAdded(columnName);
            }
        }
    }
}
