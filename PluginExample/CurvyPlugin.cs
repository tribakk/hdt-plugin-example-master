using System;
using System.Windows.Controls;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Plugins;


namespace PluginExample
{
	public class CurvyPlugin : IPlugin
	{
		private CurvyList _list;
        //private kernal.GameAnalyzer gameAnalyzer;

		public string Author
		{
			get { return "andburn"; }
		}

		public string ButtonText
		{
			get { return "Settings"; }
		}

		public string Description
		{
			get { return "A simple example plugin showing the oppoents class cards on curve."; }
		}

		public MenuItem MenuItem
		{
			get { return null; }
		}

		public string Name
		{
			get { return "Curvy"; }
		}

		public void OnButtonPress()
		{
		}

		public void OnLoad()
		{
            //System.Windows.MessageBox.Show("sadf");
            
            System.Diagnostics.Debug.Write("LOAD HS PLUGIN");
            _list = new CurvyList();
            _list.Show();
			Core.OverlayCanvas.Children.Add(_list);
			Curvy curvy = new Curvy(_list);
            kernal.GameAnalyzer gameAnalyzer =  new kernal.GameAnalyzer();


            //GameEvents.OnGameStart.Add(curvy.GameStart);
            //GameEvents.OnInMenu.Add(curvy.InMenu);
            //GameEvents.OnTurnStart.Add(curvy.TurnStart);
            GameEvents.OnGameStart.Add(gameAnalyzer.OnGameStart);
            GameEvents.OnOpponentPlay.Add(gameAnalyzer.OnOpponentPlayCard);
            GameEvents.OnGameEnd.Add(gameAnalyzer.OnGameEnd);
            GameEvents.OnOpponentDeckDiscard.Add(gameAnalyzer.OnOpponentPlayCard);
            GameEvents.OnOpponentHandDiscard.Add(gameAnalyzer.OnOpponentPlayCard);
        }

		public void OnUnload()
		{
			Core.OverlayCanvas.Children.Remove(_list);
		}

		public void OnUpdate()
		{
		}

		public Version Version
		{
			get { return new Version(0, 1, 1); }
		}
	}
}