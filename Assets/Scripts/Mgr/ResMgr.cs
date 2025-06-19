using System.Collections.Generic;
using Obj;
using UnityEngine;
using XYZFrameWork.Base;

namespace Mgr
{
    public class ResMgr: BaseSingle<ResMgr>
    {
        private readonly Dictionary<string, Sprite> _cardImgDic     = new();
        private readonly CardObj _backCard = new(CardValue.Back, CardSuit.Spade);
        
        // 获取牌的图片 根据花色与值
        public Sprite GetCardImg(CardObj card)
        {
            var cardId = card.Suit +"_"+ (int)card.Value;
            if (!_cardImgDic.ContainsKey(cardId))
            {
                var sprites    = Resources.LoadAll<Sprite>(card.Suit.ToString());
                foreach (var img in sprites)
                {
                    _cardImgDic.Add(img.name, img);
                }
            }

            return _cardImgDic[cardId];
        }

        public Sprite GetCardBackImg()
        {
            return GetCardImg(_backCard);
        }
    }
}