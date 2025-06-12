using System.Collections.Generic;
using System.Linq;
using AttachMachine;
using Base;
using Mgr;
using Obj;
using Unity.Mathematics;
using UnityEditor;
using XYZFrameWork;
namespace UI
{
	public class InsertAndReplaceUI : BaseViewMono
	{
		//AUTO-GENERATE
		private UnityEngine.UI.Button _btnCheckSelect;
		private UnityEngine.UI.Button BtnCheckSelect 
				=> _btnCheckSelect ??= transform.Find("go_bg/go_btn_CheckSelect").GetComponent<UnityEngine.UI.Button>();

		private UnityEngine.UI.Button _btnClose;
		private UnityEngine.UI.Button BtnClose 
				=> _btnClose ??= transform.Find("go_bg/btn_Close").GetComponent<UnityEngine.UI.Button>();

		private UnityEngine.UI.Button _btnUndo;
		private UnityEngine.UI.Button BtnUndo 
				=> _btnUndo ??= transform.Find("go_bg/go_btn_Undo").GetComponent<UnityEngine.UI.Button>();

		private UI.CardItem _monoCardItem;
		private UI.CardItem MonoCardItem 
				=> _monoCardItem ??= transform.Find("go_bg/mono_CardItem").GetComponent<UI.CardItem>();

		private UI.CardZone _monoCollectedCardHeap;
		private UI.CardZone MonoCollectedCardHeap 
				=> _monoCollectedCardHeap ??= transform.Find("go_bg/go_mono_CollectedCardHeap").GetComponent<UI.CardZone>();

		private UI.CardZone _monoHandCardHeap;
		private UI.CardZone MonoHandCardHeap 
				=> _monoHandCardHeap ??= transform.Find("go_bg/go_mono_HandCardHeap").GetComponent<UI.CardZone>();

		private UI.CardZone _monoTotalCardHeap;
		private UI.CardZone MonoTotalCardHeap 
				=> _monoTotalCardHeap ??= transform.Find("go_bg/go_mono_TotalCardHeap").GetComponent<UI.CardZone>();

		private UI.DraggableCard _monoDrappableCardItem;
		private UI.DraggableCard MonoDrappableCardItem 
				=> _monoDrappableCardItem ??= transform.Find("go_bg/mono_DrappableCardItem").GetComponent<UI.DraggableCard>();

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

		private UnityEngine.GameObject _goUndo;
		private UnityEngine.GameObject GoUndo 
				=> _goUndo ??= transform.Find("go_bg/go_btn_Undo").gameObject;

		private TMPro.TextMeshProUGUI _txtOperateCount;
		private TMPro.TextMeshProUGUI TxtOperateCount 
				=> _txtOperateCount ??= transform.Find("go_bg/txt_OperateCount").GetComponent<TMPro.TextMeshProUGUI>();

		//AUTO-GENERATE-END
		private          bool          _isShowInsert;
		private          bool          _isShowReplace;
		private          int           _operateCount;
		private readonly List<CardObj> _selectedTopIndexes = new();
		private readonly List<CardObj> _selectedOwnIndexes = new();
		private readonly List<InsertMoveData> _operationList = new();
		
		private List<CardObj> _topList;
		private List<CardObj> _skillList;
		
