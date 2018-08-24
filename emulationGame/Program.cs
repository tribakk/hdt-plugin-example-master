using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginExample.kernal;
using System.Drawing;
using HDT = Hearthstone_Deck_Tracker;

namespace emulationGame
{
    class Program
    {
        static void TestDeck()
        {
            HearthDb.Deckstrings.Deck deck = HearthDb.Deckstrings.DeckSerializer.Deserialize("AAECAf0ECvsMoM4Cws4Cm9MCnOICo+sCpvACt/ECw/gCxvgCCk3JA+wHm8IC08UClscCm8sC1+ECluQC4vgCAA==");
            GameAnalyzer gameAnalyzer = new GameAnalyzer();
            var pEnum = deck.GetCards().GetEnumerator();
            Dictionary<Hearthstone_Deck_Tracker.Hearthstone.Card, int> FindDeck = new Dictionary<Hearthstone_Deck_Tracker.Hearthstone.Card, int>();
            gameAnalyzer.OnGameStart();
            for (int i = 0; pEnum.MoveNext(); i++)
            {
                for (int cardNo = 0; cardNo < pEnum.Current.Value; cardNo++)
                    gameAnalyzer.OnOpponentPlayCard(new Hearthstone_Deck_Tracker.Hearthstone.Card(pEnum.Current.Key));
                gameAnalyzer.GetBestDeck().CopyToDict(FindDeck, true);
                if (i > 10)
                    break;
            }
            gameAnalyzer.OnGameEnd();
            //gameAnalyzer.OnOpponentPlayCard(new Hearthstone_Deck_Tracker.Hearthstone.Card(HearthDb.Cards.Collectible[HearthDb.CardIds.Collectible.Druid.AddledGrizzly]));
            //FindDeck = gameAnalyzer.GetBestDeck();
        }

        static void TestLoadDeck()
        {
            //string currentDir = System.IO.Directory.GetCurrentDirectory();
            //currentDir += "\\DECKS\\";

            //string deck1 = currentDir += "deck1.txt";
            //System.IO.TextReader tr = new System.IO.StreamReader(deck1);
            //string sDeck = tr.ReadToEnd();
            //HearthDb.Deckstrings.Deck deck = HearthDb.Deckstrings.DeckSerializer.Deserialize(sDeck);
            //var Deck = deck.GetCards();
        }

        static void TestBadDeck()
        {
            GameAnalyzer gameAnalyzer = new GameAnalyzer();
            gameAnalyzer.OnGameStart();
            gameAnalyzer.OnOpponentPlayCard(new Hearthstone_Deck_Tracker.Hearthstone.Card(HearthDb.Cards.Collectible[HearthDb.CardIds.Collectible.Druid.AddledGrizzly]));
            gameAnalyzer.OnOpponentPlayCard(new Hearthstone_Deck_Tracker.Hearthstone.Card(HearthDb.Cards.Collectible[HearthDb.CardIds.Collectible.Mage.Aluneth]));
            gameAnalyzer.OnGameEnd();
        }

        static void TestImage()
        {
            Hearthstone_Deck_Tracker.Hearthstone.Card card = new Hearthstone_Deck_Tracker.Hearthstone.Card(HearthDb.Cards.Collectible[HearthDb.CardIds.Collectible.Druid.AncientOfWar]);
            System.Windows.Media.Imaging.BitmapImage image = Hearthstone_Deck_Tracker.Utility.ImageCache.GetCardImage(card);
            System.Drawing.Bitmap bitmap = Hearthstone_Deck_Tracker.Utility.ImageCache.GetCardBitmap(card);
            System.Windows.Forms.Form pForm = new System.Windows.Forms.Form();
            pForm.Show();
            pForm.Size = new System.Drawing.Size(200, 200);
            System.Drawing.Graphics pGraphics = pForm.CreateGraphics();
            pGraphics.ScaleTransform(1.0f, 0.5f);
            
            Font pFont = new Font("Arial", 16);
            SolidBrush pBrush = new SolidBrush(Color.Black);
            PointF pPoint = new PointF(0.0f, 0.0f);

            pGraphics.DrawImage(bitmap,0,0);
            pGraphics.DrawString(card.Name, pFont, pBrush, pPoint);
            System.Drawing.Size size = bitmap.Size;
            string opClass = Hearthstone_Deck_Tracker.API.Core.Game.Opponent.Class;
            pGraphics.Clear(System.Drawing.Color.Cyan);

        }

        static void Main(string[] args)
        {
            
            
            TestDeck();
            //TestLoadDeck();
            //TestBadDeck();
            //TestImage();
        }
    }
}
