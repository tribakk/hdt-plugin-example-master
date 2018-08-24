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
        public DecksForm()
        {
            InitializeComponent();
        }

        public void SetShowDeck(kernal.PlayedDeck deck)
        {
            m_ShowDeck = deck;
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

        public void RedrawDeck()
        {
            if (m_ShowDeck != null)
            {
                System.Drawing.Graphics pGraphics = this.CreateGraphics();
                m_ShowDeck.DrawToGraphics(pGraphics);
            }
        }

        private void DecksForm_Paint(object sender, PaintEventArgs e)
        {
            RedrawDeck();
        }
    }
}
