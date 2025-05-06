using XYZFrameWork.Base;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Base;
namespace UI
{
    public class DealCardPlayerUI : BaseViewMono
    {
		//AUTO-GENERATE
		private CardHeap _monoPlayerCardHeap;

		protected override void FindUI()
		{
			_monoPlayerCardHeap = transform.Find("mono_PlayerCardHeap").GetComponent<CardHeap>();
		}


		//AUTO-GENERATE-END

		public void Init()
		{
			gameObject.SetActive(false);
		}
    }
}
