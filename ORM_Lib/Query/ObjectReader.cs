using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;

namespace ORM_Lib.Query
{
    public class ObjectReader<T> : IEnumerable<T>, IEnumerable where T : class, new()
    {
        Enumerator<T> _enumerator;

        ObjectReader(DbDataReader reader) => _enumerator = new Enumerator<T>(reader);

        public IEnumerator<T> GetEnumerator()
        {
            var e = _enumerator;
            // enumerator can only be handed out once! Because DataReaders can not reset and execute again!
            if (e == null)
            {
                throw new InvalidOperationException("Cannot enumerate more than once");
            }

            _enumerator = null;
            return e;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    internal class Enumerator<T> : IEnumerator<T>, IEnumerator, IDisposable where T : new()
    {
        private DbDataReader _reader;

        private FieldInfo[] _fields;

        // in welchem index vom reader müssen wir _fields[x] suchen
        private int[] _fieldLookup;

        internal Enumerator(DbDataReader reader)
        {
            _reader = reader;
            _fields = typeof(T).GetFields();
        }

        object IEnumerator.Current => Current;

        public T Current { get; private set; }

        bool IEnumerator.MoveNext()
        {
            if (!_reader.Read()) return false;
            if (_fieldLookup == null)
            {
                InitFieldLookup();
            }

            var instance = new T();
            for (int i = 0, n = _fields.Length; i < n; i++)
            {
                var index = _fieldLookup[i];
                if (index >= 0)
                {
                    var fieldInfo = _fields[i];
                    fieldInfo.SetValue(instance, _reader.IsDBNull(index) ? null : _reader.GetValue(index));
                }
            }

            Current = instance;
            return true;
        }

        private void InitFieldLookup()
        {
            var map = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
            for (int i = 0, n = _reader.FieldCount; i < n; i++)
            {
                map.Add(_reader.GetName(i), i);
            }

            _fieldLookup = new int[_fields.Length];

            for (int i = 0, n = _fields.Length; i < n; i++)
            {
                if (map.TryGetValue(_fields[i].Name, out var index))
                {
                    _fieldLookup[i] = index;
                }
                else
                {
                    _fieldLookup[i] = -1;
                }
            }
        }


        public void Dispose()
        {
            _reader.Dispose();
        }

        public void Reset()
        {
        }
    }
}