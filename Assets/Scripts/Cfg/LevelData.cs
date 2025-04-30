using UnityEngine;
using UnityEngine.Serialization;

namespace Cfg
{
    public class LevelData : ScriptableObject
    {
        public int    Level            { get; private set; }
        public float  BossChip         { get; private set; }
        public string PlayerSkill      { get; private set; }
        public int    TableLevel       { get; private set; }
        public string[] CarryCard        { get; private set; }
        public int    MaxCard          { get; private set; }
        public string SpecialCondition { get; private set; }

        public LevelData() { }
    }

    public enum PlayerSkill
    {
        None,
        // 集中观察 或 记忆 
        WatchAndRemember,
        // 神速伸缩
        SwitchCard,
        // 自然谎言
        Lie,
        // 灵活之指
        HideCard,
        
    }
}