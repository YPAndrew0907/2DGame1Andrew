using System;
using System.Collections.Generic;
using System.Linq;
using Cfg;
using Obj;
using Unity.Mathematics;
using UnityEngine;
using XYZFrameWork.Base;
using Random = UnityEngine.Random;

namespace Mgr
{
    /// <summary>
    /// 单局状态管理（局内手牌、轮次、押注、牌桌流程等）
    /// </summary>
    public class GameSessionMgr : BaseSingle<GameSessionMgr>
    {
        public int           CurrentBossBet  { get; private set; }
        public int           CurrentPlayerBet  { get; private set; }
        public int           PlayerChips { get; private set; }
        public int           AIChips     { get; private set; }
        public List<CardObj> PlayerCards { get; private set; } = new();
        public List<CardObj> AICards     { get; private set; } = new();
        public List<CardObj> PlayerSkillCards  { get; private set; } = new();
        public List<CardObj> BossSkillCards  { get; private set; } = new();

        public PlayerType                 CurrentTurnPlayer    { get; private set; }
        public int                        RoundTimes           { get; private set; } // 回合
        public bool                       PlayerIsContinue     { get; set; }
        public bool                       AIIsContinue         { get; set; }
        public PlayerType                 LastPlayerType       { get; private set; }
        public PlayerType                 CurShuffleRole       { get; private set; } = PlayerType.None;
        public PlayerType                 CurPlayerType        { get; private set; } = PlayerType.None;
        public IReadOnlyList<PlayerSkill> CurPlayerSkills      { get; private set; }
        public IReadOnlyList<PlayerSkill> CurBossSkills        { get; private set; }
        public IReadOnlyList<CardObj>     LastRoundPlayerCards { get; private set; }
        public IReadOnlyList<CardObj>     LastRoundAICards     { get; private set; }

        public void InitSession(int playerChips, int aiChips, IReadOnlyList<PlayerSkill> playerSkills,
                                IReadOnlyList<PlayerSkill> bossSkills)
        {
            PlayerChips = playerChips;
            AIChips     = aiChips;
            CurrentPlayerBet  = 0;
            PlayerCards.Clear();
            AICards.Clear();
            PlayerSkillCards.Clear();
            BossSkillCards.Clear();
            CurrentTurnPlayer = PlayerType.Player;
            RoundTimes          = 0;
            PlayerIsContinue  = true;
            AIIsContinue      = true;
            CurPlayerSkills   = playerSkills ?? new List<PlayerSkill>();
            CurBossSkills     = bossSkills ?? new List<PlayerSkill>() ;
        }

