using System;
using Base;
using Mgr;
using Unity.Mathematics;
namespace UI
{
	// 押注UI
	public class BetUI : BaseViewMono
	{
		//AUTO-GENERATE
		private UnityEngine.UI.Button _btnCheckBet;
		private UnityEngine.UI.Button BtnCheckBet 
				=> _btnCheckBet ??= transform.Find("go_Bg/BetPanel/btn_CheckBet").GetComponent<UnityEngine.UI.Button>();

		private UnityEngine.UI.Button _btnPlus;
		private UnityEngine.UI.Button BtnPlus 
				=> _btnPlus ??= transform.Find("go_Bg/BetPanel/btn_Plus").GetComponent<UnityEngine.UI.Button>();

		private UnityEngine.UI.Button _btnReduce;
		private UnityEngine.UI.Button BtnReduce 
				=> _btnReduce ??= transform.Find("go_Bg/BetPanel/btn_Reduce").GetComponent<UnityEngine.UI.Button>();

		private UnityEngine.GameObject _goBg;
		private UnityEngine.GameObject GoBg 
				=> _goBg ??= transform.Find("go_Bg").gameObject;

		private TMPro.TextMeshProUGUI _txtPlaceholder;
		private TMPro.TextMeshProUGUI TxtPlaceholder 
				=> _txtPlaceholder ??= transform.Find("go_Bg/BetPanel/input_ChipNum/Text Area/txt_Placeholder").GetComponent<TMPro.TextMeshProUGUI>();

		private TMPro.TextMeshProUGUI _txtPlus;
		private TMPro.TextMeshProUGUI TxtPlus 
				=> _txtPlus ??= transform.Find("go_Bg/BetPanel/btn_Plus/txt_Plus").GetComponent<TMPro.TextMeshProUGUI>();

		private TMPro.TextMeshProUGUI _txtReduce;
		private TMPro.TextMeshProUGUI TxtReduce 
				=> _txtReduce ??= transform.Find("go_Bg/BetPanel/btn_Reduce/txt_Reduce").GetComponent<TMPro.TextMeshProUGUI>();

		private TMPro.TMP_InputField _inputChipNum;
		private TMPro.TMP_InputField InputChipNum 
				=> _inputChipNum ??= transform.Find("go_Bg/BetPanel/input_ChipNum").GetComponent<TMPro.TMP_InputField>();

		//AUTO-GENERATE-END
		private int _curChipNum;
		private int _minChipNum;
		private int _maxChipNum;
		private int _intervalChip;
		private int _ownMaxChipNum;

		public void ShowBetUI(int originBet, int intervalChip, int minNum, int maxNum, int ownChip)
		{
			_intervalChip       = intervalChip;
			_minChipNum         = minNum;
			_maxChipNum         = maxNum;
			_ownMaxChipNum      = ownChip;
			_curChipNum         = math.min(originBet, ownChip);
			TxtPlus.text        = "+" + _intervalChip;
			TxtReduce.text      = "-" + _intervalChip;
			InputChipNum.text   = _curChipNum.ToString();
			TxtPlaceholder.text = $"{_minChipNum} ~ {Math.Min(_maxChipNum, _ownMaxChipNum)}";
			GoBg.SetActive(true);
		}

		public void Init()
		{
			BtnReduce.onClick.AddListener(OnReduceClick);
			BtnPlus.onClick.AddListener(OnPlusClick);
			BtnCheckBet.onClick.AddListener(OnCheckBet);
			// 绑定输入框变化事件
			InputChipNum.onValueChanged.AddListener(OnChipInputChange);
			GoBg.SetActive(false);
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
			if (chipNum > Math.Min(_maxChipNum, _ownMaxChipNum))
				return;
			_curChipNum = chipNum;
			ChipNumChange();
		}
		private void OnCheckBet()
		{
			if (_curChipNum == 0)
			{
				NotifyMgr.SendEvent(NotifyDefine.NOTICE, "请选择合法的筹码数量！");
				return;
			}
			if (!GameSessionMgr.Instance.PlayerEnough)
			{
				return;
			}

			NotifyMgr.SendEvent(NotifyDefine.BET_CHIP, _curChipNum);
		}

		private void ChipNumChange()
		{
			InputChipNum.text = _curChipNum.ToString();
		}
		private void OnChipInputChange(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				_curChipNum = 0;  // 输入框为空时内部值为0
				// 不自动回填，保持空，让水印显示
				return;
			}

			int val;
			// 只允许正整数
			if (int.TryParse(str, out val))
			{
				// 限定输入范围
				val         = Math.Max(_minChipNum, Math.Min(val, Math.Min(_maxChipNum, _ownMaxChipNum)));
				_curChipNum = val;
				// 自动修正非法范围
				if (_curChipNum.ToString() != str)
					InputChipNum.text = _curChipNum.ToString();
			}
			else
			{
				// 非法输入恢复为上一次的合法值（如果之前为0，也恢复为空）
				if (_curChipNum > 0)
					InputChipNum.text = _curChipNum.ToString();
				else
					InputChipNum.text = "";
			}
		}

		public void Hide()
		{
			GoBg.SetActive(false);
		}
	}
}
