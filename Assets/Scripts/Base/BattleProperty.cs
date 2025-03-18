using System;
using UnityEngine;

namespace Entity
{
    public class BattleProperty
    {
        private event Action<int, int> OnValueChanged; // (old, new)

        private int   _baseValue;
        private int   _additive;
        private float _multiplier; // 百分比加成（0.1 = 10%）
        private int   _cachedValue;

        public int BaseValue    => _baseValue;
        public int CurrentValue => _cachedValue;

        public BattleProperty(int baseValue)
        {
            _baseValue = baseValue;
            Recalculate(); // 初始化时计算
        }

        // 修改基础值（如装备变化）
        public void SetBase(int newBase)
        {
            if (_baseValue == newBase) return;
            _baseValue = newBase;
            Recalculate();
        }

        // 增加固定值（如buff加成）
        public void AddFlat(int amount)
        {
            if (amount == 0) return;
            _additive += amount;
            Recalculate();
        }

        // 增加百分比（如技能加成）
        public void AddMultiplier(float percent)
        {
            if (Mathf.Approximately(percent, 0f)) return;
            _multiplier += percent;
            Recalculate();
        }

        // 核心计算方法
        private void Recalculate()
        {
            int old = _cachedValue;
            
            // 计算新值（向下取整）
            int newValue = Mathf.FloorToInt((_baseValue + _additive) * (1 + _multiplier));

            // 值未变化时不触发事件
            if (newValue == old) return;

            _cachedValue = newValue;
            OnValueChanged?.Invoke(old, newValue);
        }

        // 重置所有修正
        public void ResetModifiers()
        {
            _additive   = 0;
            _multiplier = 0f;
            Recalculate();
        }
        
        // 订阅属性变化事件
        public void Subscribe(Action<int, int> callback)
        {
            OnValueChanged += callback;
        }
        // 取消订阅属性变化事件
        public void Unsubscribe(Action<int, int> callback)
        {
            OnValueChanged -= callback;
        }

        public void ClearAction()
        {
            OnValueChanged = null;
        }
        
        public static implicit operator int(BattleProperty property)
        {
            return property.CurrentValue;
        }

        public static implicit operator float(BattleProperty property)
        {
            return property.CurrentValue;
        }
    }
}