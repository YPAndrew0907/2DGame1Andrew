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
            NotifyMgr.RegisterNotify(NotifyDefine.GAME_END,SaveGameInfo);
            NotifyMgr.RegisterNotify(NotifyDefine.NEXT_ROUND,NextRound);
            LoadLevelData();
        }
        
        ~DataMgr()
        {
            NotifyMgr.UnRegisterNotify(NotifyDefine.GAME_END,SaveGameInfo);
            NotifyMgr.UnRegisterNotify(NotifyDefine.NEXT_ROUND,NextRound);
        }

        private int _curLevel = 1;
        public int CurLevel=> _curLevel;

        private int _money = 0;
        public  int Money
        {
            get => _money;
            private set
            {
                _money = value;
                NotifyMgr.SendEvent(NotifyDefine.MONEY_CHANGE,_money);
            }
        }

        #region 对局数据

        public bool PlayerIsContinue { get; set; }  // 玩家是否还在要牌
        public bool AIIsContinue     { get; set; } // 对面是否还在要牌

        public  PlayerType LastPlayerType { get; private set; } = PlayerType.None;
        public  PlayerType CurPlayerType  => _curPlayerType; // 当前要牌的人。
        private PlayerType _curPlayerType = PlayerType.None;

        #endregion

        #region 当前关卡信息

        private Dictionary<int,LevelData>  LevelDataDict { get; set; }= new();
        
        // 技能最大操控纸牌数量
        // -1 表示没有上限
        public  int CollectCardCount { get; private set;  }

        // 当前关卡Boss筹码数量
        // 如果 小于1 表示倍数
        public float BossChip          { get; private set; }
        public int   RememberCardCount { get; private set; }
        
        // 当前洗牌人员。 None 表示需要随机决定。
        public PlayerType CurShuffleRole { get; private set; } = PlayerType.None;
        public int        TableLevel     { get; private set; } = 0;
        public int        CurMinBetChip  => 5 * TableLevel;
        public int        CurMaxBetChip  => 50 * TableLevel;
        public int        CurBetChip     { get; private set; }
        public int        AIChip         { get; private set; } = 0;
        public string     AIName         { get; private set; }

        public int StartMoney { get; private set; }

        public string PlayerName => "You";

        // 游戏轮次，五次一洗牌。 -1 未开始 
        public int TurnTimes { get; private set; }
 
        public          IReadOnlyList<PlayerSkill> CurSkills    { get; private set; }
        public          List<string>               CurSkillDesc => LevelData.GetSkillDesc(CurSkills);
        public readonly List<CardObj>              LastRoundPlayerCards = new();
        public readonly List<CardObj>              LastRoundAICards     = new();
        public readonly List<CardObj>              AICards              = new();
        public readonly List<CardObj>              PlayerCards          = new();


        public IReadOnlyList<CardObj> CurLevelInitCarryCard { get; private set; }

        public void LoadCurLevelInfo()
        {
            ResetInLevelData();
            if (!LevelDataDict.TryGetValue(_curLevel, out var levelData))
            {
                Debug.LogError("关卡数据不存在");
                return;
            }

            CollectCardCount  = levelData.MaxCard < 0 ? int.MaxValue: levelData.MaxCard;
            RememberCardCount = 3; // 默认记牌数量设置 3
            BossChip          = levelData.BossChip;
            TableLevel        = levelData.TableLevel;
                
            ParseAIData(levelData);
            ParseCarryCard(levelData);
            ParsePlayerSkill(levelData);
            ParseSpecialCondition(levelData);
                
            // 初始化 要牌玩家
            NextPlayerAskCard();
            NextShuffleRole();
            StartMoney = Money;
        }

        private void LoadLevelData()
        {
            var json       = Resources.Load<TextAsset>("LevelData");
            var levelDatas = JsonConvert.DeserializeObject<List<LevelData>>(json.text);
            LevelDataDict = levelDatas.ToDictionary(x => x.Level);
        }

        private void NextRound(NotifyMsg obj)
        {
            PlayerIsContinue = true;
            AIIsContinue     = true;
        }
        private void ParseAIData(LevelData levelData)
        {
            if (levelData.BossChip > 100)
            {
                AIChip = (int)levelData.BossChip;
            }
            else
            {
                AIChip = (int)(levelData.BossChip * Money);
            }

            AIName = levelData.LevelAIName;
        }
        private void ParseSpecialCondition(LevelData levelData)
        {
            if (levelData.SpecialCondition == null) return;

            var scs = levelData.SpecialCondition.Split("|");
            
            foreach (var specialCondition in scs)
            {
                if (!string.IsNullOrEmpty(specialCondition))
                {
                    var sc = Enum.Parse<SpecialCondition>(specialCondition);
                    switch (sc)
                    {
                        case SpecialCondition.BossShuffle:
                            // 默认会先执行 NextShuffle 方法，再取值，所以对方洗牌就设置player
                            CurShuffleRole = PlayerType.Player; 
                            break;
                        default: throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
        private void ParsePlayerSkill(LevelData levelData)
        {
            var skillList = new List<PlayerSkill>();
            if (levelData.PlayerSkill != null)
            {
                var skillStr = levelData.PlayerSkill.Split('|');
                foreach (var str in skillStr)
                {
                    skillList.Add(Enum.Parse<PlayerSkill>(str));
                }
            }
            CurSkills = skillList;
        }

        private void ParseCarryCard(LevelData levelData)
        {
            if (levelData.CarryCard == null) return;
            
            // 解析 携带的纸牌信息
            // 牌面值起头，花色结尾。-1 表示随机 如 A_-1 ，随机花色 A。两张就是 两个A_-1
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
            CurLevelInitCarryCard = list;
        }
        
        #endregion
        
        #region 技能相关

        // // GuessOrRemember 的初始化。 none表示没有这个技能。 ==GuessOrRemember，表示没初始化
        // public PlayerSkill GuessOrRemember = PlayerSkill.None;
        #endregion

        #region 数据获取

        public int CurAICardNum => TotalCardNum(AICards);

        public int CurPlayer => TotalCardNum(PlayerCards);

        private int TotalCardNum(List<CardObj> list)
        {
            if (list is not {Count:>0})
            {
                return 0;
            }
            
            int i = 0;
            foreach (var card in AICards)
            {
                i += (int)card.Value + 1;
            }

            return i;
        }

        #endregion
        
        #region 数据变更方法

        // 关卡结束重置
        private void ResetInLevelData()
        {
            CollectCardCount     = 0;
            BossChip              = 0;
            AIIsContinue          = true;
            PlayerIsContinue      = true;
            CurShuffleRole        = PlayerType.None;
            CurSkills             = null;
            CurLevelInitCarryCard = null;
            
            TurnCounter(true);
            AddCardToAIOrPlayer(null);
        }
        public void NextLevel()
        {
            _curLevel++;
            _curLevel = Math.Clamp( _curLevel,1, LevelDataDict.Count);
            LoadCurLevelInfo();
        }
        public void LastLevel()
        {
            _curLevel--;
            _curLevel = Math.Clamp( _curLevel,1, LevelDataDict.Count);
            LoadCurLevelInfo();
        }
        public void TurnCounter(bool reset = false)
        {
            if(reset) TurnTimes = 0;
            else TurnTimes++;
        }
        public void NextShuffleRole()
        {
            if (CurShuffleRole == PlayerType.None)
            {
                CurShuffleRole = Random.Range(-1, 1) >= 0? PlayerType.Player : PlayerType.AI;
            }
            else
            {
                if (CurShuffleRole == PlayerType.Player)
                    CurShuffleRole = PlayerType.AI;
                if (CurShuffleRole == PlayerType.AI)
                    CurShuffleRole = PlayerType.Player;
            }
        }
        public PlayerType NextPlayerAskCard()
        {
            LastPlayerType = _curPlayerType;
            switch (_curPlayerType)
            {
                case PlayerType.None:
                    // 随机一下
                    if (AIIsContinue && PlayerIsContinue)
                        _curPlayerType = Random.Range(0f, 1.0f) > 0.5f ? PlayerType.Player : PlayerType.AI;
                    else if (AIIsContinue)
                        _curPlayerType = PlayerType.AI;
                    else if (PlayerIsContinue)
                        _curPlayerType = PlayerType.Player;
                    break;
                case PlayerType.Player:
                    _curPlayerType = AIIsContinue ? PlayerType.AI : PlayerIsContinue? PlayerType.Player: PlayerType.None;
                    break;
                case PlayerType.AI:  
                    _curPlayerType = PlayerIsContinue ? PlayerType.Player : AIIsContinue? PlayerType.AI: PlayerType.None;
                    break;
                default:                throw new ArgumentOutOfRangeException();
            }
            return _curPlayerType;
        }
        public void SaveGameInfo(NotifyMsg notifyMsg)
        {
            PlayerPrefs.SetInt(CurLevelStr, 1);
            PlayerPrefs.SetInt(MoneyStr,_money);
        }

        public void PayChip(bool  playerIsWin)
        {
            if (playerIsWin)
            {
                BossChip -= CurBetChip;
                Money    += CurBetChip;
            }
            else
            {
                BossChip += CurBetChip;
                Money    -= CurBetChip;
            }
        }

        public void AddCardToAIOrPlayer(CardObj card)
        {
            if (card == null )
            {
                PlayerCards?.Clear();
                AICards?.Clear();
                return;
            }
                
            switch (card.Owner)
            {
                case PlayerType.Player: 
                    PlayerCards.Add(card);
                    break;
                case PlayerType.AI:     
                    AICards.Add(card);
                    break;
                default:
                    Debug.LogError(LogTxt.PARAM_ERROR);
                    break;
            }
        }

        public void SkillSelect(PlayerSkill skill)
        {
            List<PlayerSkill> term = new List<PlayerSkill>();
            foreach (var s in CurSkills)
            {
                if (!s.HasFlag(skill))
                {
                    term.Add(s);
                }
            }
            term.Add(skill);

            CurSkills = term;
        }
        
        public void BetChip(int money)
        {
            CurBetChip = money;
        }

        public void ClearPlayerCards()
        {
            LastRoundPlayerCards.Clear();
            LastRoundPlayerCards.AddRange(PlayerCards);
            PlayerCards.Clear();
            LastRoundAICards.Clear();
            LastRoundAICards.AddRange(AICards);
            AICards.Clear();
        }
        
        #endregion

        #region check

        public bool          PlayerEnough     => _money > CurMinBetChip;
        public bool          BossEnough       => BossChip > CurMinBetChip;

        public bool WillShuffle()
        {
            return TurnTimes is <= 0 or >= 5;
        }
        
        #endregion
        
        private const string CurLevelStr = "CurLevel";
        private const string MoneyStr = "MoneyStr";
    }
}