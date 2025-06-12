 using System;

 namespace Obj
{
    public enum CardValue
    {
        A = 0, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, J, Q, K, Joker,Back
    }

    public enum CardSuit
    {
        Spade = 0, Heart, Club, Diamond
    }

    public class CardObj: IComparable<CardObj>,IEquatable<CardObj>
    {
        public readonly CardValue  Value;
        public readonly CardSuit   Suit;
        public bool IsFaceUp => IsFirstCard || IsRemembered || Owner == PlayerType.Player || Owner == PlayerType.Public;
        public bool IsFirstCard;  // 是否正面朝上
        public bool IsRemembered; // 是否被记着
        public bool IsCopy; // 是否是场外牌
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

        public CardObj DeepCopy()
        {
            return (CardObj)MemberwiseClone();
        }


        public bool Equals(CardObj other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Value == other.Value && Suit == other.Suit && IsFirstCard == other.IsFirstCard && IsRemembered == other.IsRemembered && IsCopy == other.IsCopy && Owner == other.Owner;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((CardObj)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)Value, (int)Suit, IsFirstCard, IsRemembered, IsCopy, (int)Owner);
        }
    }
}