 namespace Obj
{
    public enum CardValue
    {
        A = 0, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, J, Q, K, Back
    }

    public enum CardSuit
    {
        Spade,Heart,Club,Diamond
    }
    
    public class CardObj
    {
        public readonly CardValue Value;
        public readonly CardSuit  Suit;
        public bool IsFaceUp; // 是否正面朝上

        public CardObj(CardValue value, CardSuit suit)
        {
            Value     = value;
            Suit = suit;
            IsFaceUp = false;
        }
    }
}