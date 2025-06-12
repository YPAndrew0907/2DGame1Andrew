using System.Collections.Generic;
using AttachMachine;
using Base;
using Mgr;
namespace UI
{
    public class SkillUpgradeUI : BaseViewMono
    {
    	//AUTO-GENERATE
    	private UnityEngine.UI.Button _btnBackHome;
    	private UnityEngine.UI.Button BtnBackHome 
    			=> _btnBackHome ??= transform.Find("go_Bg/btn_BackHome").GetComponent<UnityEngine.UI.Button>();

    	private UnityEngine.GameObject _goBg;
    	private UnityEngine.GameObject GoBg 
    			=> _goBg ??= transform.Find("go_Bg").gameObject;

    	private UnityEngine.GameObject _goSkillListItem;
    	private UnityEngine.GameObject GoSkillListItem 
    			=> _goSkillListItem ??= transform.Find("go_Bg/scroll_SkillList/Viewport/go_mono_SkillListItem").gameObject;

    	private UnityEngine.UI.ScrollRect _scrollSkillList;
    	private UnityEngine.UI.ScrollRect ScrollSkillList 
    			=> _scrollSkillList ??= transform.Find("go_Bg/scroll_SkillList").GetComponent<UnityEngine.UI.ScrollRect>();

    	private UI.SkillListItem _monoSkillListItem;
    	private UI.SkillListItem MonoSkillListItem 
    			=> _monoSkillListItem ??= transform.Find("go_Bg/scroll_SkillList/Viewport/go_mono_SkillListItem").GetComponent<UI.SkillListItem>();

    	private TMPro.TextMeshProUGUI _txtSkillPoint;
    	private TMPro.TextMeshProUGUI TxtSkillPoint 
    			=> _txtSkillPoint ??= transform.Find("go_Bg/SkillPointBg/txt_SkillPoint").GetComponent<TMPro.TextMeshProUGUI>();

    	//AUTO-GENERATE-END
	    
	    private readonly List<SkillListItem> _items = new();
	    public void Init()
	    {
		    GoBg.SetActive(false);
		    BtnBackHome.onClick.RemoveAllListeners();
		    BtnBackHome.onClick.AddListener(OnClickBackHome);
	    }
	    private void OnClickBackHome()
	    {
		    Hide();
		    XAttachMachine.SwitchState(SkillUpgradeUIState.StateIDStr, HomeUIState.StateIDStr);
	    }
	    public void Show(IEnumerable<SkillConfig> skillCfgs)
	    {
		    GoBg.SetActive(true);
		    RefreshSkillPoint();
		    // 清空旧Item
		    foreach (var item in _items)
			    if (item != null) Destroy(item.gameObject);
		    _items.Clear();
		    foreach (var cfg in skillCfgs)
		    {
			    var go = Instantiate(GoSkillListItem, ScrollSkillList.content.transform);
			    go.SetActive(true);
			    var item = go.GetComponent<SkillListItem>();
			    item.Init(cfg);
			    // 可选：给item传递一个回调，用于升级后刷新SkillPoint
			    item.OnUpgradeOrUnlock = RefreshSkillPoint;
			    _items.Add(item);
		    }
	    }
	    public void Hide()
	    {
		    GoBg.SetActive(false);
	    }
	    private void RefreshSkillPoint()
	    {
		    TxtSkillPoint.text = $"可用技能点：{SkillMgr.Instance.SkillPoint}";
		    foreach (var item in _items)
		    {
			    item.SetActive(SkillMgr.Instance.SkillPoint > 0);
		    }
	    }
    }
}
