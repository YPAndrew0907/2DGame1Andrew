using Base;
using Mgr;
using XYZFrameWork;
namespace UI
{
    public class HomeUI : BaseViewMono
    {
    	//AUTO-GENERATE
    	private UnityEngine.UI.Button _btnLastLevel;
    	private UnityEngine.UI.Button BtnLastLevel 
    			=> _btnLastLevel ??= transform.Find("go_bg/btn_LastLevel").GetComponent<UnityEngine.UI.Button>();

    	private UnityEngine.UI.Button _btnNextLevel;
    	private UnityEngine.UI.Button BtnNextLevel 
    			=> _btnNextLevel ??= transform.Find("go_bg/btn_NextLevel").GetComponent<UnityEngine.UI.Button>();

    	private UnityEngine.UI.Button _btnQuit;
    	private UnityEngine.UI.Button BtnQuit 
    			=> _btnQuit ??= transform.Find("go_bg/btn_quit").GetComponent<UnityEngine.UI.Button>();

    	private UnityEngine.UI.Button _btnStart;
    	private UnityEngine.UI.Button BtnStart 
    			=> _btnStart ??= transform.Find("go_bg/btn_Start").GetComponent<UnityEngine.UI.Button>();

    	private UnityEngine.GameObject _goBg;
    	private UnityEngine.GameObject GoBg 
    			=> _goBg ??= transform.Find("go_bg").gameObject;

    	private TMPro.TextMeshProUGUI _txtLevel;
    	private TMPro.TextMeshProUGUI TxtLevel 
    			=> _txtLevel ??= transform.Find("go_bg/txt_level").GetComponent<TMPro.TextMeshProUGUI>();

    	//AUTO-GENERATE-END
	    public void Init()
	    {
		    BtnQuit.onClick.AddListener(OnQuitClick);
		    BtnStart.onClick.AddListener(OnStartClick);
		    BtnLastLevel.onClick.AddListener(OnLastLevelClick);
		    BtnNextLevel.onClick.AddListener(OnNextLevelClick);
	    }
	    private void Refresh()
	    {
		    TxtLevel.text = "Level: __" + DataMgr.Instance.CurLevel;
	    } 
	    public void ShowUI()
	    {
		    Refresh();
		    GoBg.SetActive(true);
	    }
	    public void HideUI()
	    {
		    GoBg.SetActive(false);
	    }
		private void OnDestroy()
		{
			BtnQuit.onClick.RemoveListener(OnQuitClick);
			BtnStart.onClick.RemoveListener(OnStartClick);
		}
		private void OnStartClick()
		{
			if (DataMgr.Instance.PlayerEnough)
			{
				NotifyMgr.SendEvent(NotifyDefine.GAME_READY, DataMgr.Instance.CurLevel);
				GoBg.SetActive(false);
			}
			else
			{
				NotifyMgr.SendEvent(NotifyDefine.NOTICE,"筹码不够，无法开始游戏");
			}
		}
		private void OnQuitClick()
		{
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#else 
			Application.Quit();
			#endif
		}

		private void OnLastLevelClick()
		{
			DataMgr.Instance.LastLevel();
			Refresh();
		}

		private void OnNextLevelClick()
		{
			DataMgr.Instance.NextLevel();
			Refresh();
		}
    }
}
