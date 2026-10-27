using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyKitchen_Core.Tools
{
    public class ValueRange<T> where T : struct, IComparable<T>
    {
        private T? _min;
        private T? _max;

        public T? Min
        {
            get => _min;
            set { _min = value; Normalize(); }
        }

        public T? Max
        {
            get => _max;
            set { _max = value; Normalize(); }
        }

        public ValueRange(T? min = null, T? max = null)
        {
            _min = min;
            _max = max;
            Normalize();
        }

        private void Normalize()
        {
            if(_min.HasValue && _max.HasValue && _min.Value.CompareTo(_max.Value) > 0)
            {
                T temp = _min.Value;
                _min = _max;
                _max = temp;
            }
        }
    }
}
