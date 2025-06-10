using System;
using Obj;

namespace Mgr
{
    [Serializable]
    public class SkillConfig
    {
        public string      name;
        public PlayerSkill skillType;
        public float       baseHitRate;
        public int       baseParameter;
        public float       hitRateIncreasePerLevel;
        public int       parameterIncreasePerLevel;
        public string      desc;
    }
}