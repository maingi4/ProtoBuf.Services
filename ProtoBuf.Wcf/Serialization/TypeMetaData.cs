using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ProtoBuf.Wcf.Serialization
{
    [DataContract]
    public sealed class TypeMetaData
    {
        #region Fields

        [DataMember]
        private IDictionary<string, IDictionary<string, int>> _internalStore = new Dictionary<string, IDictionary<string, int>>();
        [DataMember]
        private IDictionary<string, int> _baseNumberStore = new Dictionary<string, int>();
        
        #endregion

        #region Public Methods

        public void StoreFieldNumber(string typeNameSpace, string typeName, string fieldName,
            int fieldNumber)
        {
            var fullName = GetTypeFullName(typeNameSpace, typeName);

            IDictionary<string, int> typeStore;

            if (!_internalStore.TryGetValue(fullName, out typeStore))
            {
                typeStore = new Dictionary<string, int>();

                _internalStore.Add(fullName, typeStore);
            }

            typeStore[fieldName] = fieldNumber;
        }

        public bool GetFieldNumber(string typeNameSpace, string typeName, string fieldName,
            out int? fieldNumber)
        {
            fieldNumber = null;

            var fullName = GetTypeFullName(typeNameSpace, typeName);

            IDictionary<string, int> typeStore;
            int fNumber;

            if (!_internalStore.TryGetValue(fullName, out typeStore)
                || typeStore == null || !typeStore.TryGetValue(fieldName, out fNumber))
            {
                return false;
            }

            fieldNumber = fNumber;

            return true;
        }

        public int GetMaxFieldNumber(string typeNameSpace, string typeName)
        {
            var fullName = GetTypeFullName(typeNameSpace, typeName);

            IDictionary<string, int> typeStore;

            if (!_internalStore.TryGetValue(fullName, out typeStore) || typeStore.Count == 0)
                return 0;

            return typeStore.Values.Max();
        }

        public void StoreBaseNumber(string typeNameSpace, string typeName, int baseNumber)
        {
            var fullName = GetTypeFullName(typeNameSpace, typeName);

            _baseNumberStore[fullName] = baseNumber;
        }

        public bool GetBaseNumber(string typeNameSpace, string typeName, out int? baseNumber)
        {
            baseNumber = null;

            var fullName = GetTypeFullName(typeNameSpace, typeName);

            int tempBaseNumber;

            if (!_baseNumberStore.TryGetValue(fullName, out tempBaseNumber))
                return false;

            baseNumber = tempBaseNumber;

            return true;
        }

        #endregion

        #region Private Methods

        private string GetTypeFullName(string typeNameSpace, string typeName)
        {
            return string.Concat(typeNameSpace, typeName);
        }

        #endregion
    }
}
