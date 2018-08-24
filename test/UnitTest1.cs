using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using PluginExample.kernal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestPlayAllDeck()
        {
            //создаем деку и разыгрывает карты из нее
            //в итоге не должно остаться карт в после разыгрывания деки

            HearthDb.Deckstrings.Deck deck = HearthDb.Deckstrings.DeckSerializer.Deserialize("AAECAf0ECvsMoM4Cws4Cm9MCnOICo+sCpvACt/ECw/gCxvgCCk3JA+wHm8IC08UClscCm8sC1+ECluQC4vgCAA==");
            GameAnalyzer gameAnalyzer = new GameAnalyzer();
            var pEnum = deck.GetCards().GetEnumerator();
            Dictionary<Hearthstone_Deck_Tracker.Hearthstone.Card, int> FindDeck = new Dictionary<Hearthstone_Deck_Tracker.Hearthstone.Card, int>();
            gameAnalyzer.OnGameStart();
            for (int i = 0; pEnum.MoveNext(); i++)
            {
                for (int cardNo = 0; cardNo < pEnum.Current.Value; cardNo++) //карта в 2х экземплярах
                    gameAnalyzer.OnOpponentPlayCard(new Hearthstone_Deck_Tracker.Hearthstone.Card(pEnum.Current.Key));
                gameAnalyzer.GetBestDeck().CopyToDict(FindDeck, true);
            }
            Assert.AreEqual(FindDeck.Count, 0);
        }
        [TestMethod]
        public void TestImageSize()
        {
            var cardDb = HearthDb.Cards.Collectible[HearthDb.CardIds.Collectible.Druid.AddledGrizzly];
            Hearthstone_Deck_Tracker.Hearthstone.Card card = new Hearthstone_Deck_Tracker.Hearthstone.Card(cardDb);
            System.Drawing.Bitmap bmp = Hearthstone_Deck_Tracker.Utility.ImageCache.GetCardBitmap(card);
            Assert.AreEqual(bmp.Height, PluginExample.kernal.ComponentSize.DeckSize.ImageHeight);
            Assert.AreEqual(bmp.Width, PluginExample.kernal.ComponentSize.DeckSize.ImageWidth);
        }

        [TestMethod]
        public void TestDeckSize()
        {
            PlayedDeck deck = new PlayedDeck("AAECAf0ECvsMoM4Cws4Cm9MCnOICo+sCpvACt/ECw/gCxvgCCk3JA+wHm8IC08UClscCm8sC1+ECluQC4vgCAA==");
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(1,1);
            System.Drawing.Graphics tempGr = System.Drawing.Graphics.FromImage(bmp);
            System.Drawing.Size drawSize = deck.DrawToGraphics(tempGr);
            Assert.IsTrue(drawSize.Equals(deck.GetSize()));
        }
    }
}
