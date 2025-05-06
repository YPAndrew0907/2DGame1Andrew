using XYZFrameWork.Base;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Base;
namespace UI
{
    public class DealCardAIUI : BaseViewMono
    {
		//AUTO-GENERATE
		private CardHeap _monoAICardHeap;

		protected override void FindUI()
		{
			_monoAICardHeap = transform.Find("mono_AICardHeap").GetComponent<CardHeap>();
		}


		//AUTO-GENERATE-END

		public void Init()
		{
			gameObject.SetActive(false);
		}
    }
}
