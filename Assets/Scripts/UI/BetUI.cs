using System;
using XYZFrameWork.Base;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Base;
using Mgr;
using Unity.Mathematics;

namespace UI
{
    // 押注UI
    public class BetUI : BaseViewMono
    {
		//AUTO-GENERATE
		private Button _btnPlus;
		private Button _btnReduce;
		private TextMeshProUGUI _txtChipNum;
		private TextMeshProUGUI _txtPlus;
		private TextMeshProUGUI _txtReduce;

		protected override void FindUI()
		{
			   _btnPlus = transform.Find("BetPanel/btn_Plus").GetComponent<UnityEngine.UI.Button>();
			 _btnReduce = transform.Find("BetPanel/btn_Reduce").GetComponent<UnityEngine.UI.Button>();
			_txtChipNum = transform.Find("BetPanel/txt_ChipNum").GetComponent<TMPro.TextMeshProUGUI>();
			   _txtPlus = transform.Find("BetPanel/btn_Plus/txt_Plus").GetComponent<TMPro.TextMeshProUGUI>();
			 _txtReduce = transform.Find("BetPanel/btn_Reduce/txt_Reduce").GetComponent<TMPro.TextMeshProUGUI>();
		}


		//AUTO-GENERATE-END
		
		private int _curChipNum;
		private int _minChipNum;
		private int _maxChipNum;
		private int _intervalChip;
		private int _ownMaxChipNum;

		public void ShowBetUI()
		{
			_intervalChip    = DataMgr.Instance.TableLevel;
			_minChipNum      = DataMgr.Instance.CurMinBetMoney;
			_maxChipNum      = DataMgr.Instance.CurMaxBetMoney;
			_ownMaxChipNum   = DataMgr.Instance.Money;
			_curChipNum      = _minChipNum;
			_txtPlus.text    = "+" + _intervalChip;
			_txtReduce.text  = "-" + _intervalChip;
			_txtChipNum.text = _curChipNum.ToString();
			gameObject.SetActive(true);
		}

		public void InitBetUI()
        {
	        _btnReduce.onClick.AddListener(OnReduceClick);
	        _btnPlus.onClick.AddListener(OnPlusClick);
	        gameObject.SetActive(false);
        }

        private void OnReduceClick()
        {
	        var chipNum = _curChipNum - _intervalChip;
	        if (chipNum < _minChipNum)
				return;
	        _curChipNum = chipNum;
	        ChipNumChange();
        }

        private void OnPlusClick()
        {
	        var chipNum = _curChipNum + _intervalChip;
	        if (chipNum > math.min(_maxChipNum,_ownMaxChipNum))
		        return;
	        _curChipNum = chipNum;
	        ChipNumChange();
        }

        private void ChipNumChange()
        {
	        _txtChipNum.text = _curChipNum.ToString();
        }

        public void OnDestroy()
        {
	        _btnPlus.onClick.RemoveListener(OnPlusClick);
	        _btnReduce.onClick.RemoveListener(OnReduceClick);
        }
    }
}
