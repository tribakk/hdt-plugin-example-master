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
        System.Drawing.Graphics m_Graphics;
        System.Windows.Forms.Form m_procentForm = new System.Windows.Forms.Form();
        bool m_bFirstPlayCard = true;

        public GameAnalyzer()
        {
            m_Graphics = m_procentForm.CreateGraphics();
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
                System.Drawing.Size size = m_BestDeck.GetSize();
                size.Width += (7 * 2); //бордеры формочки
                size.Height += (30 + 6);
                if (size.Width < 100)
                    size.Width = 100;
                if (size.Width < 100)
                    size.Width = 100;
                m_procentForm.Size = size;
                m_Graphics = m_procentForm.CreateGraphics();
                m_BestDeck.DrawToGraphics(m_Graphics);
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
