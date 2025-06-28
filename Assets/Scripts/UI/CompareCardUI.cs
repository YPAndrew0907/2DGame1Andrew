using System;
using System.Collections.Generic;
using Base;
using Cfg;
using Mgr;
using Obj;
using UnityEngine;

namespace UI
{
    public class CompareCardUI : BaseViewMono
    {
        // AUTO-GENERATE
        private UnityEngine.UI.Button _btnGiveUp;
        private UnityEngine.UI.Button BtnGiveUp
            => _btnGiveUp ??= transform.Find("go_Bg/bg/btn_GiveUp").GetComponent<UnityEngine.UI.Button>();

        private UnityEngine.UI.Button _btnNextRound;
        private UnityEngine.UI.Button BtnNextRound
            => _btnNextRound ??= transform.Find("go_Bg/bg/btn_NextRound").GetComponent<UnityEngine.UI.Button>();

        private UI.CardZone _monoAICards;
        private UI.CardZone MonoAICards
            => _monoAICards ??= transform.Find("go_Bg/bg/AIScore/mono_AICards").GetComponent<UI.CardZone>();

        private UI.CardZone _monoPlayerCards;
        private UI.CardZone MonoPlayerCards
            => _monoPlayerCards ??= transform.Find("go_Bg/bg/PlayerScore/mono_PlayerCards").GetComponent<UI.CardZone>();

        private UnityEngine.GameObject _goBg;
        private UnityEngine.GameObject GoBg
            => _goBg ??= transform.Find("go_Bg").gameObject;

        private TMPro.TextMeshProUGUI _txtAICardNum;
        private TMPro.TextMeshProUGUI TxtAICardNum
            => _txtAICardNum ??= transform.Find("go_Bg/bg/AIScore/txt_AICardNum").GetComponent<TMPro.TextMeshProUGUI>();

        private TMPro.TextMeshProUGUI _txtAIName;
        private TMPro.TextMeshProUGUI TxtAIName
            => _txtAIName ??= transform.Find("go_Bg/bg/AIScore/txt_AIName").GetComponent<TMPro.TextMeshProUGUI>();

        private TMPro.TextMeshProUGUI _txtAIResult;
        private TMPro.TextMeshProUGUI TxtAIResult
            => _txtAIResult ??= transform.Find("go_Bg/bg/AIScore/txt_AIResult").GetComponent<TMPro.TextMeshProUGUI>();

        private TMPro.TextMeshProUGUI _txtPlayerCardNum;
        private TMPro.TextMeshProUGUI TxtPlayerCardNum
            => _txtPlayerCardNum ??= transform.Find("go_Bg/bg/PlayerScore/txt_PlayerCardNum").GetComponent<TMPro.TextMeshProUGUI>();

        private TMPro.TextMeshProUGUI _txtPlayerName;
        private TMPro.TextMeshProUGUI TxtPlayerName
            => _txtPlayerName ??= transform.Find("go_Bg/bg/PlayerScore/txt_PlayerName").GetComponent<TMPro.TextMeshProUGUI>();

        private TMPro.TextMeshProUGUI _txtPlayerResult;
        private TMPro.TextMeshProUGUI TxtPlayerResult
            => _txtPlayerResult ??= transform.Find("go_Bg/bg/PlayerScore/txt_PlayerResult").GetComponent<TMPro.TextMeshProUGUI>();

        // AUTO-GENERATE-END
        private const string WinStr   = "<color=green>Win</color>";
        private const string LossStr  = "<color=red>Loss</color>";
        private const string DrawStr  = "<color=yellow>Draw</color>";
        private const string OutStr   = "<color=red>Out</color>";
        private const string CheatStr = "<color=orange>Cheat</color>";

        /// <summary>
        /// 0 = 未结束，-1 = AI胜，1 = 玩家胜
        /// </summary>
        private int _hasEnd = 0;

