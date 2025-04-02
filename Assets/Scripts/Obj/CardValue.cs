 namespace Obj
{
    public enum CardValue
    {
        A,Two,Three,Four,Five,Six,Seven,Eight,Nine,Ten,J,Q,K
    }
    public enum CardSuit
    {
        Spade,Heart,Club,Diamond
    }
    
    public class Card
    {
        private readonly CardValue _value;
        private readonly CardSuit  _suit;

        public Card(CardValue value, CardSuit suit)
        {
            _value     = value;
            _suit = suit;
        }
    }
}