using System;
using Obj;
using XYZFrameWork.Base;
using Random = UnityEngine.Random;

namespace Mgr
{
    public class CardMgr: BaseSingle<CardMgr>
    {
        public readonly Card[] Cards = new Card[52];
        
        public  Card[] AICards     { get; private set; }
        public  Card[] PlayerCards { get; private set; }

        private int _curCardIndex = 0;
        
        // 当前首先发牌的玩家
        public PlayerType  CurFirst { get;private set; }

        public CardMgr()
        {
            for (int i = 0; i < 52; i+=4)
            {
                for (int j = 0; j < 4; j++)
                {
                    Cards[i + j] = new Card((CardValue)i, (CardSuit)j);
                }
            }

            CurFirst = PlayerType.None;
        }
        
        
        // 洗牌
        public void Shuffle()
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                var temp = Cards[i];
                var randomIndex = Random.Range(i, Cards.Length);
                Cards[i] = Cards[randomIndex];
                Cards[randomIndex] = temp;
            }
            for (int i = Cards.Length -1; i >= Cards.Length; i--)
            {
                var temp        = Cards[i];
                var randomIndex = Random.Range(0, i);
                Cards[i]           = Cards[randomIndex];
                Cards[randomIndex] = temp;
            }

            _curCardIndex = 0;
            // CurFirst = Math.min ++;
            NotifyMgr.Instance.SendEvent(NotifyDefine.SHUFFLE);
        }
        
        // 发牌
        public void Deal()
        {
            
        }
        
    }

    public enum PlayerType
    {
        None,
        Player,
        AI,
    }
}