        public void Show(List<KeyValuePair<string, IReadOnlyList<CardObj>>> data)
        {
            if (data == null || data.Count < 2)
            {
                Debug.LogError(LogTxt.PARAM_ERROR);
                return;
            }

            string playerResult = "";
            string aiResult = "";

            // ----------- 10回合强制结束 -----------
            if (GameSessionMgr.Instance.RoundTimes == 10)
            {
                int playerChips = GameSessionMgr.Instance.PlayerChips;
                int aiChips     = GameSessionMgr.Instance.AIChips;

                if (playerChips > aiChips)
                {
                    playerResult = WinStr;
                    aiResult     = LossStr;
                    _hasEnd = 1;
                }
                else if (playerChips < aiChips)
                {
                    playerResult = LossStr;
                    aiResult     = WinStr;
                    _hasEnd = -1;
                }
                else
                {
                    playerResult = DrawStr;
                    aiResult     = DrawStr;
                    _hasEnd = 0;
                }

                TxtPlayerName.text    = data[0].Key;
                TxtPlayerResult.text  = playerResult;
                TxtPlayerCardNum.text = playerChips.ToString();
                MonoPlayerCards.ClearCard();
                MonoPlayerCards.RefreshCard();

                TxtAIName.text    = data[1].Key;
                TxtAIResult.text  = aiResult;
                TxtAICardNum.text = aiChips.ToString();
                MonoAICards.ClearCard();
                MonoAICards.RefreshCard();

                GoBg.SetActive(true);

                // 游戏结束，禁用下一回合按钮
                BtnNextRound.gameObject.SetActive(false);
                return;
            }

            // ----------- 正常流程 -----------
            var playerCards = data[0].Value;
            var aiCards     = data[1].Value;
            var playerNum   = CardMgr.TotalCardNum(playerCards);
            var aiNum       = CardMgr.TotalCardNum(aiCards);

            // ----------- 作弊判定 -----------
            if (playerNum == -1 || aiNum == -1)
            {
                if (playerNum == -1 && aiNum == -1)
                {
                    playerResult = CheatStr;
                    aiResult     = CheatStr;
                    _hasEnd = 0;
                }
                else if (playerNum == -1)
                {
                    playerResult = CheatStr;
                    aiResult     = WinStr;
                    _hasEnd = -1;
                }
                else // aiNum == -1
                {
                    playerResult = WinStr;
                    aiResult     = CheatStr;
                    _hasEnd = 1;
                }

                TxtPlayerName.text    = data[0].Key;
                TxtPlayerResult.text  = playerResult;
                TxtPlayerCardNum.text = playerNum == -1 ? "-" : playerNum.ToString();
                MonoPlayerCards.SetCard(playerCards, CardMgr.IsCardShowCompareResult);
                MonoPlayerCards.RefreshCard();

                TxtAIName.text    = data[1].Key;
                TxtAIResult.text  = aiResult;
                TxtAICardNum.text = aiNum == -1 ? "-" : aiNum.ToString();
                MonoAICards.SetCard(aiCards, CardMgr.IsCardShowCompareResult);
                MonoAICards.RefreshCard();

                GoBg.SetActive(true);
                // 不在作弊这里控制下一回合按钮
                // 让最后统一控制即可
            }
            else
            {
                // ----------- 普通结算 -----------
                var aiResultValue = AIMgr.AIIsLoss(aiNum, playerNum); // 1=AI输，-1=AI赢，0=平
                GameSessionMgr.Instance.PayChip(aiResultValue);

                // 结算后押注资格
                bool playerEnough = GameSessionMgr.Instance.PlayerEnough;
                bool bossEnough   = GameSessionMgr.Instance.BossEnough;
                _hasEnd = playerEnough && bossEnough ? 0 : playerEnough ? 1 : -1;

                // 玩家结果
                if (!playerEnough)
                    playerResult = OutStr;
                else if (aiResultValue == 1)
                    playerResult = WinStr;
                else if (aiResultValue == -1)
                    playerResult = LossStr;
                else
                    playerResult = DrawStr;

                // AI结果
                if (!bossEnough)
                    aiResult = OutStr;
                else if (aiResultValue == 1)
                    aiResult = LossStr;
                else if (aiResultValue == -1)
                    aiResult = WinStr;
                else
                    aiResult = DrawStr;

                TxtPlayerName.text    = data[0].Key;
                TxtPlayerResult.text  = playerResult;
                TxtPlayerCardNum.text = playerNum.ToString();
                MonoPlayerCards.SetCard(playerCards, CardMgr.IsCardShowCompareResult);
                MonoPlayerCards.RefreshCard();

                TxtAIName.text    = data[1].Key;
                TxtAIResult.text  = aiResult;
                TxtAICardNum.text = aiNum.ToString();
                MonoAICards.SetCard(aiCards, CardMgr.IsCardShowCompareResult);
                MonoAICards.RefreshCard();

                GoBg.SetActive(true);
            }

            // ----------- 统一控制下一回合按钮显隐 -----------
            bool gameEnd =
                GameSessionMgr.Instance.RoundTimes + 1 >= 10
                || !GameSessionMgr.Instance.PlayerEnough
                || !GameSessionMgr.Instance.BossEnough;
            BtnNextRound.gameObject.SetActive(!gameEnd);
        }

        public void Hide()
        {
            GoBg.SetActive(false);
            TxtAIName.text         = string.Empty;
            TxtAIResult.text       = string.Empty;
            TxtAICardNum.text      = string.Empty;
            MonoAICards.ClearCard();
            MonoAICards.RefreshCard();

            TxtPlayerName.text     = string.Empty;
            TxtPlayerResult.text   = string.Empty;
            TxtPlayerCardNum.text  = string.Empty;
            MonoPlayerCards.ClearCard();
            MonoPlayerCards.RefreshCard();
        }

        public void Init()
        {
            Hide();
            BtnNextRound.onClick.RemoveAllListeners();
            BtnNextRound.onClick.AddListener(OnClickNextRound);
            BtnGiveUp.onClick.RemoveAllListeners();
            BtnGiveUp.onClick.AddListener(OnClickEnd);
        }

        private void OnClickNextRound()
        {
            Hide();
            NotifyMgr.SendEvent(NotifyDefine.GAME_NEXT_ROUND);
        }

        private void OnClickEnd()
        {
            Hide();
            NotifyMgr.SendEvent(NotifyDefine.GAME_END, _hasEnd);
        }
    }
}
