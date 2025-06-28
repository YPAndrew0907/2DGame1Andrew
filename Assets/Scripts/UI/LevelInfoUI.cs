using Base;
using Obj;

namespace UI
{
	public class LevelInfoUI : BaseViewMono
	{
		//AUTO-GENERATE
		private UnityEngine.GameObject _goBg;
		private UnityEngine.GameObject GoBg 
				=> _goBg ??= transform.Find("go_bg").gameObject;

		private UnityEngine.GameObject _goCurBet;
		private UnityEngine.GameObject GoCurBet 
				=> _goCurBet ??= transform.Find("go_bg/skillAndMoney/go_CurBet").gameObject;

		private UnityEngine.GameObject _goCurRound;
		private UnityEngine.GameObject GoCurRound 
				=> _goCurRound ??= transform.Find("go_bg/skillAndMoney/go_CurRound").gameObject;

		private UnityEngine.GameObject _goMoney;
		private UnityEngine.GameObject GoMoney 
				=> _goMoney ??= transform.Find("go_bg/skillAndMoney/go_money").gameObject;

		private UnityEngine.UI.Image _imgBg;
		private UnityEngine.UI.Image ImgBg 
				=> _imgBg ??= transform.Find("go_bg/img_Bg").GetComponent<UnityEngine.UI.Image>();

		private TMPro.TextMeshProUGUI _txtAIBetNum;
		private TMPro.TextMeshProUGUI TxtAIBetNum 
				=> _txtAIBetNum ??= transform.Find("go_bg/img_Bg/txt_AIBetNum").GetComponent<TMPro.TextMeshProUGUI>();

		private TMPro.TextMeshProUGUI _txtCurBet;
		private TMPro.TextMeshProUGUI TxtCurBet 
				=> _txtCurBet ??= transform.Find("go_bg/skillAndMoney/go_CurBet/txt_CurBet").GetComponent<TMPro.TextMeshProUGUI>();

		private TMPro.TextMeshProUGUI _txtCurRound;
		private TMPro.TextMeshProUGUI TxtCurRound 
				=> _txtCurRound ??= transform.Find("go_bg/skillAndMoney/go_CurRound/txt_CurRound").GetComponent<TMPro.TextMeshProUGUI>();

		private TMPro.TextMeshProUGUI _txtLevel;
		private TMPro.TextMeshProUGUI TxtLevel 
				=> _txtLevel ??= transform.Find("go_bg/txt_level").GetComponent<TMPro.TextMeshProUGUI>();

		private TMPro.TextMeshProUGUI _txtMoney;
		private TMPro.TextMeshProUGUI TxtMoney 
				=> _txtMoney ??= transform.Find("go_bg/skillAndMoney/go_money/txt_money").GetComponent<TMPro.TextMeshProUGUI>();

		//AUTO-GENERATE-END
		private void SetLevel(int level)
		{
			TxtLevel.text = $"赌局信息：Level - {level}";
		}
		public void SetMoney(int money)
		{
			TxtMoney.text = money < 0 ? "" : money.ToString();
		}
		public void SetCurBet(PlayerType playerType, int curBet)
		{
			if (playerType == PlayerType.AI)
				TxtAIBetNum.text = "对方押注：" + curBet;
			else if (playerType == PlayerType.Player)
				TxtCurBet.text = curBet.ToString();
		}
		public void SetCurRound(int roundTimes)
		{
			TxtCurRound.text = (roundTimes+1).ToString();
		}
		public void Init()
		{
			GoBg.SetActive(false);
			TxtCurBet.text = string.Empty;
			TxtMoney.text  = string.Empty;
			TxtLevel.text  = string.Empty;
			TxtCurRound.text = string.Empty;
		}
		public void Hide()
		{
			GoBg.SetActive(false);
		}
		public void ShowUI(int level,int money, int curBet)
		{
			SetLevel(level);
			SetMoney(money);
			SetCurBet(PlayerType.Player, curBet);
			SetCurBet(PlayerType.AI, curBet);
			GoBg.SetActive(true);
		}
    }
}
