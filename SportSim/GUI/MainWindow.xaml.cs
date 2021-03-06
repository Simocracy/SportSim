﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Simocracy.SportSim
{
	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		#region Constructor

		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;

			MainFrame.Content = new InfoPage();
		}

		#endregion

		#region Menu Commands

		/// <summary>
		/// Daten speichern
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuItemSave_Click(object sender, RoutedEventArgs e)
		{
			Settings.SaveSettings();
		}

		/// <summary>
		/// Teamverwaltung öffnen
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuItemTeams_Click(object sender, RoutedEventArgs e)
		{
			MainFrame.Content = new ManageFootballTeamsPage();
		}

		/// <summary>
		/// Stadionverwaltung öffnen
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuItemStadiums_Click(object sender, RoutedEventArgs e)
		{
			MainFrame.Content = new ManageStadiumsPage();
		}

		/// <summary>
		/// Staatenverwaltung öffnen
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuItemStates_Click(object sender, RoutedEventArgs e)
		{
			MainFrame.Content = new ManageStatesPage();
		}

		/// <summary>
		/// Gruppenvorlagenverwaltung öffnen
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuItemLeagueTemplates_Click(object sender, RoutedEventArgs e)
		{
			MainFrame.Content = new ManageLeagueWikiTemplatesPage();
		}

		/// <summary>
		/// Credits öffnen
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuItemCredits_Click(object sender, RoutedEventArgs e)
		{
			MainFrame.Content = new InfoPage();
		}

		/// <summary>
		/// Programm beenden
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuItemExit_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		/// <summary>
		/// Fußballliga simulieren
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuItemFootballLeague_Click(object sender, RoutedEventArgs e)
		{
			MainFrame.Content = new FootballLeaguePage();
		}

		#endregion
	}
}
