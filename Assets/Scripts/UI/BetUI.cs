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

    	private TMPro.TextMeshProUGUI _txtChipNum;
    	private TMPro.TextMeshProUGUI TxtChipNum 
    			=> _txtChipNum ??= transform.Find("go_Bg/BetPanel/txt_ChipNum").GetComponent<TMPro.TextMeshProUGUI>();

    	private TMPro.TextMeshProUGUI _txtPlus;
    	private TMPro.TextMeshProUGUI TxtPlus 
    			=> _txtPlus ??= transform.Find("go_Bg/BetPanel/btn_Plus/txt_Plus").GetComponent<TMPro.TextMeshProUGUI>();

    	private TMPro.TextMeshProUGUI _txtReduce;
    	private TMPro.TextMeshProUGUI TxtReduce 
    			=> _txtReduce ??= transform.Find("go_Bg/BetPanel/btn_Reduce/txt_Reduce").GetComponent<TMPro.TextMeshProUGUI>();

    	//AUTO-GENERATE-END
		
		private int _curChipNum;
		private int _minChipNum;
		private int _maxChipNum;
		private int _intervalChip;
		private int _ownMaxChipNum;
		public void ShowBetUI()
		{
			_intervalChip    = DataMgr.Instance.TableLevel;
			_minChipNum      = DataMgr.Instance.CurMinBetChip;
			_maxChipNum      = DataMgr.Instance.CurMaxBetChip;
			_ownMaxChipNum   = DataMgr.Instance.Money;
			_curChipNum      = _minChipNum;
			TxtPlus.text    = "+" + _intervalChip;
			TxtReduce.text  = "-" + _intervalChip;
			TxtChipNum.text = _curChipNum.ToString();
			GoBg.SetActive(true);
		}
		public void Init()
        {
	        BtnReduce.onClick.AddListener(OnReduceClick);
	        BtnPlus.onClick.AddListener(OnPlusClick);
	        BtnCheckBet.onClick.AddListener(OnCheckBet);
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
	        if (chipNum > math.min(_maxChipNum,_ownMaxChipNum))
		        return;
	        _curChipNum = chipNum;
	        ChipNumChange();
        }
        private void OnCheckBet()
        {
	        if (DataMgr.Instance.Money< _curChipNum)
		        return;
	        
	        NotifyMgr.SendEvent(NotifyDefine.BET_CHIP,_curChipNum);
        }
        
        private void ChipNumChange()
        {
	        TxtChipNum.text = _curChipNum.ToString();
        }
        
        public void Hide()
        {
	        GoBg.SetActive(false);
        }
    }
}
