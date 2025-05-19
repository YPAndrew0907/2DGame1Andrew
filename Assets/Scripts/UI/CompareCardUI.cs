using System;
using System.Collections.Generic;
using System.Linq;
using Base;
using Cfg;
using Mgr;
using Obj;
using UnityEngine;
namespace UI
{
    public class CompareCardUI : BaseViewMono
    {
    	//AUTO-GENERATE
    	private UnityEngine.UI.Button _btnBackHome;
    	private UnityEngine.UI.Button BtnBackHome 
    			=> _btnBackHome ??= transform.Find("go_Bg/bg/btn_BackHome").GetComponent<UnityEngine.UI.Button>();

    	private UnityEngine.UI.Button _btnNextRound;
    	private UnityEngine.UI.Button BtnNextRound 
    			=> _btnNextRound ??= transform.Find("go_Bg/bg/btn_NextRound").GetComponent<UnityEngine.UI.Button>();

    	private UI.CardHeap _monoAICards;
    	private UI.CardHeap MonoAICards 
    			=> _monoAICards ??= transform.Find("go_Bg/bg/AIScore/mono_AICards").GetComponent<UI.CardHeap>();

    	private UI.CardHeap _monoPlayerCards;
    	private UI.CardHeap MonoPlayerCards 
    			=> _monoPlayerCards ??= transform.Find("go_Bg/bg/PlayerScore/mono_PlayerCards").GetComponent<UI.CardHeap>();

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

    	//AUTO-GENERATE-END
	    private const string WinStr = "<color=green>Win</color>";
	    private const string LossStr = "<color=red>Loss</color>";
	    private const string OutStr = "<color=red>Out</color>";
	    public void Show(List<KeyValuePair<string, List<CardObj>>> data)
	    {
		    if (data.Count < 2)
		    {
			    Debug.LogError(LogTxt.PARAM_ERROR);
			    return;
		    }
		    var playerNum = data[0].Value?.Sum(item => (int)item.Value + 1) ?? 0;
		    var aiNum = data[1].Value?.Sum(item => (int)item.Value + 1) ?? 0;
		    var playerWin = AIMgr.AIIsLoss(aiNum, playerNum);
		    DataMgr.Instance.PayChip(playerWin);
		    TxtPlayerName.text    = data[0].Key;
		    TxtPlayerResult.text  = data[0].Value != null ? playerWin ? WinStr : LossStr: OutStr;
		    TxtPlayerCardNum.text = playerNum.ToString();
		    
		    MonoPlayerCards.SetCard(data[0].Value,CardMgr.IsCardShowCompareResult);
		    MonoPlayerCards.RefreshCard();
		    TxtAIName.text    = data[1].Key;
		    TxtAIResult.text  = data[1].Value!= null? !playerWin ? WinStr : LossStr: OutStr;
		    TxtAICardNum.text = aiNum.ToString();
		    MonoAICards.SetCard(data[1].Value,CardMgr.IsCardShowCompareResult);
		    MonoAICards.RefreshCard();
		    GoBg.SetActive(true);
	    }
	    public void Hide()
        {
            GoBg.SetActive(false);
            TxtAIName.text     = String.Empty;
            TxtAIName.text = String.Empty;
            MonoAICards.ClearCard();
            MonoAICards.RefreshCard();
            
            TxtPlayerName.text = String.Empty;
            TxtPlayerCardNum.text = String.Empty;
            MonoPlayerCards.ClearCard();
            MonoPlayerCards.RefreshCard();
        }
	    
        public void Init()
        {
	        Hide();
	        BtnNextRound.onClick.RemoveAllListeners();
	        BtnNextRound.onClick.AddListener(OnClickNextRound);
	        BtnBackHome.onClick.RemoveAllListeners();
	        BtnBackHome.onClick.AddListener(OnClickBackHome);
        }
        // 填充玩家与ai对局数据。
        // 绑定按钮方法
        private void OnClickNextRound() 
        {
			NotifyMgr.Instance.SendEvent(NotifyDefine.NEXT_ROUND);    
        }

        private void OnClickBackHome()
        {
	        NotifyMgr.Instance.SendEvent(NotifyDefine.GAME_END_BACK_HOME);
        }
    }
}
