using XYZFrameWork.Base;
using TMPro;
using UnityEngine.UI;
using Base;
using Mgr;
using Obj;
using UnityEngine;
namespace UI
{
    public class CardItem : BaseViewMono
    {
		//AUTO-GENERATE
		private Image _imgCard;

		protected override void FindUI()
		{
			_imgCard = transform.Find("").GetComponent<UnityEngine.UI.Image>();
		}


		//AUTO-GENERATE-END
		
		public CardObj CardValue { get; private set;}

		private         Outline _outline;
		private Outline Outline
		{
			get
			{
				if (_outline == null)
				{
					_outline = GetComponent<Outline>();
				}
				return _outline;
			}
		}
		
		private readonly Color _hideColor = new Color(0, 0, 0, 0);
		private readonly Color _showColor = new Color(1, 1, 1, 1);
		
        public void RefreshCard(CardObj cardValue)
        {
	        CardValue = cardValue;
            var img  = ResMgr.Instance.GetCardImg(cardValue);
            if (cardValue.IsFaceUp)
            {
				_imgCard.sprite = img;
            }
            else
            {
	            _imgCard.sprite = ResMgr.Instance.GetCardBackImg();
            }
        }
        public void HideImg()
        {
	        _imgCard.color = _hideColor;
        }
        public void ShowImg()
        {
	        _imgCard.color = _showColor;
        }
        public void CancelSelect()
        {
	        Outline.effectColor = Color.black;
	        Outline.effectDistance = new Vector2(1, -1);
        }
        
        public void Selected()
        {
	        Outline.effectColor = new Color(1,1,0.26f,1);
	        Outline.effectDistance = new Vector2(3, -3);
        }
    }
}
