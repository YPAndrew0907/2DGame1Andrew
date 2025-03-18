using System;
using System.Collections.Generic;

public class BindableData<T>
{
    public BindableData(T defaultValue = default) => mValue = defaultValue;

    protected T mValue;

    public static Func<T, T, bool> Comparer { get; set; } = (a, b) => a.Equals(b);

    public BindableData<T> WithComparer(Func<T, T, bool> comparer)
    {
        Comparer = comparer;
        return this;
    }

    public T Value
    {
        get => GetValue();
        set
        {
            if (value == null && mValue == null) return;
            if (value != null && Comparer(value, mValue)) return;
            var oldValue = mValue;
            SetValue(value);
            Invoke(oldValue, value);
        }
    }

    protected virtual void SetValue(T newValue) => mValue = newValue;

    protected virtual T GetValue() => mValue;

    // 旧值，新值
    private HashSet<Action<T,T>>  _mOnValueChanged = new();

    public void Register(Action<T,T> onValueChanged)
    {
        if (!_mOnValueChanged.Contains(onValueChanged))
        {
            _mOnValueChanged.Add(onValueChanged);
        }
    }

    public void UnRegister(Action<T, T> onValueChanged)
    {
        if (_mOnValueChanged.Contains(onValueChanged))
        {
            _mOnValueChanged.Remove(onValueChanged);
        }
    }

    private void Invoke(T oldValue,T newValue)
    {
        foreach (var action in _mOnValueChanged)
        {
            action.Invoke(oldValue,newValue);
        }
    }

    public override string ToString() => Value.ToString();
}