using System.Collections.Generic;
using UnityEngine;

namespace Obj
{
    public class LevelData
    {
        public int      Level            { get; set; }
        public float    BossChip         { get; set; }
        public string   BossSkill        { get; set; }
        public int      TableLevel       { get; set; }
        public string[] CarryCard        { get; set; }
        public int      MaxCard          { get; set; }
        public string   SpecialCondition { get; set; }
        public string   LevelAIName      => "Lv." + Level + "AI";

        public LevelData()
        {
        }

        private static readonly Dictionary<PlayerSkill, string> SpecialConditionDesc = new()
        {
            { Obj.PlayerSkill.GuessOrRemember, "集中观察/记忆" },
            { Obj.PlayerSkill.Guess, "集中观察" },
            { Obj.PlayerSkill.Remember, "记忆"},
            
           
            { Obj.PlayerSkill.CopyAndSwitch, "神速伸缩" },
            { Obj.PlayerSkill.Copy, "神速伸缩" },
            { Obj.PlayerSkill.Switch, "神速伸缩" },
            
            { Obj.PlayerSkill.StealAndInsert, "灵活之指" },
            
            { Obj.PlayerSkill.Lie, "自然谎言" },
            
            { Obj.PlayerSkill.Detect, "识破" }
        };

        public static string GetSkillDesc(PlayerSkill skill)
        {
            return SpecialConditionDesc.GetValueOrDefault(skill);
        }
        
        public static List<string> GetSkillsDesc(IEnumerable<PlayerSkill> skills)
        {
            var uniqueDesc = new HashSet<string>();
            if (skills == null)
                return null;
            foreach (var skill in skills)
            {
                var termS = GetSkillDesc(skill);
                if (!string.IsNullOrEmpty(termS))
                {
                    uniqueDesc.Add(termS);
                } 
            }
            return new List<string>(uniqueDesc);
        }
    }
}