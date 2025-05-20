 using System;

 namespace Obj
{
    public enum CardValue
    {
        A = 0, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, J, Q, K, Joker,Back
    }
    
    public static class ExCardValue{
        public static string ToShortStr(this CardValue self)
        {
            switch (self)
            {
                case CardValue.Two:   
                case CardValue.Three: 
                case CardValue.Four:  
                case CardValue.Five:  
                case CardValue.Six:   
                case CardValue.Seven: 
                case CardValue.Eight: 
                case CardValue.Nine:  
                case CardValue.Ten:
                    return $"{self + 1}";
                default:
                {
                    // A, J，Q，K,Joker,Back
                    return self.ToString();
                }
            }
        }
    } 

    public enum CardSuit
    {
        Spade,Heart,Club,Diamond
    }
    
    public class CardObj: IComparable<CardObj>
    {
        public readonly CardValue  Value;
        public readonly CardSuit   Suit;
        public bool IsFaceUp => IsFirstCard || IsRemembered || Owner == PlayerType.Player || Owner == PlayerType.Public;
        public bool IsFirstCard;  // 是否正面朝上
        public bool IsRemembered; // 是否被记着
        public bool IsShowRange;
        public PlayerType Owner;
        public long TimeTicks;

        public CardObj(CardValue value, CardSuit suit)
        {
            Value    = value;
            Suit     = suit;
            IsFirstCard = false;
            Owner    = PlayerType.None;
        }

        public override string ToString()
        {
            return $"【{Owner}】--花色：【{Suit}】--牌值：【{Value}】";
        }

        public int CompareTo(CardObj other)
        {
            if (ReferenceEquals(this, other))
                return 0;
            if (ReferenceEquals(null, other))
                return 1;
            var faceUpComparison = IsFaceUp.CompareTo(other.IsFaceUp);
            if (faceUpComparison != 0)
                return faceUpComparison;
            var valueComparison  = Value.CompareTo(other.Value);
            if (valueComparison != 0)
                return valueComparison;
            var suitComparison = Suit.CompareTo(other.Suit);
            if (suitComparison != 0)
                return suitComparison;
            return TimeTicks.CompareTo(other.TimeTicks);
        }
    }
}