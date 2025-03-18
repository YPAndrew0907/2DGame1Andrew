 using System;

 public interface INotifyParam
    {
        long ParmaUid { get; set; }
        bool Init { get; }
        void Release();
    }

    public class NormalParam : INotifyParam
    {
        public long ParmaUid { get; set; }
        private bool _init;
        public bool Init => _init;

        private int _intValue;
        private long _longValue;
        private string _str;
        private float _floatValue;
        private double _doubleValue;

        public int IntValue => _init ? _intValue : int.MinValue;
        public long LongValue => _init ? _longValue : long.MinValue;
        public string StrValue => _init ? _str : string.Empty;
        public float FloatValue => _init ? _floatValue : float.NaN;
        public double DoubleValue => _init ? _doubleValue : double.NaN;

        public void Release()
        {
            _init = false;
            _intValue = default;
            _longValue = default;
            _floatValue = default;
            _doubleValue = default;
            _str = string.Empty;
        }

        public void SetValue<T>(T value)
        {
            _init = true;
            switch (value)
            {
                case int i:
                    _intValue = i;
                    break;
                case long l:
                    _longValue = l;
                    break;
                case float f:
                    _floatValue = f;
                    break;
                case double d:
                    _doubleValue = d;
                    break;
                case string s:
                    _str = s;
                    break;
            }
        }
    }

    public class CustomParam : INotifyParam
    {
        public long ParmaUid { get; set; }
        private object _value;
        private Array _values;
        private bool _init;

        public bool Init => _init;

        public void Release()
        {
            ParmaUid = 0;
            _init = false;
            _value = null;
            _values = null;
        }

        public void SetValue(object value)
        {
            _value = value;
            _init = true;
        }

        public void SetValue(Array values)
        {
            _values = values;
            _init = true;
        }

        public object Value => _init ? _value : null;
        public Array Values => _init ? _values : null;
    }