using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginExample.kernal
{
    public class GameAnalyzer
    {
        DecksCollection m_GameDecks = new DecksCollection();
        PlayedDeck m_CurrentDeck = new PlayedDeck();
        Dictionary<Hearthstone_Deck_Tracker.Hearthstone.Card, int> m_BestDeck;
        private CurvyList m_List = null;
        bool m_bFirstPlayCard = true;

        public GameAnalyzer(CurvyList list)
        {
            m_List = list;
        }

        public void OnGameStart(/*HearthDb.Enums.CardClass CardClass*/)
        {
            if (m_List != null)
                m_List.Show();
            m_CurrentDeck = new PlayedDeck();
            //m_GameDecks = m_GameDecks.FilterDeck(CardClass);
        }

        public void OnOpponentPlayCard(Hearthstone_Deck_Tracker.Hearthstone.Card card)
        {
            if (!card.Collectible)
                return;
            m_CurrentDeck.AddCard(card);
            m_BestDeck = m_GameDecks.GetBestCompareDeck(m_CurrentDeck);
            List<Hearthstone_Deck_Tracker.Hearthstone.Card> cardList = new List<Hearthstone_Deck_Tracker.Hearthstone.Card>();
            var pEnum = m_BestDeck.GetEnumerator();
            for (;pEnum.MoveNext();)
            {
                cardList.Add(pEnum.Current.Key);
            }
            for (int i = 0; i < cardList.Count - 1; i++)
            {
                for (int j = 0; j < cardList.Count - 1; j++)
                {
                    if (cardList[j].Cost > cardList[j+1].Cost)
                    {
                        var temp = cardList[j];
                        cardList[j] = cardList[j+1];
                        cardList[j+1] = temp;
                    }
                }
            }
            if (m_List != null)
            {
                m_List.Update(cardList);
                m_List.Show();
            }
            //m_List.Update(m_BestDeck);


        }

        public Dictionary<Hearthstone_Deck_Tracker.Hearthstone.Card, int> GetBestDeck()
        {
            return m_BestDeck; 
        }

        public void OnGameEnd()
        {

        }
    }
}
