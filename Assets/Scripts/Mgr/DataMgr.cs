using System;
using System.Collections.Generic;
using System.Linq;
using Cfg;
using Newtonsoft.Json;
using Obj;
using UnityEngine;
using XYZFrameWork;
using XYZFrameWork.Base;
using Random = UnityEngine.Random;

namespace Mgr
{
    public class DataMgr: BaseSingle<DataMgr>
    {
        public DataMgr()
        {
            _curLevel = PlayerPrefs.GetInt(CurLevelStr, 1);
            _money = PlayerPrefs.GetInt(MoneyStr, 200);
            NotifyMgr.Register(NotifyDefine.GAME_END,SaveGameInfo);
            NotifyMgr.Register(NotifyDefine.GAME_END,Reset);
            NotifyMgr.Register(NotifyDefine.GAME_READY,LoadLevelInfo);
        }

        
        
        ~DataMgr()
        {
            NotifyMgr.UnRegister(NotifyDefine.GAME_END,SaveGameInfo);
        }

        private int _curLevel = 1;
        public int CurLevel=> _curLevel;

        private int _money = 0;
        public int Money=> _money;
        
        
        

        #region 当前关卡信息

        private Dictionary<int,LevelData>  LevelDataDict { get; set; }= new();
        
        // 技能最大操控纸牌数量
        // -1 表示没有上限
        public  int MaxSkillCardCount { get; private set;  }

        // 当前关卡Boss筹码数量
        // 如果 小于1 表示倍数
        public float BossChip { get; private set; }
        
        // 由谁发牌。-1 对方，0 随机，1 玩家。
        public int ShuffleRole    { get; private set; }
        // 当前发牌人员。 -1 表示需要随机。
        public int CurShuffleRole { get; private set; } = -1;
        public int TableLevel     { get; private set; } = 0;
        public int CurMinBetMoney    => 5 * TableLevel;
        public int CurMaxBetMoney    => 50 * TableLevel;
        public int AIChip         { get; private set; } = 0;

        public IReadOnlyList<PlayerSkill> CurSkills { get; private set; }

        public IReadOnlyList<CardObj> CurLevelInitCard { get; private set; }

        public void LoadLevelInfo(NotifyMsg notifyMsg) 
        {
            if (notifyMsg.Param is NormalParam param)
            {
                LevelData levelData;
                if (LevelDataDict.Count == 0)
                {
                    var json = Resources.Load<TextAsset>("LevelData");
                    var levelDatas = JsonConvert.DeserializeObject<List<LevelData>>(json.text);
                    LevelDataDict = levelDatas.ToDictionary(x => x.Level);
                }

                if (!LevelDataDict.TryGetValue(param.IntValue, out levelData))
                {
                    Debug.LogError("关卡数据不存在");
                    return;
                }

                MaxSkillCardCount = levelData.MaxCard;
                BossChip = levelData.BossChip;
                TableLevel = levelData.TableLevel;
                
                ParseAIData(levelData);
                ParseCarryCard(levelData);
                ParsePlayerSkill(levelData);
                ParseSpecialCondition(levelData);
            }
        }

        private void ParseAIData(LevelData levelData)
        {
            if (levelData.BossChip>100)
            {
                AIChip = (int)levelData.BossChip;
            }
            else
            {
                AIChip = (int)(levelData.BossChip * Money);
            }
        }

        private void ParseSpecialCondition(LevelData levelData)
        {
            var SCs = levelData.SpecialCondition.Split("|");
            
            foreach (var SC in SCs)
            {
                if (!string.IsNullOrEmpty(SC))
                {
                    var sc = Enum.Parse<SpecialCondition>(SC);
                    switch (sc)
                    {
                        case SpecialCondition.BossShuffle:
                            ShuffleRole    = -1;
                            CurShuffleRole = -1;
                            break;
                        default: throw new ArgumentOutOfRangeException();
                    }
                }
                
            }
        }

        private void ParsePlayerSkill(LevelData levelData)
        {
            var skillList = new List<PlayerSkill>();
            var skillStr = levelData.PlayerSkill.Split('|');
            foreach (var str in skillStr)
            {
                skillList.Add(Enum.Parse<PlayerSkill>(str));
            }
            
            CurSkills = skillList;
        }

        private void ParseCarryCard(LevelData levelData)
        {
            // 解析 携带的纸牌信息
            List<CardObj> list = new List<CardObj>();
            foreach (var str in levelData.CarryCard)
            {
                var      strs   = str.Split('_');
                var      cardV  = Enum.Parse<CardValue>(strs[0]);
                var      sIndex = int.Parse(strs[1]);
                CardSuit cardS;
                if (sIndex > 0)
                {
                    cardS = (CardSuit) sIndex; 
                }
                else
                {
                    cardS = (CardSuit)Random.Range(1,5);   
                }
                list.Add(new CardObj(cardV,cardS));
            }
            CurLevelInitCard = list;
        }

        #endregion
        
        #region 技能相关

        public PlayerSkill GuessOrRemember = PlayerSkill.None;
        #endregion

        #region 数据变更方法

        // 关卡结束重置
        public void Reset(NotifyMsg notifyMsg)
        {
            MaxSkillCardCount = 0;
            BossChip          = 0;
            ShuffleRole       = 0;
            CurShuffleRole    = -1;
            CurSkills         = null;
            CurLevelInitCard  = null;
            GuessOrRemember   = PlayerSkill.None;
        }

        public void NextShuffleRole()
        {
            if (ShuffleRole != 0)
            {
                return;
            }

            if (CurShuffleRole == 0)
            {
                CurShuffleRole = Random.Range(-1, 1)>=0? 1 : -1;
            }
            else
            {
                CurShuffleRole *= -1;
            }
        }
        
        public void SaveGameInfo(NotifyMsg notifyMsg)
        {
            PlayerPrefs.SetInt(CurLevelStr, 1);
            PlayerPrefs.SetInt(MoneyStr,_money);
        }

        #endregion
        
        
        private const string CurLevelStr = "CurLevel";
        private const string MoneyStr = "MoneyStr";
    }
}