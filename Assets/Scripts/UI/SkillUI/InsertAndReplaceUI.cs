using System.Collections.Generic;
using Base;
using Mgr;
using Obj;

namespace UI
{
    public class InsertAndReplaceUI : BaseViewMono
    {
    	//AUTO-GENERATE
    	private UnityEngine.UI.Button _btnCheckSelect;
    	private UnityEngine.UI.Button BtnCheckSelect 
    			=> _btnCheckSelect ??= transform.Find("go_bg/go_btn_CheckSelect").GetComponent<UnityEngine.UI.Button>();

    	private UnityEngine.UI.Button _btnToInsert;
    	private UnityEngine.UI.Button BtnToInsert 
    			=> _btnToInsert ??= transform.Find("go_bg/Switch/btn_ToInsert").GetComponent<UnityEngine.UI.Button>();

    	private UnityEngine.UI.Button _btnToReplace;
    	private UnityEngine.UI.Button BtnToReplace 
    			=> _btnToReplace ??= transform.Find("go_bg/Switch/btn_ToReplace").GetComponent<UnityEngine.UI.Button>();

    	private UI.CardHeap _monoCollectedCardHeap;
    	private UI.CardHeap MonoCollectedCardHeap 
    			=> _monoCollectedCardHeap ??= transform.Find("go_bg/go_mono_CollectedCardHeap").GetComponent<UI.CardHeap>();

    	private UI.CardHeap _monoHandCardHeap;
    	private UI.CardHeap MonoHandCardHeap 
    			=> _monoHandCardHeap ??= transform.Find("go_bg/go_mono_HandCardHeap").GetComponent<UI.CardHeap>();

    	private UI.CardHeap _monoTotalCardHeap;
    	private UI.CardHeap MonoTotalCardHeap 
    			=> _monoTotalCardHeap ??= transform.Find("go_bg/go_mono_TotalCardHeap").GetComponent<UI.CardHeap>();

    	private UnityEngine.GameObject _goBg;
    	private UnityEngine.GameObject GoBg 
    			=> _goBg ??= transform.Find("go_bg").gameObject;

    	private UnityEngine.GameObject _goCheckSelect;
    	private UnityEngine.GameObject GoCheckSelect 
    			=> _goCheckSelect ??= transform.Find("go_bg/go_btn_CheckSelect").gameObject;

    	private UnityEngine.GameObject _goCollectedCardHeap;
    	private UnityEngine.GameObject GoCollectedCardHeap 
    			=> _goCollectedCardHeap ??= transform.Find("go_bg/go_mono_CollectedCardHeap").gameObject;

    	private UnityEngine.GameObject _goHandCardHeap;
    	private UnityEngine.GameObject GoHandCardHeap 
    			=> _goHandCardHeap ??= transform.Find("go_bg/go_mono_HandCardHeap").gameObject;

    	private UnityEngine.GameObject _goTotalCardHeap;
    	private UnityEngine.GameObject GoTotalCardHeap 
    			=> _goTotalCardHeap ??= transform.Find("go_bg/go_mono_TotalCardHeap").gameObject;

    	//AUTO-GENERATE-END
        public List<CardValue> CollectedCard { get; set; }
        public CardItem collectCardTemplate;
        private bool _isShowInsert;
        private bool _isShowReplace;
        private int _replaceCount;
        private List<List<CardObj>> _termSelectCard;// [0][x] 手牌，[1][x] 牌堆的牌
        private List<int> selectedTopIndexes = new();
        private List<int> selectedOwnIndexes = new();
        public void Init()
        {
            GoBg.SetActive(false);
            _isShowInsert = false;
            _isShowReplace = false;
            selectedTopIndexes.Clear();
            selectedOwnIndexes.Clear();
            MonoCollectedCardHeap.SetCard(null, CardMgr.IsCardShowSelectCard);
            MonoHandCardHeap.SetCard(null, CardMgr.IsCardShowPlayerCardList);
            MonoTotalCardHeap.SetCard(null, CardMgr.IsCardShowSelectCard);
            BtnCheckSelect.onClick.RemoveAllListeners();
            BtnCheckSelect.onClick.AddListener(OnClickCheckReplace);
        }
        public void ShowReplace(List<CardObj> collectedCard, List<CardObj> curCards, int replaceCount)
        {
            _replaceCount = replaceCount;
            selectedTopIndexes.Clear();
            selectedOwnIndexes.Clear();
            MonoHandCardHeap.SetCard(curCards, CardMgr.IsCardShowSelectCard, OnCurCardClick);
            MonoCollectedCardHeap.SetCard(collectedCard, CardMgr.IsCardShowSelectCard, OnCollectedCardClick);
            GoBg.SetActive(true);
        }
        public void Hide()
        {
            if (_isShowReplace)
            {
                _isShowReplace = false;
                MonoHandCardHeap.ClearCard();
                MonoHandCardHeap.RefreshCard();
            }
            if (_isShowInsert)
            {
                _isShowInsert = false;
                MonoTotalCardHeap.ClearCard();
                MonoTotalCardHeap.RefreshCard();
            }
            MonoCollectedCardHeap.ClearCard();
            MonoCollectedCardHeap.RefreshCard();
            GoBg.SetActive(false);
            selectedTopIndexes.Clear();
            selectedOwnIndexes.Clear();
        }
        private void OnCurCardClick(int idx, CardItem card)
        {
            if (selectedTopIndexes.Contains(idx))
            {
                selectedTopIndexes.Remove(idx);
                card.CancelSelect();
            }
            else
            {
                if (selectedTopIndexes.Count < _replaceCount)
                {
                    selectedTopIndexes.Add(idx);
                    card.Selected();
                }
            }
        }
        private void OnCollectedCardClick(int idx, CardItem card)
        {
            if (selectedOwnIndexes.Contains(idx))
            {
                selectedOwnIndexes.Remove(idx);
                card.CancelSelect();
            }
            else
            {
                if (selectedOwnIndexes.Count < _replaceCount)
                {
                    selectedOwnIndexes.Add(idx);
                    card.Selected();
                }
            }
        }
        private void OnClickCheckReplace()
        {
            // if (selectedTopIndexes.Count == selectedOwnIndexes.Count && selectedTopIndexes.Count > 0)
            // {
            //     _termSelectCard = new List<List<CardObj>>
            //     {
            //         new(),
            //         new()
            //     };
            //     foreach (var idx in selectedTopIndexes)
            //         _termSelectCard[0].Add(MonoCurCardHeap.CardList[idx]);
            //     foreach (var idx in selectedOwnIndexes)
            //         _termSelectCard[1].Add(MonoCollectedCardHeap.CardList[idx]);
            //     NotifyMgr.SendEvent(NotifyDefine.REPLACE_CARD, PlayerType.Player, _termSelectCard);
            // }
            // else
            // {
            //     NotifyMgr.SendEvent(NotifyDefine.NOTICE, "请选择相同数量的牌");
            // }
        }
    }
}
