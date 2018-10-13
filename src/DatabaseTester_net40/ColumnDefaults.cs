using System.Collections;
using System.Collections.Generic;
using VulcanAnalytics.DBTester.Exceptions;

namespace VulcanAnalytics.DBTester
{
    public class ColumnDefaults : IEnumerable
    {
        private bool isEmpty;
        private Dictionary<string,object> defaults;

        public bool IsEmpty
        {
            get { return isEmpty; }
        }

        public ColumnDefaults()
        {
            isEmpty = true;
            defaults = new Dictionary<string, object>();
        }

        public void AddDefault(KeyValuePair<string,object> columnDefault)
        {
            isEmpty = false;
            if (defaults.ContainsKey(columnDefault.Key))
            {
                throw new ColumnDefaultAlreadyAdded(columnDefault.Key);
            }
            defaults.Add(columnDefault.Key,columnDefault);
        }

        public IEnumerator GetEnumerator()
        {
            return defaults.Values.GetEnumerator();
        }
    }
}
