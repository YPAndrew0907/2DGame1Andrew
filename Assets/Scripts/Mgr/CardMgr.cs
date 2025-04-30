using System;
using System.Collections.Generic;
using System.Linq;
using Obj;
using Unity.VisualScripting.Dependencies.NCalc;
using XYZFrameWork.Base;
using Random = UnityEngine.Random;

namespace Mgr
{
    public class CardMgr: BaseSingle<CardMgr>
    {
        public readonly IReadOnlyList<CardObj> Cards = new CardObj[52]; // 所有的牌
        private readonly CardObj[]                 _cards = new CardObj[52]; // 所有的牌
        
        public CardObj[] AICards     { get; private set; }
        public CardObj[] PlayerCards { get; private set; }

        private int _curCardIndex = 0;
        
        public CardMgr()
        {
            for (int i = 0; i < 52; i+=4)
            {
                for (int j = 0; j < 4; j++)
                {
                    _cards[i + j] = new CardObj((CardValue)i, (CardSuit)j);
                }
            }
        }
        
        // 洗牌
        public void Shuffle()
        {
            for (int i = 0; i < _cards.Length; i++)
            {
                var temp = Cards[i];
                var randomIndex = Random.Range(i, _cards.Length);
                _cards[i]           = Cards[randomIndex];
                _cards[randomIndex] = temp;
            }
            for (int i = _cards.Length -1; i >= _cards.Length; i--)
            {
                var temp        = Cards[i];
                var randomIndex = Random.Range(0, i);
                _cards[i]           = Cards[randomIndex];
                _cards[randomIndex] = temp;
            }

            _curCardIndex = 0;
        }
        
        // 只负责发牌。剩余牌不够不在这判断
        public CardObj Deal()
        {
            if (_curCardIndex + 1 >= _cards.Length)
                return null;
            
            return Cards[_curCardIndex++];
        }
    }
}