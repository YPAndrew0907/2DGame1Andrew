using System.Collections.Generic;
using UnityEngine;

namespace Obj
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
        
        public static Dictionary<PlayerSkill, string> SpecialConditionDesc = new()
        {
            { Obj.PlayerSkill.GuessOrRemember ,"集中观察/记忆"},
            { Obj.PlayerSkill.SwitchCard,"灵活之指"},
            { Obj.PlayerSkill.StealAndInsert,"灵活之指"},
            { Obj.PlayerSkill.Lie,"自然谎言"},
        };

        public static List<string> GetSkillDesc(IEnumerable<PlayerSkill> skills)
        {
            var str = new List<string>();
            foreach (var skill in skills)
            {
                str.Add(SpecialConditionDesc[skill]);
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