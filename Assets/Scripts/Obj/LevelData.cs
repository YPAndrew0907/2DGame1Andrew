using System.Collections.Generic;
using UnityEngine;

namespace Obj
{
    public class LevelData
    {
        public int      Level            { get; set; }
        public float    BossChip         { get; set; }
        public string   PlayerSkill      { get; set; }
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
            { Obj.PlayerSkill.SwitchCard, "灵活之指" },
            { Obj.PlayerSkill.StealAndInsert, "灵活之指" },
            { Obj.PlayerSkill.Lie, "自然谎言" },
        };

        public static List<string> GetSkillDesc(IEnumerable<PlayerSkill> skills)
        {
            var str = new List<string>();
            if (skills == null)
                return null;
            foreach (var skill in skills)
            {
                if (SpecialConditionDesc.ContainsKey(skill))
                {
                    str.Add(SpecialConditionDesc[skill]);
                }
            }

            return str;
        }
    }


    public enum PlayerSkill
    {
        None,
        NoInit= 1 << 1,
        // 集中观察 或 记忆 
        Guess = 1 << 2,
        Remember = 1 << 3,
        GuessOrRemember = Guess | Remember | NoInit,
        // 神速伸缩。开局带一张牌，自己回合可以进行更换。
        SwitchCard = 1 << 4,
        // 自然谎言
        Lie = 1 << 5,
        // 灵活之指
        StealAndInsert = 1 << 6,
    }
}