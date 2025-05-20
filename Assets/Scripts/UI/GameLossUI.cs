using System;
using Base;
using Mgr;
using Obj;
namespace UI
{
    public class GameLossUI : BaseViewMono
    {
    	//AUTO-GENERATE
    	private UnityEngine.UI.Button _btnBackHome;
    	private UnityEngine.UI.Button BtnBackHome 
    			=> _btnBackHome ??= transform.Find("go_bg/btn_BackHome").GetComponent<UnityEngine.UI.Button>();

    	private UnityEngine.UI.Button _btnReStart;
    	private UnityEngine.UI.Button BtnReStart 
    			=> _btnReStart ??= transform.Find("go_bg/btn_ReStart").GetComponent<UnityEngine.UI.Button>();

    	private UnityEngine.GameObject _goBg;
    	private UnityEngine.GameObject GoBg 
    			=> _goBg ??= transform.Find("go_bg").gameObject;

    	private TMPro.TextMeshProUGUI _txtInfo;
    	private TMPro.TextMeshProUGUI TxtInfo 
    			=> _txtInfo ??= transform.Find("go_bg/txt_Info").GetComponent<TMPro.TextMeshProUGUI>();

    	private TMPro.TextMeshProUGUI _txtTitle;
    	private TMPro.TextMeshProUGUI TxtTitle 
    			=> _txtTitle ??= transform.Find("go_bg/txt_Title").GetComponent<TMPro.TextMeshProUGUI>();

    	//AUTO-GENERATE-END

	    private const string LossStr     = "闯关失败";
	    private const string GiveUpStr     = "放弃闯关";
	    private const string LossStrEarn = "停下了闯关得脚步，但是获得筹码{0}";
	    private const string LossStrLoss = "停下了闯关得脚步，损失筹码{0}";
        public void Show(GameEndCode endCode, int moneyDelta)
        {
	        switch (endCode)
	        {
		        case GameEndCode.Lose:
					TxtTitle.text = LossStr;
			        break;
		        case GameEndCode.GiveUp:
			        TxtTitle.text = GiveUpStr;
			        break;
		        default:                 throw new ArgumentOutOfRangeException(nameof(endCode), endCode, null);
	        }
	        if (moneyDelta>0)
	        {
		        TxtInfo.text = string.Format(LossStrEarn, moneyDelta);
	        }
	        else
	        {
		        TxtInfo.text = string.Format(LossStrLoss, moneyDelta);
	        }
            GoBg.SetActive(true);
        }
        public void Hide()
        {
            GoBg.SetActive(false);
        }
        public void Init()
        {
            Hide();
            BtnBackHome.onClick.RemoveAllListeners();
            BtnBackHome.onClick.AddListener(OnBackHome);
        }

        private void OnBackHome()
        {
	        NotifyMgr.SendEvent(NotifyDefine.CLOSE_GAME_END_UI);
        }
    }
}
