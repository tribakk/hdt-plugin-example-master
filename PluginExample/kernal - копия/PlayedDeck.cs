using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PluginExample.kernal
{
    [Serializable]
    class PlayedDeck
    {
        Dictionary<Hearthstone_Deck_Tracker.Hearthstone.Card, int> m_Cards = new Dictionary<Hearthstone_Deck_Tracker.Hearthstone.Card, int>();
        HearthDb.Enums.CardClass m_Class;
        double m_DeckPersent = 0;
        public PlayedDeck()
        {

        }

        public void SetFoundPercent(double p)
        {
            m_DeckPersent = p;
        }

        public void TrySave()
        {
            if (m_DeckPersent >= 0.8)
                return;

            var pEnum = m_Cards.GetEnumerator();
            string stringDeck = "";
            for(;pEnum.MoveNext();)
            {
                stringDeck += pEnum.Current.Key + Environment.NewLine;
            }

            string dir = "C:\\HS DECKS\\NotFound";
            string foundName = dir;
            for (int i = 0; ;i++)
            {
                foundName = dir + i.ToString() + ".txt";
                if (System.IO.Directory.Exists(foundName))
                    continue;
                break;
            }
            System.IO.StreamWriter wr = new System.IO.StreamWriter(foundName);
            wr.WriteLine(stringDeck);
            wr.Dispose();
        }

        public PlayedDeck(string InputDeck)
        {
            HearthDb.Deckstrings.Deck deck = HearthDb.Deckstrings.DeckSerializer.Deserialize(InputDeck);
            Dictionary<HearthDb.Card, int> DbDeck = deck.GetCards();
            var pEnum = DbDeck.GetEnumerator();
            for (;pEnum.MoveNext();)
            {
                m_Cards.Add(new Hearthstone_Deck_Tracker.Hearthstone.Card(pEnum.Current.Key), pEnum.Current.Value);
            }
            m_Class = deck.GetHero().Class;
        }
        public HearthDb.Enums.CardClass GetClass()
        {
            return m_Class;
        }
        public void AddCard(Hearthstone_Deck_Tracker.Hearthstone.Card card)
        {
            if (!card.Collectible)
                return;
            if (HaveCard(card) > 0)
                m_Cards[card] ++;
            else
                m_Cards.Add(card, 1);
        }

        public void ClearDeck(Dictionary<Hearthstone_Deck_Tracker.Hearthstone.Card, int> deck)
        {
            var enum1 = m_Cards.GetEnumerator();
            for(;enum1.MoveNext();)
            {
                var enum2 = deck.GetEnumerator();
                for (;enum2.MoveNext();)
                {
                    if (enum1.Current.Key.Equals(enum2.Current.Key))
                    {
                        deck[enum1.Current.Key] = Math.Max(0, enum2.Current.Value - enum1.Current.Value);
                        if (deck[enum1.Current.Key] == 0) 
                            deck.Remove(enum1.Current.Key);
                        break;
                    }
                }
            }
        }

        public int HaveCard(Hearthstone_Deck_Tracker.Hearthstone.Card card)
        {
            var pEnum = m_Cards.GetEnumerator();
            int CardCount = 0;
            
            for (; pEnum.MoveNext();)
            {
                if (pEnum.Current.Key.Equals(card))
                {
                    CardCount = pEnum.Current.Value;
                    break;
                }
            }
            return CardCount;
        }

        public void RemoveCard(Hearthstone_Deck_Tracker.Hearthstone.Card card)
        {
            int CardCount = HaveCard(card);
            if (CardCount > 0)
            {
                m_Cards[card] --;
                if (m_Cards[card] == 0)
                    m_Cards.Remove(card);
            }
        }

        public void Copy(PlayedDeck deck)
        {
            m_Cards = new Dictionary<Hearthstone_Deck_Tracker.Hearthstone.Card, int>(deck.m_Cards);
            m_Class = deck.m_Class;
        }

        public void CopyToDict(Dictionary<Hearthstone_Deck_Tracker.Hearthstone.Card, int> dict)
        {
            var en = m_Cards.GetEnumerator();
            for (;en.MoveNext();)
            {
                var en1 = dict.GetEnumerator();
                bool bFind = false;
                for (;en1.MoveNext();)
                {
                    if (en1.Current.Key.Equals(en.Current.Key))
                    {
                        dict[en1.Current.Key] += en.Current.Value;
                        bFind = true;
                        break;
                    }
                }
                if (!bFind)
                    dict.Add(en.Current.Key, en.Current.Value);
            }
        }

        public double Compare(PlayedDeck deck)
        {
            int SameCardCount = 0;
            int AllCardCount = 0;
            double procent = 0;
            var pEnum = deck.m_Cards.GetEnumerator();
            for (;pEnum.MoveNext();)
            {
                //если в сыгранно в этой партии колоде 2 одинаковые карты, а в эталонной одна, то исходная логика нарушается
                int testDeckCard = pEnum.Current.Value;
                int idealDeckCard = HaveCard(pEnum.Current.Key);
                AllCardCount += testDeckCard;
                SameCardCount += Math.Min(testDeckCard, idealDeckCard);
            }
            procent = (double)SameCardCount / (double)AllCardCount;
            return procent;
        }
    }
}
