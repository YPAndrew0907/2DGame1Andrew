 namespace Obj
{
    public enum CardValue
    {
        A = 0, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, J, Q, K, Joker,Back
    }

    public enum CardSuit
    {
        Spade,Heart,Club,Diamond
    }
    
    public class CardObj
    {
        public readonly CardValue  Value;
        public readonly CardSuit   Suit;
        public          bool       IsFaceUp; // 是否正面朝上
        public          PlayerType Owner;
        public          long       TimeTicks;

        public CardObj(CardValue value, CardSuit suit)
        {
            Value    = value;
            Suit     = suit;
            IsFaceUp = false;
            Owner    = PlayerType.Public;
        }

        public override string ToString()
        {
            return $"【{Owner}】--花色：【{Suit}】--牌值：【{Value}】";
        }
    }
}