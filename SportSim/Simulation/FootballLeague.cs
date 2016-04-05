﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simocracy.SportSim
{
	/// <summary>
	/// Simulation einer Fußballliga bzw. Turniergruppe
	/// </summary>
	public class FootballLeague
	{
		#region Properties

		/// <summary>
		/// Teams der Liga
		/// </summary>
		public FootballTeamCollection Teams { get; set; }

		/// <summary>
		/// Spiele der Liga
		/// </summary>
		public ObservableCollection<FootballMatch> Matches { get; set; }

		/// <summary>
		/// Rundenmodus
		/// </summary>
		public LeagueRoundMode RoundMode { get; set; }

		/// <summary>
		/// Anzahl der Teams
		/// </summary>
		public int TeamCount { get { return Teams.Count; } }

		#endregion

		#region Constructor

		/// <summary>
		/// Erstellt eine neue Fußballliga
		/// </summary>
		public FootballLeague()
			: this(LeagueRoundMode.DoubleRound)
		{ }

		/// <summary>
		/// Erstellt eine neue Fußballliga
		/// </summary>
		/// <param name="roundMode">Rundenmodus der Liga</param>
		public FootballLeague(LeagueRoundMode roundMode)
			: this(roundMode, new FootballTeamCollection())
		{ }

		/// <summary>
		/// Erstellt eine neue Fußballliga
		/// </summary>
		/// <param name="roundMode">Rundenmodus der Liga</param>
		/// <param name="teams">Teams der Liga</param>
		public FootballLeague(LeagueRoundMode roundMode, params FootballTeam[] teams)
			: this(roundMode, new FootballTeamCollection(teams))
		{ }

		/// <summary>
		/// Erstellt eine neue Fußballliga
		/// </summary>
		/// <param name="roundMode">Rundenmodus der Liga</param>
		/// <param name="teams">Teams der Liga</param>
		public FootballLeague(LeagueRoundMode roundMode, List<FootballTeam> teams)
			: this(roundMode, new FootballTeamCollection(teams))
		{ }

		/// <summary>
		/// Erstellt eine neue Fußballliga
		/// </summary>
		/// <param name="roundMode">Rundenmodus der Liga</param>
		/// <param name="teams">Teams der Liga</param>
		public FootballLeague(LeagueRoundMode roundMode, FootballTeamCollection teams)
		{
			RoundMode = roundMode;
			Teams = teams;
		}

		#endregion

		#region Simulation

		#endregion
	}
}