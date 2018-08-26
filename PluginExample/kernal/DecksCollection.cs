using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginExample.kernal
{
    class DecksCollection
    {
        static string GetDeckFolder()
        {
            //string currentDir = System.IO.Directory.GetCurrentDirectory();
            //currentDir += "\\DECKS\\";
            //return currentDir;
            return "C:\\HS DECKS";
        }
        class CardInfo
        {
            public HearthDb.Card m_Card;
            public int m_Count;
            public CardInfo(HearthDb.Card card, int count)
            {
                m_Card = card;
                m_Count = count;
            }
        }

        List<PlayedDeck> m_Decks = new List<PlayedDeck>();
        public DecksCollection()
        {
            InitDeck();
        }
        public DecksCollection(List<PlayedDeck> decks)
        {
            //здесь не нужен InitDeck, т.к. мы инициализируемся уже готовым списком
            //по новой нам его не надо создавать
            m_Decks = decks;
        }
        public void AddDeck(PlayedDeck deck)
        {
            m_Decks.Add(deck);
        }

        public DecksCollection FilterDeck(HearthDb.Enums.CardClass CardClass)
        {
            List<PlayedDeck> filterDecks = new List<PlayedDeck>();

            int count = m_Decks.Count;
            for (int i = 0; i < count; i++)
            {
                if (m_Decks[i].GetClass() == CardClass)
                    filterDecks.Add(m_Decks[i]);
            }
            return new DecksCollection(filterDecks);
        }
        
        public void Copy(DecksCollection decks)
        {
            int count = decks.m_Decks.Count;
            for (int i = 0; i < count; i++)
            {
                PlayedDeck deck = new PlayedDeck();
                deck.Copy(decks.m_Decks[i]);
                m_Decks.Add(deck);
            }
        }

        public PlayedDeck GetBestCompareDeck(PlayedDeck playedDeck)
        {
            int count = m_Decks.Count;
            int number = 0;
            double maxValue = m_Decks[0].Compare(playedDeck);
            for (int i = 1; i < count; i++)
            {
                double temp = m_Decks[i].Compare(playedDeck);
                if (temp > maxValue)
                {
                    number = i;
                    maxValue = temp;
                }
            }
            playedDeck.SetFoundPercent(maxValue);
            PlayedDeck findDeck = new PlayedDeck(m_Decks[number].GetSerialzeDeck());
            findDeck.SetFoundPercent(maxValue);
            playedDeck.ClearDeck(findDeck);
            return findDeck;
        }
        class FindDeckInfo
        {
            public int DeckNumber = 0;
            public double Procent = 0.0; 
        };

        public List<PlayedDeck> GetBestDeckList(PlayedDeck playedDeck)
        {
            List<PlayedDeck> deckList = new List<PlayedDeck>();
            Dictionary<string, List<FindDeckInfo>> m_DeckNameMap = new Dictionary<string, List<FindDeckInfo>>();
            int count = m_Decks.Count;
            for (int i = 0; i<count; i++)
            {
                PlayedDeck deck = m_Decks[i];
                List<FindDeckInfo> FindDeckList = null;
                string deckName = deck.GetDeckName();
                if (!m_DeckNameMap.ContainsKey(deckName))
                    m_DeckNameMap.Add(deckName, new List<FindDeckInfo>());

                FindDeckList = m_DeckNameMap[deckName];
                FindDeckInfo info = new FindDeckInfo();
                info.DeckNumber = i;
                info.Procent = deck.Compare(playedDeck);
                FindDeckList.Add(info);
            }

            var pEnum = m_DeckNameMap.GetEnumerator();
            for (;pEnum.MoveNext();)
            {
                List<FindDeckInfo> deckListSort = pEnum.Current.Value;
                deckListSort.Sort
                (
                    delegate(FindDeckInfo deck1, FindDeckInfo deck2)
                    {
                        return deck2.Procent.CompareTo(deck1.Procent);
                    }
                );
                FindDeckInfo info = deckListSort[0];
                PlayedDeck findDeck = new PlayedDeck(m_Decks[info.DeckNumber].GetSerialzeDeck());
                findDeck.SetFoundPercent(info.Procent);
                playedDeck.ClearDeck(findDeck);
                deckList.Add(findDeck);
            }
            return deckList;
        }


        private void InitDeck()
        {
            System.Diagnostics.Debug.Write("INIT DECKS");
            InitDruidDeckFrom("DRUID");
            InitDruidDeckFrom("HUNTER");
            InitDruidDeckFrom("MAGE");
            InitDruidDeckFrom("PALADIN");
            InitDruidDeckFrom("PRIEST");
            InitDruidDeckFrom("ROGUE");
            InitDruidDeckFrom("SHAMAN");
            InitDruidDeckFrom("WARLOCK");
            InitDruidDeckFrom("WARRIOR");
        }

        private void InitDruidDeckFrom(string classFolder)
        {
            string folder = GetDeckFolder();
            folder += "\\" + classFolder + "\\";
            if (!System.IO.Directory.Exists(folder))
                return;
            string[] files = System.IO.Directory.GetFiles(folder);
            int count = files.Length;
            for (int i = 0; i<count; i++)
            {
                System.IO.TextReader tr = new System.IO.StreamReader(files[i]);
                string sDeck = tr.ReadToEnd();
                m_Decks.Add(new PlayedDeck(sDeck));
            }
        }
    }
}
