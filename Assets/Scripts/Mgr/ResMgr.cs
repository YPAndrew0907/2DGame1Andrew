using System.Collections.Generic;
using Obj;
using UnityEngine;
using XYZFrameWork.Base;

namespace Mgr
{
    public class ResMgr: BaseSingle<ResMgr>
    {
        Dictionary<int,string> _cardImgDic = new();
        
        private CardObj backCard = new CardObj(CardValue.Back,CardSuit.Spade);

        // 获取牌的图片 Path 根据花色与值
        public string GetCardName(CardObj card)
        {
            var cardId = GetCardId(card);
            if (!_cardImgDic.TryGetValue(cardId,out var cardStr))
            {
                cardStr = card.Suit + "_" + (int)card.Value;
                _cardImgDic.Add(cardId,cardStr);
            }
            return cardStr;
        }
        
        // 获取牌的图片 根据花色与值
        public Sprite GetCardImg(CardObj card)
        {
            var cardId = GetCardName(card);
            var img    = Resources.Load<Sprite>(cardId);
            return img;
        }

        public Sprite GetCardBackImg()
        {
            return GetCardImg(backCard);
        }

        // 获取牌的唯一ID，根据花色与值
        public int GetCardId(CardObj card)
        {
            if (card == null) return  (int)CardSuit.Club * 100 + (int)CardValue.Back;
            return (int)card.Suit * 100 + (int)card.Value;
        }
    }
}