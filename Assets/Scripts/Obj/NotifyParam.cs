 using System;

 public interface INotifyParam
    {
        long ParmaUid { get; set; }
        bool Init { get; }
        void Release();
    }

     /// <summary>
     ///  可以一次设置多个值类型数据
     /// </summary>
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

            if (value is int i)
            {
                _intValue = i;
            }
            else if (value is long l)
            {
                _longValue = l;
            }
            else if (value is float f)
            {
                _floatValue = f;
            }
            else if (value is double d)
            {
                _doubleValue = d;
            }
            else if (value is string s)
            {
                _str = s;
            }
            else
            {
                // 类型不支持，重置_init
                _init = false;
                throw new ArgumentException($"不支持的参数类型：{typeof(T)}");
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

        public void SetValue(Object value)
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