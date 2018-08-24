using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Hearthstone;

namespace PluginExample
{
	public partial class CurvyList
	{
        double m_Size = 0;
		public CurvyList()
		{
			InitializeComponent();
		}

		public void Update(List<Card> cards)
		{
			// hide if card list is empty
			this.Visibility = cards.Count <= 0 ? Visibility.Hidden : Visibility.Visible;
			this.ItemsSource = cards;
			UpdatePosition();
		}
        public void Update(Dictionary<Card, int> cards)
        {
            // hide if card list is empty
            m_Size = cards.Count;
            this.Visibility = cards.Count <= 0 ? Visibility.Hidden : Visibility.Visible;
            this.ItemsSource = cards;
            UpdatePosition();
        }

        public void UpdatePosition()
		{
            //Canvas.SetTop(this, Core.OverlayWindow.Height * 50 / 100);
            Canvas.SetBottom(this, Core.OverlayWindow.Height * (double)50 / 15 * m_Size / 100);
            Canvas.SetLeft(this, Core.OverlayWindow.Width * 0 / 100);
		}

		public void Show()
		{
			this.Visibility = Visibility.Visible;
		}

		public void Hide()
		{
			this.Visibility = Visibility.Hidden;
		}
	}
}