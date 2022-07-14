using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine.GameObjects.CardGame
{
    class Library
    {
        private LinkedList<Card> _library;
        
        public int Count
        {
            get { return _library.Count; }
        }

        public Library(LinkedList<Card> deck)
        {
            _library = deck;
        }

        public Card Draw()
        {
            Card card = _library.First.Value;
            _library.RemoveFirst();
            return card;
        }

        public void Shuffle()
        {
            int count = _library.Count;
            int n = count;
            Random rnd = new Random();
            while (n > 1)
            {
                Card[] copy = new Card[count];
                _library.CopyTo(copy, 0);

                int i = rnd.Next(n) + count - n;
                _library.Remove(copy[i]);
                _library.AddFirst(copy[i]);
                n--;
            }
        }

        public override String ToString()
        {
            return "Library()";
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                return (_library == ((Library)obj)._library);
            }
        }
    }
}
