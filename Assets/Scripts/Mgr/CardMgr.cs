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
        // public static    List<CardObj>          RememberCardList { get; set; }
        private          int                    _curCardIndex = 0;
        
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

        public void ResetCards()
        {
            Array.Sort(_cards);
            
            _curCardIndex = _cards.Length -1;
            foreach (var cardObj in _cards)
            {
                cardObj.IsRemembered = false;
                cardObj.IsFirstCard  = false;
                cardObj.IsCopy       = false;
                cardObj.Owner        = PlayerType.None;
                cardObj.TimeTicks    = DateTime.Now.Ticks;
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
        public void RememberCard(List<CardObj> list)
        {
            foreach (var rCardObj in list)
            {
                rCardObj.IsRemembered = true;
            }
        } 
        
        public static int TotalCardNum(IReadOnlyList<CardObj> list)
        {
            if (list is not {Count:>0})
            {
                return 0;
            }
            
            int i = 0;
            foreach (var card in list)
            {
                i += (int)card.Value + 1;
            }

            return i;
        }

        #region Filter

        public List<CardObj> FilterCopy(List<CardObj> list)
        {
            return list.FindAll(item => item.IsCopy);
        }

        public List<CardObj> FilterRemember(List<CardObj> list)
        {
            return list.FindAll(item => item.IsRemembered);
        }
        

        #endregion
        
        public static bool IsCardShowCompareResult(CardObj cardObj) => true;
        public static bool IsCardShowSelectCard(CardObj cardObj)    => true;
        public static bool IsCardShowSkillCardList(CardObj cardObj) => true;
        public static bool IsCardShowTotalCardList(CardObj cardObj) => cardObj.IsRemembered;
        public static bool IsCardShowPlayedCardList(CardObj cardObj) => true;

        public static bool IsCardShowPlayerCardList(CardObj cardObj) => true;
        public static bool IsCardShowAICardList(CardObj cardObj)     => cardObj.IsFirstCard || cardObj.IsRemembered;

        public List<CardObj> StealCard(List<int> indexList)
        {
            var result = new List<CardObj>();
            // 排序从大到小，防止后面的index被交换错位
            indexList.Sort();
            indexList.Reverse();

            foreach (var index in indexList)
            {
                // 如果索引超出当前可用范围，跳过
                if (index > _curCardIndex || index < 0)
                    continue;

                // 拿出要偷的那张牌
                var stealCard = _cards[index];
                result.Add(stealCard);

                // 和当前可用最后一张牌交换
                if (index != _curCardIndex)
                {
                    var temp = _cards[_curCardIndex];
                    _cards[_curCardIndex] = stealCard;
                    _cards[index]         = temp;
                }

                // 当前可用牌数减少
                _curCardIndex--;
            }

            // 最后 result 里面是从大到小顺序，按需求可以 reverse
            result.Reverse();
            return result;
        }
        
        public List<CardObj> CopyCard(List<int> indexList)
        {
            var result = new List<CardObj>();

            foreach (var index in indexList)
            {
                if (index < 0 || index >= _cards.Length)
                    continue;

                var copy = _cards[index].DeepCopy();
                copy.IsCopy = true;
                // 通常拷贝后最好刷新时间戳，否则有的逻辑会因“时间相等”出现混乱
                copy.TimeTicks = DateTime.Now.Ticks;
                result.Add(copy);
            }
            return result;
        }


    }
}