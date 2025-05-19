using System.Collections.Generic;
using Base;
using Mgr;
using Obj;

namespace UI
{
    public class PlayedCardUI : BaseViewMono
    {
    	//AUTO-GENERATE
    	private UnityEngine.CanvasGroup _cgAniPanel;
    	private UnityEngine.CanvasGroup CgAniPanel 
    			=> _cgAniPanel ??= transform.Find("go_bg/cg_aniPanel").GetComponent<UnityEngine.CanvasGroup>();

    	private UI.CardHeap _monoCards2;
    	private UI.CardHeap MonoCards2 
    			=> _monoCards2 ??= transform.Find("go_bg/mono_Cards2").GetComponent<UI.CardHeap>();

    	private UnityEngine.GameObject _goBg;
    	private UnityEngine.GameObject GoBg 
    			=> _goBg ??= transform.Find("go_bg").gameObject;

    	private TMPro.TextMeshProUGUI _txtTitle;
    	private TMPro.TextMeshProUGUI TxtTitle 
    			=> _txtTitle ??= transform.Find("go_bg/cg_aniPanel/txt_title").GetComponent<TMPro.TextMeshProUGUI>();

    	//AUTO-GENERATE-END
        public void Init()
        {
	        GoBg.SetActive(false);
        }
        public void Show()
        {
	        MonoCards2.SetCard(null, CardMgr.IsCardShowPlayedCardList);
	        GoBg.SetActive(true);
        }
        public void Hide()
        {
	        GoBg.SetActive(false);
        }

        public void AddCards(List<CardObj> list)
        {
	        MonoCards2.AddCard(list);
	        MonoCards2.RefreshCard();
        }
    }
}
