using System;
using System.Collections.Generic;
using System.Linq;
using Cfg;
using Obj;
using XYZFrameWork.Base;
using Random = UnityEngine.Random;

namespace Mgr
{
    public class CardMgr : BaseSingle<CardMgr>
    {
        private CardObj[] _cards = new CardObj[GameCfg.MaxCardNum * 2]; // 所有的牌
        public IReadOnlyList<CardObj> Cards; // 所有的牌
        public List<CardObj> CardsList ; 
        
        private int _curCardIndex = 0;
        private int _curMaxLength;

        public CardMgr()
        {
            _curMaxLength = GameCfg.MaxCardNum;
            for (int i = 0; i < _curMaxLength; i += 8)
            {
                for (int j = 0; j < 8; j += 2)
                {
                    _cards[i + j]     = new CardObj((CardValue)(i / 8), (CardSuit)(j / 2));
                    _cards[i + j + 1] = new CardObj((CardValue)(i / 8), (CardSuit)(j / 2));
                }
            }

            RefreshCardList();
        }

        public void ResetCards(bool removeCopyCard = false)
        {
            if (removeCopyCard)
            {
                int newLength = 0;
                for (int i = 0; i < _curMaxLength; i++)
                {
                    if (_cards[i] == null || _cards[i].IsCopy)
                        continue;

                    _cards[newLength++] = _cards[i];
                }

                // 清理冗余区域
                for (int i = newLength; i < _curMaxLength; i++)
                {
                    _cards[i] = null;
                }

                _curMaxLength = newLength;
            }

            Array.Sort(_cards, 0, _curMaxLength);

            _curCardIndex = _curMaxLength - 1;

            for (var index = 0; index < _curMaxLength; index++)
            {
                var cardObj = _cards[index];
                if (cardObj == null)
                    continue;

                cardObj.IsRemembered = false;
                cardObj.IsFirstCard  = false;
                cardObj.Owner        = PlayerType.None;
                cardObj.TimeTicks    = DateTime.Now.Ticks;
            }
            RefreshCardList();
        }


        // 洗牌
        public void Shuffle()
        {
            for (int i = 0; i < _curMaxLength; i++)
            {
                var temp        = _cards[i];
                var randomIndex = Random.Range(i, _curMaxLength);
                _cards[i]           = _cards[randomIndex];
                _cards[randomIndex] = temp;
            }

            for (int i = _curMaxLength - 1; i >= _curMaxLength; i--)
            {
                var temp        = _cards[i];
                var randomIndex = Random.Range(0, i);
                _cards[i]           = _cards[randomIndex];
                _cards[randomIndex] = temp;
            }
            
            RefreshCardList();
        }

        private void RefreshCardList()
        {
            Cards         = new Span<CardObj>(_cards, 0, _curMaxLength).ToArray();
            CardsList = new Span<CardObj>(_cards, 0, _curMaxLength).ToArray().ToList();
            
            _curCardIndex = _curMaxLength - 1;
        }

        // 只负责发牌。剩余牌不够不在这判断
        public CardObj Deal()
        {
            if (_curCardIndex == 0)
                return null;

            return Cards[--_curCardIndex];
        }

        // 获取多张牌
        public List<CardObj> GetCards(int i)
        {
            var result = new List<CardObj>();
            while (i > 0)
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
            if (list == null) return -1;
            if (list.Count == 0) return 0;

            int i = 0;
            foreach (var card in list)
            {
                i += (int)card.Value + 1;
            }

            return i;
        }

        public static bool IsCardShowCompareResult(CardObj cardObj)  => true;
        public static bool IsCardShowSelectCard(CardObj cardObj)     => true;
        public static bool IsCardShowSkillCardList(CardObj cardObj)  => true;
        public static bool IsCardShowTotalCardList(CardObj cardObj)  => cardObj.IsRemembered;
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
                if (index < 0 || index >= _curMaxLength)
                    continue;

                var copy = _cards[index].DeepCopy();
                copy.IsCopy = true;
                // 通常拷贝后最好刷新时间戳，否则有的逻辑会因“时间相等”出现混乱
                copy.TimeTicks = DateTime.Now.Ticks;
                result.Add(copy);
            }

            return result;
        }


        // 插入牌
        public void PushCard(CardObj[] toArray)
        {
            if (toArray == null || toArray.Length == 0)
                return;

            foreach (var card in toArray)
            {
                card.Owner = PlayerType.None;
                if (_curMaxLength >= _cards.Length)
                {
                    // 扩容
                    Array.Resize(ref _cards, _curMaxLength * 2);
                }

                _cards[_curMaxLength++] = card;
            }

            Cards         = _cards;
            _curCardIndex = _curMaxLength - 1;
        }

    }
}