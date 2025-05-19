using Base;
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
        public void Show(GameEndCode endCode)
        {
            GoBg.SetActive(true);
        }
        public void Hide()
        {
            GoBg.SetActive(false);
        }
        public void Init()
        {
            Hide();
        }
    }
}
