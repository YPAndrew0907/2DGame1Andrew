using System;
using System.Collections.Generic;
using System.Linq;
using Cfg;
using Obj;
using Unity.VisualScripting.Dependencies.NCalc;
using XYZFrameWork.Base;
using Random = UnityEngine.Random;

namespace Mgr
{
    public class CardMgr: BaseSingle<CardMgr>
    {
        public           IReadOnlyList<CardObj> Cards;                                    // 所有的牌
        private readonly CardObj[]              _cards = new CardObj[GameCfg.MaxCardNum]; // 所有的牌

        private int _curCardIndex = 0;
        
        public CardMgr()
        {
            for (int i = 0; i < GameCfg.MaxCardNum; i+=8)
            {
                for (int j = 0; j < 8; j+=2)
                {
                    _cards[i + j] = new CardObj((CardValue)(i/8), (CardSuit)(j/2));
                    _cards[i + j + 1] = new CardObj((CardValue)(i/8), (CardSuit)(j/2));
                }
            }

            Cards = _cards;
            _curCardIndex = Cards.Count - 1;
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

            Cards         = _cards;
            _curCardIndex = Cards.Count - 1;
        }
        
        // 只负责发牌。剩余牌不够不在这判断
        public CardObj Deal()
        {
            if (_curCardIndex == 0)
                return null;
            
            return Cards[--_curCardIndex];
        }

        public List<CardObj> GetCards(int i)
        {
            var result = new List<CardObj>();
            while (i>0)
            {
                i--;
                result.Add(Deal());
            }

            return result;
        }
        public static bool IsCardShowCompareResult(CardObj cardObj) => true;
        public static bool IsCardShowSkillCardList(CardObj cardObj) => true;
        public static bool IsCardShowTotalCardList(CardObj cardObj) => false;
        public static bool IsCardShowPlayedCardList(CardObj cardObj) => true;

        public static bool IsCardShowPlayerCardList(CardObj cardObj) => true;
        public static bool IsCardShowAICardList(CardObj cardObj) => cardObj.IsFaceUp;

    }
}