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
            InitDeck();
            m_Decks = decks;
        }
        public void AddDeck(PlayedDeck deck)
        {
            m_Decks.Add(deck);
        }
        public void RemoveDeck(PlayedDeck deck)
        {

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

        public Dictionary<Hearthstone_Deck_Tracker.Hearthstone.Card, int> GetBestCompareDeck(PlayedDeck playedDeck)
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
            string fileName = GetDeckFolder() + "\\procent.txt";
            System.IO.StreamWriter wr = new System.IO.StreamWriter(fileName);
            wr.WriteLine(maxValue.ToString());
            wr.Dispose();
            Dictionary<Hearthstone_Deck_Tracker.Hearthstone.Card, int> BestDeck = new Dictionary<Hearthstone_Deck_Tracker.Hearthstone.Card, int>();
            if (maxValue >= 0.8)
            {
                m_Decks[number].CopyToDict(BestDeck);
                playedDeck.ClearDeck(BestDeck);
            }
            return BestDeck;
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

            InitDruidDeck();
            InitHuntDeck();
            InitMageDeck();
            InitPaladinDeck();
            InitPriestDeck();
            InitRogueDeck();
            InitShamanDeck();
            InitWarlockDeck();
            InitWarriorDeck();
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
        private void InitDruidDeck()
        {
            //DRUID
            // Mecha'thun Druid
            m_Decks.Add(new PlayedDeck("AAECAZICApnTAvH7Ag5AX+kB/gHTA8QGpAf2B+QIktICmNICntICv/ICj/YCAA=="));
            //Big Druid
            m_Decks.Add(new PlayedDeck("AAECAZICCsIGognCzgKZ0wKv0wL94QL55gLx6gKM+wL1/AIKX+kB5AjJxwKgzQKY0gKe0gLm0wLd6wKP9gIA"));
            //Malygos Druid
            m_Decks.Add(new PlayedDeck("AAECAZICBFa0A5nTAoz7Ag1AX9MD5AigzQKHzgKU0gKY0gKe0gLb0wKE5gLi+ALk+wIA"));
            //Mill Druid
            m_Decks.Add(new PlayedDeck("AAECAZICBlbtBZnTAv3rApruAuT7AgxAX+kBxAbkCKDNAofOApjSAp7SAtvTAr/yAo/2AgA="));
            //Token Druid
            m_Decks.Add(new PlayedDeck("AAECAZICApnTAsX9Ag5AX/0C5gWFCOQIoM0Ch84CmNICntIC29MChOYC1+8C4vgCAA=="));
            ////
            //m_Decks.Add(new PlayedDeck(""));
            ////
            //m_Decks.Add(new PlayedDeck(""));
            ////
            //m_Decks.Add(new PlayedDeck(""));
            ////
            //m_Decks.Add(new PlayedDeck(""));
        }
        private void InitHuntDeck()
        {
            //Mech Hunter
            m_Decks.Add(new PlayedDeck("AAECAR8ErwT7Bf3qAuH1Ag2rwgLYwgLR4QLh4wLg9QLi9QLv9QK5+AK8/AKC/QL2/QKJgAPMgQMA"));
            //Midrange Hunter
            m_Decks.Add(new PlayedDeck("AAECAR8CxQiG0wIOqAK1A4cEyQTrB5cI2wn+DI7DAtfNAt3SAt/SAuPSAovlAgA="));
            //Spell Hunter
            m_Decks.Add(new PlayedDeck("AAECAR8C6dIChtMCDo0BqAK1A4cEyQSXCNsJ/gzd0gLf0gLj0gLh4wLq4wKH+wIA"));
            //Deathrattle Hunter
            m_Decks.Add(new PlayedDeck("AAECAR8E+AiG0wK26gLh9QINjQH7BZcI7QmrwgLYwgKczQLd0gLh4wLi9QK5+AKR+wL2/QIA"));
            ////
            //m_Decks.Add(new PlayedDeck(""));
            ////
            //m_Decks.Add(new PlayedDeck(""));
            ////
            //m_Decks.Add(new PlayedDeck(""));
        }
        

        private void InitMageDeck()
        {
            //Freeze Mage
            m_Decks.Add(new PlayedDeck("AAECAf0EBu0F7Ae4CNrFAs3rAuL4AgyKAfsBnAKuA8kDqwTLBOYElgW50QKW5AK0/AIA"));
            //Big Spell Mage
            m_Decks.Add(new PlayedDeck("AAECAf0ECE3QAssE7QXsB8/HApvTAvLTAguKAb8DyQOrBJvCApbHApbkAsP4AuL4Arn/Au+AAwA="));
            //Tempo Mage
            m_Decks.Add(new PlayedDeck("AAECAf0EBHGi0wLu9gLvgAMNuwKVA6sEtATmBJYF7AXBwQKYxAKP0wL77AKV/wK5/wIA"));
            //Elemental Mage
            m_Decks.Add(new PlayedDeck("AAECAf0ECooB0ALFBMrBAsLOApvTAtDnArfxAu72Asb4AgrhB5fBAqzCAuvCAsrDAsjHAs7vAs7yAsP4ArX8AgA="));
            //DK
            m_Decks.Add(new PlayedDeck("AAECAf0ECvsMoM4Cws4Cm9MCnOICo+sCpvACt/ECw/gCxvgCCk3JA+wHm8IC08UClscCm8sC1+ECluQC4vgCAA=="));
            ////
            //m_Decks.Add(new PlayedDeck(""));
            ////
            //m_Decks.Add(new PlayedDeck(""));
        }
        private void InitPaladinDeck()
        {
            //Mech Paladin
            m_Decks.Add(new PlayedDeck("AAECAZ8FBrnBAr3TArfpAuL4AvH+AqCAAwynBa8H1uUCpfUCmfcCkfsCmPsC9v0C1v4C1/4C4f4CzIEDAA=="));
            //Murloc Paladin
            m_Decks.Add(new PlayedDeck("AAECAZ8FAqcFucECDtsDpwizwQKdwgKxwgLjywL40gLR4QLW5QLi+ALW/gLh/gLMgQPeggMA"));
            //Odd Paladin
            m_Decks.Add(new PlayedDeck("AAECAZ8FBK8EpwXxBZ74Ag1G9QX5CpvCAuvCAoPHArjHAuPLApXOAvvTAtHhAtblArXmAgA="));
            ////
            //m_Decks.Add(new PlayedDeck(""));
            ////
            //m_Decks.Add(new PlayedDeck(""));
            ////
            //m_Decks.Add(new PlayedDeck(""));
            ////
            //m_Decks.Add(new PlayedDeck(""));
        }
        private void InitPriestDeck()
        {
            //Quest Priest
            m_Decks.Add(new PlayedDeck("AAECAa0GCKQDxQTTCtYKlsQCx8sCkNMC6vcCC4oB+wGhBMPBAqvCAsnHAovhAsvmArfxAqH+AoiCAwA="));
            //Combo Priest
            m_Decks.Add(new PlayedDeck("AAECAa0GBL7IAqbwAsv4Avz6Ag34AuUE9gfRCtIK8gz7DNHBAtjBAr/lAuX3ApH7AvGAAwA="));
            //Control Priest
            m_Decks.Add(new PlayedDeck("AAECAa0GBNMKkNMCqeIC8fsCDe0BnALlBPYH0gryDPsM0cEC8M8C6NACn+sCvfMC4/cCAA=="));
            ////
            //m_Decks.Add(new PlayedDeck(""));
            ////
            //m_Decks.Add(new PlayedDeck(""));
            ////
            //m_Decks.Add(new PlayedDeck(""));
            ////
            //m_Decks.Add(new PlayedDeck(""));
        }
        private void InitRogueDeck()
        {
            //Pogo Rogue
            m_Decks.Add(new PlayedDeck("AAECAaIHBrICmwWA0wLP4QLn+gKggAMMigG0Ae0CiAfdCIYJgcIC3NEC2+MC4vgC1/oC4PoCAA=="));
            //Malygos Rogue
            m_Decks.Add(new PlayedDeck("AAECAaIHCLQDxgXtBffBAoDTAvDmAuf6Auz8AguKAbQBxAHNA70EmwWIB4YJ2+MC3+MC3voCAA=="));
            //Odd Rogue
            m_Decks.Add(new PlayedDeck("AAECAaIHBKICrwTKywKe+AINjALLA9QF9QXdCIHCAp/CAuvCAsrDAtHhAovlAqbvAsf4AgA="));
            //Pick Pocket Rogue
            m_Decks.Add(new PlayedDeck("AAECAaIHBLICgNMC6/ACqPcCDbQBywObBYYJgcIC68ICm8gC5dEC2+MC6vMCt/UCovcCx/gCAA=="));
            //Deathrattle Rogue
            m_Decks.Add(new PlayedDeck("AAECAaIHBK8E5dECoIAD0YEDDbQBjALNA4gHhgmrwgLrwgKL4QLb4wL96gK09gLe+gLs/AIA"));
            //Tempo Rogue
            m_Decks.Add(new PlayedDeck("AAECAaIHAoDTAuvwAg60AZwC7QLNA70EmwWIB6QHhgmBwgKbyALc0QLb4wKo9wIA"));
            ////
            //m_Decks.Add(new PlayedDeck(""));
        }

        private void InitShamanDeck()
        {
            //Shudderwock Shaman
            m_Decks.Add(new PlayedDeck("AAECAaoICN4F08UCnOICq+cCw+oCp+4CnvAC7/cCC4EE9QT+Bf8Fsgb7DJfBAsfBApvLAvPnAu/xAgA="));
            //Token Shaman
            m_Decks.Add(new PlayedDeck("AAECAaoIBvPCAuvPArDwAqH4Apj7Avb9AgzTAe4B8AexCJMJkcEC68IC4vgC6voCj/sCnP8CjIADAA=="));
            //Even Shaman
            m_Decks.Add(new PlayedDeck("AAECAaoICCCZAv4F88ICws4C9uwCp+4CzfQCC70B0wHZB/AHsQiRwQKswgKbywKW6AKU7wKw8AIA"));
            ////
            //m_Decks.Add(new PlayedDeck(""));
            ////
            //m_Decks.Add(new PlayedDeck(""));
            ////
            //m_Decks.Add(new PlayedDeck(""));
            ////
            //m_Decks.Add(new PlayedDeck(""));
        }
        
        
        
        private void InitWarlockDeck()
        {
            //Pain Warlock
            m_Decks.Add(new PlayedDeck("AAECAf0GBP3QApziArjuAo+CAw0w0wH3BM4HwgjrwgLKwwKbywL3zQLy0ALR4QL09wLT+AIA"));
            //Zoo Warlock
            m_Decks.Add(new PlayedDeck("AAECAf0GAq8EnOICDjCEAfIFzgfCCPcMysMCm8sC980Cn84C8tAC0eECh+gC7/ECAA=="));
            //Control Warlock
            m_Decks.Add(new PlayedDeck("AAECAf0GBu0FkMcCzukCw/MCnPgC8fsCDNsGtgfECJvCAufLAqLNAvfNAvLQAojSAsXzAuL4Atf+AgA="));
            //Even Warlock
            m_Decks.Add(new PlayedDeck("AAECAf0GBIoHws4Cl9MCzfQCDYoB8gX7BrYH4Qf7B40I58sC8dAC/dACiNIC2OUC6uYCAA=="));
            ////
            //m_Decks.Add(new PlayedDeck(""));
            ////
            //m_Decks.Add(new PlayedDeck(""));
            ////
            //m_Decks.Add(new PlayedDeck(""));
        }
        private void InitWarriorDeck()
        {
            //Mech Warrior
            m_Decks.Add(new PlayedDeck("AAECAQcGws4CnOICze8C5PcCkvgCiYADDByOBYoG0eECuuwCnfAC4vgCg/sCnvsCs/wC9v0CzIEDAA=="));
            //Recruit Warrior
            m_Decks.Add(new PlayedDeck("AAECAQcK0gKiCYbNAsLOAvbPAsrnAv3nAurqArj2ApL4AgpLogSRBv8HxsICzM0C6ucCm/MC9PUCg/sCAA=="));
            //Quest Warrior
            m_Decks.Add(new PlayedDeck("AAECAQcGogSQB7II08MCn9MC9PUCDEuRBv8H+wybwgK+wwLKwwKbywLMzQLP5wKq7AKb8wIA"));
            //Control Warrior
            m_Decks.Add(new PlayedDeck("AAECAQcI0gKQB47OAsLOAp/TAurqArj2ApL4AgtLogSRBvsMvsMCzM0Cz+cCm/MC9PUC4vgCg/sCAA=="));
            //Odd Warrior
            m_Decks.Add(new PlayedDeck("AAECAQcMogLQAqoG+QzRwwLTxQKixwLD6gKS+AKe+ALi+AKO+wIJS6IE/AT/B/sMm8ICyucCg/sCs/wCAA=="));
            //
            //m_Decks.Add(new PlayedDeck(""));
            ////
            //m_Decks.Add(new PlayedDeck(""));
        }
        
    }
}
