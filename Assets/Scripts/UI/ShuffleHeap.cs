using System;
using System.Collections;
using XYZFrameWork.Base;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Base;
using DG.Tweening;
using Mgr;
using Obj;
using XYZFrameWork;

namespace UI
{
    public class ShuffleHeap : BaseViewMono
    {
		//AUTO-GENERATE
		private CardHeap _monoCards2;

		protected override void FindUI()
		{
			_monoCards2 = transform.Find("mono_Cards2").GetComponent<CardHeap>();
		}


		//AUTO-GENERATE-END
		
		public CardObj[]    remaindCard;

		public CanvasGroup shuffleAni;

		public void Start()
		{
			NotifyMgr.Instance.RegisterEvent(NotifyDefine.SHUFFLE_START, OnShuffle);
			NotifyMgr.Instance.RegisterEvent(NotifyDefine.SHUFFLE_END, OnShuffleEnd);
		}

		private void OnShuffle(NotifyMsg notifyMsg)
		{
			if (notifyMsg.Param is CustomParam param)
			{
				if (param.Values is CardObj[] { Length: > 0 } cards)
				{
					remaindCard = cards;
					CoroutineMgr.Instance.StartCoroutine(CorShuffleStart());
				}
			}
		}

		private void OnShuffleEnd(NotifyMsg notifyMsg)
		{
			CoroutineMgr.Instance.StartCoroutine(CorShuffleEnd());
		}

		IEnumerator CorShuffleStart()
		{
			shuffleAni.alpha = 0;
			shuffleAni.blocksRaycasts = true;
			shuffleAni.DOFade(1, 0.3f);
			yield return new WaitForSeconds(0.32f);
			_monoCards2.SetCard(remaindCard);
		}

		IEnumerator CorShuffleEnd()
		{
			yield return new WaitForSeconds(0.1f);
			shuffleAni.DOFade(0, 0.3f);
			yield return new WaitForSeconds(0.32f);
			shuffleAni.blocksRaycasts = false;
		}
    }
}