        public void AddCard(CardObj card)
        {
            if (card == null)
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

        public void NextRound()
        {
            RoundTimes++;
            PlayerIsContinue = true;
            AIIsContinue     = true;
        }

        public void SwitchShufflePlayer()
        {
            LastPlayerType = CurrentTurnPlayer;
            CurrentTurnPlayer    = CurrentTurnPlayer == PlayerType.Player ? PlayerType.AI : PlayerType.Player;
        }

        public void SetBet(PlayerType playerType, int amount)
        {
            if (playerType == PlayerType.Player)
                CurrentPlayerBet = amount;
            else if (playerType == PlayerType.AI)
                CurrentBossBet = amount;
        }

        public void PayChip(bool playerIsWin)
        {
            if (playerIsWin)
            {
                AIChips     =  math.max(0, AIChips - CurrentBossBet);
                PlayerChips += CurrentBossBet;
            }
            else
            {
                PlayerChips =  math.max(0, PlayerChips - CurrentPlayerBet);
                AIChips     += CurrentPlayerBet;
            }
        }

        public void SelectSkill(PlayerType player, PlayerSkill selectSkill)
        {
            if (selectSkill == PlayerSkill.None)
                return;

            var  list      = new List<PlayerSkill>();
            var  srcSkills = (player == PlayerType.Player) ? CurPlayerSkills : CurBossSkills;
            bool hasFind   = false;

            for (var index = 0; index < srcSkills.Count; index++)
            {
                var skill = srcSkills[index];
                if (hasFind)
                {
                    list.Add(skill);
                    continue;
                }
                
                if (skill.HasFlag(selectSkill) && !skill.Equals(selectSkill))
                {
                    // 避免重复添加selectSkill
                        list.Add(selectSkill);
                        hasFind = true;
                }
                else
                {
                    list.Add(skill);
                }
            }

            if (player == PlayerType.Player)
                CurPlayerSkills = list;
            else if (player == PlayerType.AI)
                CurBossSkills = list;
        }
        
        /// <summary>
        /// 切换洗牌人员
        /// </summary>
        public void NextShuffleRole()
        {
            if (CurShuffleRole == PlayerType.None)
            {
                // CurShuffleRole = Random.Range(-1, 1) >= 0 ? PlayerType.Player : PlayerType.AI;
                CurShuffleRole = PlayerType.Player;
            }
            else
            {
                if (CurShuffleRole == PlayerType.Player)
                    CurShuffleRole = PlayerType.AI;
                else if (CurShuffleRole == PlayerType.AI)
                    CurShuffleRole = PlayerType.Player;
            }
        }
        
        /// <summary>
        /// 切换下一个要牌的玩家
        /// </summary>
        /// <returns>下一个要牌的玩家类型</returns>
        public PlayerType NextPlayerAskCard()
        {
            LastPlayerType = CurPlayerType;
            switch (CurPlayerType)
            {
                case PlayerType.None:
                    // 随机一下
                    if (AIIsContinue && PlayerIsContinue)
                        CurPlayerType = Random.Range(0f, 1.0f) > 0.5f ? PlayerType.Player : PlayerType.AI;
                    else if (AIIsContinue)
                        CurPlayerType = PlayerType.AI;
                    else if (PlayerIsContinue)
                        CurPlayerType = PlayerType.Player;
                    break;
                case PlayerType.Player:
                    CurPlayerType = AIIsContinue ? PlayerType.AI : PlayerIsContinue ? PlayerType.Player : PlayerType.None;
                    break;
                case PlayerType.AI:
                    CurPlayerType = PlayerIsContinue ? PlayerType.Player : AIIsContinue ? PlayerType.AI : PlayerType.None;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return CurPlayerType;
        }

        // 下注判断等
        public bool PlayerEnough => PlayerChips >= LevelMgr.Instance.CurMinBetChip;
        public bool BossEnough   => AIChips >= LevelMgr.Instance.CurMinBetChip;

        // 轮次/洗牌等
        public bool WillShuffle => RoundTimes  <= 0 || RoundTimes % 5 == 0;

        // 牌数量
        public int CurAICardCount     => AICards?.Count ?? 0;
        public int CurPlayerCardCount => PlayerCards?.Count ?? 0;

        public int CurAICardTotalNum => CardMgr.TotalCardNum(AICards);
        public int CurPlayerCardTotalNum => CardMgr.TotalCardNum(PlayerCards);

        public void StoreLastCard()
        {
            var list = new List<CardObj>(PlayerCards);
            list.AddRange(AICards);
            NotifyMgr.SendEvent(NotifyDefine.COLLECT_PLAYED_CARD, list);
            LastRoundPlayerCards = PlayerCards;
            LastRoundAICards     = AICards;
            PlayerCards          = new List<CardObj>();
            AICards              = new List<CardObj>();
        }

        public void PushSkillCard(bool isAi,params CardObj[] cardList)
        {
            foreach (var cardObj in cardList)
            {
                cardObj.Owner = isAi? PlayerType.AI : PlayerType.Player;
            }
            if (!isAi)
            {
                PlayerSkillCards.AddRange(cardList);
            }
            else
            {
                BossSkillCards.AddRange(cardList);
            }
        }

        public void PopSkillCard(PlayerType ai,params CardObj[] cardObjs)
        {
            if (ai == PlayerType.Player)
            {
                PlayerSkillCards.RemoveAll(cardObjs.Contains);
            }
            else if (ai == PlayerType.AI)
            {
                BossSkillCards.RemoveAll(cardObjs.Contains);
            }
        }

        public void SwitchCard(bool isAI, int[] curIndex, int[] replaceIndex)
        {
            if (curIndex.Length!= replaceIndex.Length )
            {
                return;
            }
            if (isAI)
            {
                for (var i = 0; i < curIndex.Length; i++)
                {
                    var ci = curIndex[i];
                    var ri = replaceIndex[i];
                    (AICards[ci], BossSkillCards[ri]) = (BossSkillCards[ri], AICards[ci]);
                    Debug.Log($"【替换牌】： AI 手牌{AICards[ci]} -> {BossSkillCards[ri]}");
                }
            }
            else
            {
                for (var i = 0; i < curIndex.Length; i++)
                {
                    var ci = curIndex[i];
                    var ri = replaceIndex[i];
                    (PlayerCards[ci], PlayerSkillCards[ri]) = (PlayerSkillCards[ri], PlayerCards[ci]);
                    Debug.Log($"【替换牌】：玩家 手牌{PlayerCards[ci]} -> {PlayerSkillCards[ri]}");
                }
            }
        }
    }
}
