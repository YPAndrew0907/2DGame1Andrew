using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Base;
using DG.Tweening;
using Mgr;
using Obj;
namespace UI
{
    public class TotalCardHeapUI : BaseViewMono
    {
    	//AUTO-GENERATE
    	private UnityEngine.CanvasGroup _cgAniPanel;
    	private UnityEngine.CanvasGroup CgAniPanel 
    			=> _cgAniPanel ??= transform.Find("go_bg/cg_aniPanel").GetComponent<UnityEngine.CanvasGroup>();

    	private UI.CardHeap _monoCards2;
    	private UI.CardHeap MonoCards2 
    			=> _monoCards2 ??= transform.Find("go_bg/scroll_mono_Cards2").GetComponent<UI.CardHeap>();

    	private UnityEngine.GameObject _goBg;
    	private UnityEngine.GameObject GoBg 
    			=> _goBg ??= transform.Find("go_bg").gameObject;

    	private UnityEngine.UI.ScrollRect _scrollCards2;
    	private UnityEngine.UI.ScrollRect ScrollCards2 
    			=> _scrollCards2 ??= transform.Find("go_bg/scroll_mono_Cards2").GetComponent<UnityEngine.UI.ScrollRect>();

    	private TMPro.TextMeshProUGUI _txtTitle;
    	private TMPro.TextMeshProUGUI TxtTitle 
    			=> _txtTitle ??= transform.Find("go_bg/cg_aniPanel/txt_title").GetComponent<TMPro.TextMeshProUGUI>();

    	//AUTO-GENERATE-END
	    private List<CardObj> _remainCard;
	    public  CanvasGroup   shuffleAni;
		private void Update()
		{
			if (ScrollCards2!= null)
			{
				ScrollCards2.verticalNormalizedPosition = 0;
			}
		}
		public void Init()
		{
			GoBg.SetActive(false);
			MonoCards2.SetCard(null,_=> false);
		}
		
		public void Show()
		{
			GoBg.SetActive(true);
		}
		public void Hide()
		{
			GoBg.SetActive(false);
		}

		public void SetCard(List<CardObj> cardObjs)
		{
			_remainCard = cardObjs;
		}
		
		public IEnumerator CorShuffleStart()
		{
			shuffleAni.alpha = 0;
			shuffleAni.blocksRaycasts = true;
			shuffleAni.DOFade(1, 0.3f);
			yield return new WaitForSeconds(0.32f);
		}
		public IEnumerator CorShuffleEnd()
		{
			yield return new WaitForSeconds(0.1f);
			shuffleAni.DOFade(0, 0.3f);
			yield return new WaitForSeconds(0.32f);
			shuffleAni.blocksRaycasts = false;
		}
		// 需要外部调用的部分。
		public IEnumerator StartShuffle(PlayerType shuffleRole)
		{
			switch (shuffleRole)
			{
				case PlayerType.AI:   
					print("由对方洗牌");
					break;
				case PlayerType.Player:  
					print("由玩家洗牌");
					break;
				default: 
					print("洗牌人员初始化错误");
					break;
			}
			MonoCards2.SetCard(_remainCard, CardMgr.IsCardShowTotalCardList);
			yield break;
		}

		public void RefreshTotalCard(params CardObj[] cards)
		{
			foreach (var card in cards)
			{
				_remainCard.Remove(card);
			}
			MonoCards2.SetCard(_remainCard,CardMgr.IsCardShowTotalCardList);
			// 移除刚发的牌，然后刷新
			MonoCards2.RefreshCard();
		}
    }
}
