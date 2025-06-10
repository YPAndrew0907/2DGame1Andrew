using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Obj;
using UnityEngine;
using XYZFrameWork.Base;

namespace Mgr
{
    /// <summary>
    /// 技能管理
    /// </summary>
    public class SkillMgr : BaseSingle<SkillMgr>
    {
        private readonly Dictionary<PlayerSkill, SkillConfig> _skillConfigs  = new();
        private          Dictionary<PlayerSkill, int>         _skillLevels   = new();
        private const    string                               SkillDataKey   = "PlayerSkillLevels";
        private const    string                               Key_SkillPoint = "Profile_SkillPoint";

        public int SkillPoint { get; private set; }

        public int SkillCardCount { get; private set; } // 收集卡牌的总数量

        public SkillMgr()
        {
            LoadSkillConfig();
            LoadSkillData();
        }

        public void LoadSkillConfig()
        {
            var txtJons = Resources.Load<TextAsset>("SkillData");
            if (txtJons != null)
            {
                SkillConfigRoot configRoot = JsonConvert.DeserializeObject<SkillConfigRoot>(txtJons.text);
                _skillConfigs.Clear();
                foreach (var config in configRoot.skills)
                    _skillConfigs[config.skillType] = config;
            }
            else
            {
                Debug.LogError("技能配置文件 SkillData 不存在。");
            }
        }

        public void LoadSkillData()
        {
            _skillLevels.Clear();
            string json = PlayerPrefs.GetString(SkillDataKey, "{}");
            _skillLevels   = JsonConvert.DeserializeObject<Dictionary<PlayerSkill, int>>(json);
            #if UNITY_EDITOR
            _skillLevels.TryAdd(PlayerSkill.CopyAndSwitch, 1);
            _skillLevels.TryAdd(PlayerSkill.StealAndInsert, 1);
            _skillLevels.TryAdd(PlayerSkill.GuessOrRemember, 1);
            _skillLevels.TryAdd(PlayerSkill.Lie, 1);
            _skillLevels.TryAdd(PlayerSkill.Detect, 1);
            
            #endif
            SkillCardCount = 0;
            if (_skillLevels.ContainsKey(PlayerSkill.CopyAndSwitch))
            {
                var (param, _) =  GetSkillParameters(PlayerSkill.CopyAndSwitch,0);
                SkillCardCount += (int)param;
            }

            if (_skillLevels.ContainsKey(PlayerSkill.StealAndInsert))
            {
                var (param, _)     =  GetSkillParameters(PlayerSkill.StealAndInsert,0);
                SkillCardCount += (int)param;
            }


            SkillPoint = PlayerPrefs.GetInt(Key_SkillPoint, 0);
        }

        public void SaveSkillData()
        {
            string json = JsonConvert.SerializeObject(_skillLevels);
            PlayerPrefs.SetString(SkillDataKey, json);
            PlayerPrefs.Save();
        }

        public void UpgradeSkill(PlayerSkill skill)
        {
            if (!_skillLevels.TryAdd(skill, 1))
            {
                _skillLevels[skill]++;
                
                SaveSkillData();
            }
        }

        public void UnlockSkill(PlayerSkill skill)
        {
            if (!_skillLevels.ContainsKey(skill))
            {
                _skillLevels[skill] = 1;
                SaveSkillData();
            }
        }

        public void WinUnLockSkill(IEnumerable<PlayerSkill> list)
        {
            foreach (var playerSkill in list)
            {
                UnlockSkill(playerSkill);
            }
            
            SkillPoint += LevelMgr.Instance.CurrentLevel;
            PlayerPrefs.SetInt(Key_SkillPoint, SkillPoint);
        }

        public bool IsUnLock(PlayerSkill skill) => _skillLevels.ContainsKey(skill);

        public List<PlayerSkill> UnLockSkillList() => _skillLevels.Keys.ToList();

        public int GetSkillLevel(PlayerSkill skill)
        {
            _skillLevels.TryGetValue(skill, out int level);
            return level;
        }

        public SkillConfig GetSkillConfig(PlayerSkill skill)
        {
            _skillConfigs.TryGetValue(skill, out var config);
            return config;
        }

        public IEnumerable<SkillConfig> GetAllSkillConfigs()
        {
            return _skillConfigs.Values;
        }

        public void SpendSkillPoint(int count)
        {
            if (count < 0)
                return; // 或者什么都不做

            SkillPoint = Math.Max(0, SkillPoint - count);
        }
        
        
        /// <summary>
        /// 获取技能参数
        /// </summary>
        /// <param name="skill"></param>
        /// <param name="level"> 0 时获取玩家的等级。</param>
        public (float hitRate, int parameter) GetSkillParameters(PlayerSkill skill, int level = 1)
        {
            if (_skillConfigs.TryGetValue(skill, out SkillConfig config))
            {
                int lv = level != 0 ? level : GetSkillLevel(skill);
                float hitRate = config.baseHitRate + config.hitRateIncreasePerLevel * (lv - 1);
                int parameter = config.baseParameter + config.parameterIncreasePerLevel * (lv - 1);
                return (hitRate, parameter);
            }
            return (0f, 0);
        }
    }
}
