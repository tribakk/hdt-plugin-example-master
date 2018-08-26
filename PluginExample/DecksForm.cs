using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PluginExample
{
    public partial class DecksForm : Form
    {
        kernal.PlayedDeck m_ShowDeck = null;
        List<kernal.PlayedDeck> m_DeckList = null;
        public DecksForm()
        {
            InitializeComponent();
        }

        public void SetShowDeck(kernal.PlayedDeck deck)
        {
            m_ShowDeck = deck;
            m_DeckList = null;
            System.Drawing.Size size = m_ShowDeck.GetSize();
            size.Width += (7 * 2); //бордеры формочки
            size.Height += (30 + 6);
            if (size.Width < 100)
                size.Width = 100;
            if (size.Width < 100)
                size.Width = 100;
            Size = size;
            RedrawDeck();
        }

        public void SetShowDeckList(List<kernal.PlayedDeck> listDeck)
        {
            m_DeckList = listDeck;
            m_ShowDeck = null;
            int count = m_DeckList.Count;
            Size formSize = new Size(0, 0);
            for (int i = 0; i< count; i++)
            {
                Size deckSize = m_DeckList[i].GetSize();
                formSize.Width += deckSize.Width;
                formSize.Height = Math.Max(formSize.Height, deckSize.Height);
            }
            formSize.Width += (7 * 2); //бордеры формочки
            formSize.Height += (30 + 6);
            this.Size = formSize;
            RedrawDeck();
        }

        public void RedrawDeck()
        {
            if (m_ShowDeck != null)
            {
                System.Drawing.Graphics pGraphics = this.CreateGraphics();
                m_ShowDeck.DrawToGraphics(pGraphics);
            }
            else if (m_DeckList != null)
            {
                System.Drawing.Graphics pGraphics = this.CreateGraphics();
                pGraphics.Clear(Color.Honeydew);
                int count = m_DeckList.Count;
                int CurrentX = 0;
                for (int i = 0; i < count; i++)
                {
                    kernal.PlayedDeck deck = m_DeckList[i];
                    Size deckSize = deck.GetSize();
                    var saveState = pGraphics.Save();
                    pGraphics.TranslateTransform(CurrentX, 0);
                    deck.DrawToGraphics(pGraphics);
                    pGraphics.Restore(saveState);
                    CurrentX += deckSize.Width;
                }
            }
        }

        private void DecksForm_Paint(object sender, PaintEventArgs e)
        {
            RedrawDeck();
        }
    }
}
