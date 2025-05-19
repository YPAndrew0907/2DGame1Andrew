using System.Collections;
using System.Collections.Generic;
using Base;
using Mgr;
using UnityEngine;
using UnityEngine.iOS;
using XYZFrameWork;

namespace UI
{
    public class AskCardUI : BaseViewMono
    {
    	//AUTO-GENERATE
    	private UnityEngine.UI.Button _btnAsk;
    	private UnityEngine.UI.Button BtnAsk 
    			=> _btnAsk ??= transform.Find("go_Bg/bg/go_Btns/btn_Ask").GetComponent<UnityEngine.UI.Button>();

    	private UnityEngine.UI.Button _btnNotAsk;
    	private UnityEngine.UI.Button BtnNotAsk 
    			=> _btnNotAsk ??= transform.Find("go_Bg/bg/go_Btns/btn_NotAsk").GetComponent<UnityEngine.UI.Button>();

    	private UnityEngine.GameObject _goBg;
    	private UnityEngine.GameObject GoBg 
    			=> _goBg ??= transform.Find("go_Bg").gameObject;

    	private UnityEngine.GameObject _goBtns;
    	private UnityEngine.GameObject GoBtns 
    			=> _goBtns ??= transform.Find("go_Bg/bg/go_Btns").gameObject;

    	private TMPro.TextMeshProUGUI _txtTitle;
    	private TMPro.TextMeshProUGUI TxtTitle 
    			=> _txtTitle ??= transform.Find("go_Bg/bg/txt_title").GetComponent<TMPro.TextMeshProUGUI>();

    	//AUTO-GENERATE-END
	    private       bool   _isAI;
	    private       bool   _aiAskCard;
	    private const string PlayerStr = "是否继续要牌";
	    private const string AIStr1     = "对手选择中....";
	    private const string AIStr2     = "对手不再要牌....";
	    private const string AIStr3     = "对手选择要牌....";
	    
        public void ShowUI(bool isAI, bool aiAskCard = true)
        {
	        _isAI = isAI;
	        GoBg.SetActive(true);
	        if (isAI)
	        {
		        _aiAskCard = aiAskCard;
		        GoBtns.SetActive(false);
		        TxtTitle.text = AIStr1;
		        StartCoroutine(AIAskEnd());
	        }
	        else
	        {
		        GoBtns.SetActive(true);
		        TxtTitle.text = PlayerStr;
	        }
        }
        public void Init()
        {
	        GoBg.SetActive(false);
	        BtnAsk.onClick.RemoveAllListeners();
	        BtnNotAsk.onClick.RemoveAllListeners();
	        BtnAsk.onClick.AddListener(OnClickAsk);
	        BtnNotAsk.onClick.AddListener(OnClickCancel);
        }
        private void OnClickAsk()
        {
	        NotifyMgr.Instance.SendEvent(NotifyDefine.ASK_CARD, AskCardParam(true));
        }
        private void OnClickCancel()
        {
	        NotifyMgr.Instance.SendEvent(NotifyDefine.ASK_CARD, AskCardParam(false));
        }
        
        public void Hide()
        {
	        GoBg.SetActive(false);
        }

        private List<bool> AskCardParam(bool isAsk)
        {
	        var list  = new List<bool>(){_isAI, isAsk};
	        return list;
        }

        private IEnumerator AIAskEnd()
        {
	        yield return new WaitForSeconds(2);
	        TxtTitle.text = _aiAskCard ? AIStr3 : AIStr2;
	        yield return new WaitForSeconds(0.5f);
	        NotifyMgr.Instance.SendEvent(NotifyDefine.ASK_CARD, AskCardParam(_aiAskCard));
        }
    }
}
