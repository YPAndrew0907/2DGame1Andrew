using UnityEngine;

namespace Cfg
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "MENUNAME", order = 0)]
    public class LevelData : ScriptableObject
    {
        public int       levelNumber;
        public int       bossMoney;
        public PlaySkill playSkill;

    }

    public enum PlaySkill
    {
        None,
        // 集中观察 或 记忆 
        WatchAndRemember,
        // 神速伸缩
        SwitchCard,
        Lie,
        
        
    }
}