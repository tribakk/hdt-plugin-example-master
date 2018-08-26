using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HDT = Hearthstone_Deck_Tracker;

namespace PluginExample.kernal
{
    public class GameAnalyzer
    {
        DecksCollection m_GameDecks = new DecksCollection();
        PlayedDeck m_CurrentDeck = new PlayedDeck();
        PlayedDeck m_BestDeck;
        DecksForm m_procentForm = new DecksForm();
        bool m_NeedFilterByClass = true;

        public GameAnalyzer()
        {
            m_procentForm.Size = new System.Drawing.Size(200, 200);
            m_procentForm.TopMost = true;
        }

        public void OnGameStart()
        {
            m_CurrentDeck = new PlayedDeck();
            
            m_procentForm.Show();
            m_NeedFilterByClass = true;
        }

        public void OnOpponentPlayCard(Hearthstone_Deck_Tracker.Hearthstone.Card card)
        {
            
            if (m_NeedFilterByClass)
            {
                var game = Hearthstone_Deck_Tracker.API.Core.Game;
                if (game != null) //для игры
                {
                    m_GameDecks = m_GameDecks.FilterDeck(utils.KlassConverter(game.Opponent.Class));
                    m_NeedFilterByClass = false;
                }
                else if(card.IsClassCard)//для отладки тестов (игры то нет)
                {
                    m_GameDecks = m_GameDecks.FilterDeck(utils.KlassConverter(card.PlayerClass));
                    m_NeedFilterByClass = false;
                }
            }

            if (!card.Collectible)
                return;
            if (card.IsCreated)
                return;
            m_CurrentDeck.AddCard(card);

            List<PlayedDeck> deckList = m_GameDecks.GetBestDeckList(m_CurrentDeck);
            deckList.Sort(delegate(PlayedDeck d1, PlayedDeck d2) { return d2.GetFoundPercent().CompareTo(d1.GetFoundPercent());});
            m_BestDeck = deckList[0];
            if (m_procentForm != null)
            {
                if (deckList.Count <= 8) //иначе по ширине в экран не влезет
                    m_procentForm.SetShowDeckList(deckList);
                else
                    m_procentForm.SetShowDeck(m_BestDeck);
            }
        }

        public PlayedDeck GetBestDeck()
        {
            return m_BestDeck;
        }

        public void OnGameEnd()
        {
            m_CurrentDeck.TrySave();
            m_procentForm.Hide();
        }
    }
}