		public void Init()
		{
			GoBg.SetActive(false);
			_isShowInsert  = false;
			_isShowReplace = false;
			_selectedTopIndexes.Clear();
			_selectedOwnIndexes.Clear();
			_operationList.Clear();
			MonoHandCardHeap.SetCard(null, CardMgr.IsCardShowSelectCard);
			MonoTotalCardHeap.SetCard(null, CardMgr.IsCardShowPlayerCardList);
			MonoCollectedCardHeap.SetCard(null, CardMgr.IsCardShowPlayerCardList);
			
			BtnCheckSelect.onClick.RemoveAllListeners();
			BtnCheckSelect.onClick.AddListener(OnCheckClick);
			
			BtnClose.onClick.RemoveAllListeners();
			BtnClose.onClick.AddListener(OnCloseClick);
			
			NotifyMgr.UnRegisterNotify(NotifyDefine.MOVE_CARD, OnMoveCard);
			NotifyMgr.RegisterNotify(NotifyDefine.MOVE_CARD, OnMoveCard);
		}
		// 替换和插入无法同时存在。
		public void Show(List<CardObj> skillCards, List<CardObj> topCards, bool isInsert, int operateCount)
		{
			_operateCount = operateCount;
			_selectedTopIndexes.Clear();
			_selectedOwnIndexes.Clear();
			_topList   = topCards;
			_skillList = skillCards;
			
			if (isInsert)
			{
				RefreshOperateCount();
				MonoCollectedCardHeap.cardItemPrefab = MonoDrappableCardItem;
				MonoTotalCardHeap.cardItemPrefab     = MonoDrappableCardItem;
				
				_isShowInsert  = true;
				_isShowReplace = false;
				GoHandCardHeap.SetActive(false);
				MonoTotalCardHeap.SetCard(topCards, CardMgr.IsCardShowSelectCard, OnCurCardClick);
				GoTotalCardHeap.SetActive(true);
				GoUndo.SetActive(true);
				BtnUndo.onClick.RemoveAllListeners();
				BtnUndo.onClick.AddListener(OnUndoClick);
			}
			else
			{
				// 替换
				MonoCollectedCardHeap.cardItemPrefab = MonoCardItem;
				MonoTotalCardHeap.cardItemPrefab     = MonoCardItem;
				_isShowReplace                       = true;
				_isShowInsert                        = false;
				GoTotalCardHeap.SetActive(false);
				MonoHandCardHeap.SetCard(topCards, CardMgr.IsCardShowSelectCard, OnCurCardClick);
				GoHandCardHeap.SetActive(true);
				GoUndo.SetActive(false);
			}
			MonoCollectedCardHeap.SetCard(skillCards, CardMgr.IsCardShowSelectCard, OnCollectedCardClick);
			GoBg.SetActive(true);
		}
		private void RefreshOperateCount()
		{
			if (_isShowReplace)
			{
				TxtOperateCount.text = $"可替换牌数：{math.max(_selectedTopIndexes.Count, _selectedOwnIndexes.Count)}/{_operateCount}";
			}else if (_isShowInsert)
			{
				TxtOperateCount.text = $"可操作次数：{_operationList.Count}/{_operateCount}";
			}
			else
			{
				TxtOperateCount.text = "";
			}
		}
		public void Hide()
		{
			MonoCollectedCardHeap.ClearCard();
			MonoCollectedCardHeap.RefreshCard();
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
			_selectedTopIndexes.Clear();
			_selectedOwnIndexes.Clear();
			GoBg.SetActive(false);
		}
		private void OnCurCardClick(int idx, BaseCardItem card)
		{
			if (_selectedTopIndexes.Contains(card.Value))
			{
				_selectedTopIndexes.Remove(card.Value);
				card.CancelSelect();
			}
			else
			{
				if (_selectedTopIndexes.Count < _operateCount)
				{
					_selectedTopIndexes.Add(card.Value);
					card.Selected();
				}
			}
		}
		private void OnCollectedCardClick(int idx, BaseCardItem card)
		{
			RefreshOperateCount();
			if (_selectedOwnIndexes.Contains(card.Value))
			{
				_selectedOwnIndexes.Remove(card.Value);
				card.CancelSelect();
			}
			else
			{
				if (_selectedOwnIndexes.Count < _operateCount)
				{
					_selectedOwnIndexes.Add(card.Value);
					card.Selected();
				}
			}
		}
		private void OnMoveCard(NotifyMsg msg)
		{
			if (msg.Param is CustomParam param)
			{
				var data = param.Value as InsertMoveData;
				if (data == null) return;
				if (_operationList.Count< _operateCount)
				{
					AddMoveOperation(data);
					RefreshOperateCount();
				}else
				{
					Undo(data);
					NotifyMgr.SendEvent(NotifyDefine.NOTICE, "操作次数已满，已撤回");
				}
			}
		}
		private void AddMoveOperation(InsertMoveData data)
		{
			var index = _operationList.FindIndex(item => item.TargetCard == data.TargetCard);
			if (index == -1)
			{
				_operationList.Add(data);
			}
			else
			{
				var moveData = _operationList[index];
				moveData.ToZone = data.ToZone;
				moveData.ToIdx  = data.ToIdx;
			}	
		}
		private void OnUndoClick()
		{
			if (_operationList.Count > 0)
			{
				var data = _operationList[^1];
				_operationList.RemoveAt(_operationList.Count - 1);
				
				Undo(data);
			}
			RefreshOperateCount();
		}
		private static void Undo(InsertMoveData data)
		{
			data.ToZone .CardList.Remove(data.TargetCard);
			data.FromZone.AddCard(data.TargetCard, data.FromIdx);
				
				
			data.FromZone.RefreshCard();
			data.ToZone.RefreshCard();
		}
		private void OnCheckClick()
		{
			if (_isShowReplace)
			{
				if (_selectedTopIndexes.Count == _selectedOwnIndexes.Count && _selectedTopIndexes.Count > 0)
				{
					NotifyMgr.SendEvent(NotifyDefine.REPLACE_CARD, new ReplaceCardData()
					{
						SkillCard  = _selectedOwnIndexes,
						TargetList = _selectedTopIndexes,
						IsAI       = false
					});
				}
				else
				{
					NotifyMgr.SendEvent(NotifyDefine.NOTICE, "请选择相同数量的牌");
				}
			}
			else if (_isShowInsert)
			{
				NotifyMgr.SendEvent(NotifyDefine.CARD_STEAL_INSERT, new InsertCardData()
				{
					IsAI       = false,
					ToTotalList = _operationList.FindAll(item => item.ToZone == MonoTotalCardHeap).ConvertAll(item => item.TargetCard),
					ToCollectList = _operationList.FindAll(item => item.ToZone == MonoCollectedCardHeap).ConvertAll(item => item.TargetCard)
				});
			}
			Hide();
		}

		private void OnCloseClick()
		{
			if (_isShowReplace)
			{
				NotifyMgr.SendEvent(NotifyDefine.CLOSE_PANEL, SkillUIState.StateIDStr);
			}
			else if (_isShowInsert)
			{
				NotifyMgr.SendEvent(NotifyDefine.CLOSE_PANEL, ShuffleUIState.StateIDStr);
			}
			Hide();
		}
	}
}
