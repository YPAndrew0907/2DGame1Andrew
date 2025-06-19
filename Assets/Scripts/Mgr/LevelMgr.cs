using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Obj;
using UnityEngine;
using XYZFrameWork.Base;

namespace Mgr
{
    public class LevelMgr : BaseSingle<LevelMgr>
    {
        
        private const    string                     Key_Level     = "Profile_Level";
        private readonly Dictionary<int, LevelData> _levelConfigs = new();

        // 缓存字段
        private int    _tableLevel;
        private int    _curMinBetChip;
        private int    _curMaxBetChip;
        private int    _bossChip;
        private int    _aiChip;
        private string _aiName;

        public int    TableLevel    => _tableLevel;
        public int    CurMinBetChip => _curMinBetChip;
        public int    CurMaxBetChip => _curMaxBetChip;
        public int    BossChip      => _bossChip;
        public int    AIChip        => _aiChip;
        public string AIName        => _aiName;


        private int _level;
        public  int CurrentLevel => _level;

        private List<PlayerSkill>          _levelBossSkill;
        public  IReadOnlyList<PlayerSkill> LevelBossSkill=> _levelBossSkill;


        public LevelMgr()
        {
            LoadAllLevels();
        }

        public void SetCurrentLevel(int level)
        {
            _level = Mathf.Clamp(level, 1, _levelConfigs.Count);
            UpdateLevelCache();
        }

        public void LoadAllLevels()
        {
            
            var json = Resources.Load<TextAsset>("LevelData");
            if (json == null)
            {
                Debug.LogError("LevelData 配置文件未找到");
                return;
            }
            
            _level = PlayerPrefs.GetInt(Key_Level, 1);

            var list = JsonConvert.DeserializeObject<List<LevelData>>(json.text);
            _levelConfigs.Clear();
            foreach (var l in list)
                _levelConfigs[l.Level] = l;

            // 默认加载第一个关卡数据
            UpdateLevelCache();
        }

        private void UpdateLevelCache()
        {
            if (!_levelConfigs.TryGetValue(CurrentLevel, out var data))
                data = null;
            _tableLevel    = data?.TableLevel ?? 1;
            _curMinBetChip = 5 * _tableLevel;
            _curMaxBetChip = 50 * _tableLevel;
            _bossChip      = (int)(data?.BossChip ?? 0);
            _aiChip        = (int)(data?.BossChip ?? 0); // 可分离
            _aiName        = data?.LevelAIName ?? "AI";
        
            var skillStr = data?.BossSkill;
            if (skillStr!= null)
            {
                List<PlayerSkill> termList = new List<PlayerSkill>();
                skillStr.Split("|").ToList().ForEach(str =>
                {
                    termList.Add(Enum.Parse<PlayerSkill>(str));
                });
                _levelBossSkill = termList;
            }
        }

        public LevelData GetLevelData(int levelId)
        {
            _levelConfigs.TryGetValue(levelId, out var data);
            return data;
        }

        public LevelData GetCurrentLevelData()
        {
            return GetLevelData(CurrentLevel);
        }

        public void SaveLevel()
        {
            PlayerPrefs.SetInt(Key_Level, _level);
        }
    }
}