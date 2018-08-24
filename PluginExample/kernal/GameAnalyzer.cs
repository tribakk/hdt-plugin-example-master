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
        bool m_bFirstPlayCard = true;

        public GameAnalyzer()
        {
            m_procentForm.Size = new System.Drawing.Size(200, 200);
            m_procentForm.TopMost = true;
            m_bFirstPlayCard = true;
        }

        public void OnGameStart()
        {
            m_CurrentDeck = new PlayedDeck();
            
            m_procentForm.Show();
        }

        public void OnOpponentPlayCard(Hearthstone_Deck_Tracker.Hearthstone.Card card)
        {
            var game = Hearthstone_Deck_Tracker.API.Core.Game;
            if (game != null && m_bFirstPlayCard)
                m_GameDecks = m_GameDecks.FilterDeck(utils.KlassConverter(game.Opponent.Class));
            m_bFirstPlayCard = false;
            if (!card.Collectible)
                return;
            if (card.IsCreated)
                return;
            m_CurrentDeck.AddCard(card);
            m_BestDeck = m_GameDecks.GetBestCompareDeck(m_CurrentDeck);
            Dictionary<Hearthstone_Deck_Tracker.Hearthstone.Card, int> bestDeck = new Dictionary<Hearthstone_Deck_Tracker.Hearthstone.Card, int>();
            m_BestDeck.CopyToDict(bestDeck);
            List<Hearthstone_Deck_Tracker.Hearthstone.Card> cardList = utils.CardDictinaryToList(bestDeck);

            if (m_procentForm != null)
            {
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
