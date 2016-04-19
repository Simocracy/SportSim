﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simocracy.SportSim
{
	/// <summary>
	/// Simulation einer Fußballliga bzw. Turniergruppe
	/// </summary>
	[DebuggerDisplay("TeamCount={TeamCount}")]
	public class FootballLeague : INotifyPropertyChanged
	{
		#region Members

		private FootballTeamCollection _Teams;
		private ObservableCollection<FootballMatch> _Matches;
		private ELeagueRoundMode _RoundMode;
		private DataTable _Table;

		#endregion

		#region Constructor

		/// <summary>
		/// Erstellt eine neue Fußballliga
		/// </summary>
		public FootballLeague()
			: this(ELeagueRoundMode.DoubleRound)
		{ }

		/// <summary>
		/// Erstellt eine neue Fußballliga
		/// </summary>
		/// <param name="roundMode">Rundenmodus der Liga</param>
		public FootballLeague(ELeagueRoundMode roundMode)
			: this(roundMode, new FootballTeamCollection())
		{ }

		/// <summary>
		/// Erstellt eine neue Fußballliga
		/// </summary>
		/// <param name="teams">Teams der Liga</param>
		public FootballLeague(params FootballTeam[] teams)
			: this(ELeagueRoundMode.DoubleRound, new FootballTeamCollection(teams))
		{ }

		/// <summary>
		/// Erstellt eine neue Fußballliga
		/// </summary>
		/// <param name="teams">Teams der Liga</param>
		public FootballLeague(List<FootballTeam> teams)
			: this(ELeagueRoundMode.DoubleRound, new FootballTeamCollection(teams))
		{ }

		/// <summary>
		/// Erstellt eine neue Fußballliga
		/// </summary>
		/// <param name="teams">Teams der Liga</param>
		public FootballLeague(FootballTeamCollection teams)
			: this(ELeagueRoundMode.DoubleRound, teams)
		{ }

		/// <summary>
		/// Erstellt eine neue Fußballliga
		/// </summary>
		/// <param name="roundMode">Rundenmodus der Liga</param>
		/// <param name="teams">Teams der Liga</param>
		public FootballLeague(ELeagueRoundMode roundMode, params FootballTeam[] teams)
			: this(roundMode, new FootballTeamCollection(teams))
		{ }

		/// <summary>
		/// Erstellt eine neue Fußballliga
		/// </summary>
		/// <param name="roundMode">Rundenmodus der Liga</param>
		/// <param name="teams">Teams der Liga</param>
		public FootballLeague(ELeagueRoundMode roundMode, List<FootballTeam> teams)
			: this(roundMode, new FootballTeamCollection(teams))
		{ }

		/// <summary>
		/// Erstellt eine neue Fußballliga
		/// </summary>
		/// <param name="roundMode">Rundenmodus der Liga</param>
		/// <param name="teams">Teams der Liga</param>
		public FootballLeague(ELeagueRoundMode roundMode, FootballTeamCollection teams)
		{
			RoundMode = roundMode;
			Teams = teams;

			Matches = new ObservableCollection<FootballMatch>();
			CreateMatches();
			CreateTable();

			SimpleLog.Log("League created");
		}

		#endregion

		#region Properties

		/// <summary>
		/// Teams der Liga
		/// </summary>
		public FootballTeamCollection Teams
		{
			get { return _Teams; }
			set { _Teams = value; Notify(); }
		}

		/// <summary>
		/// Spiele der Liga
		/// </summary>
		public ObservableCollection<FootballMatch> Matches
		{
			get { return _Matches; }
			set { _Matches = value; Notify(); }
		}

		/// <summary>
		/// Rundenmodus
		/// </summary>
		public ELeagueRoundMode RoundMode
		{
			get { return _RoundMode; }
			set { _RoundMode = value; Notify(); }
		}

		/// <summary>
		/// Anzahl der Teams
		/// </summary>
		public int TeamCount { get { return Teams.Count; } }

		/// <summary>
		/// Tabelle der Liga
		/// </summary>
		public DataTable Table
		{
			get { return _Table; }
			set { _Table = value; Notify(); }
		}

		#endregion

		#region Simulation

		/// <summary>
		/// Erstellt den Spielplan der Liga
		/// </summary>
		public void CreateMatches()
		{
			Matches.Clear();

			switch(RoundMode)
			{
				case ELeagueRoundMode.SingleRound:
					int id = 0;
					for(int i = 0; i < Teams.Count; i++)
					{
						for(int j = i + 1; j < Teams.Count; j++)
						{
							Matches.Add(new FootballMatch(id++, Teams[i], Teams[j]));
						}
					}
					break;

				case ELeagueRoundMode.DoubleRound:
					foreach(var teamA in Teams)
					{
						foreach(var teamB in Teams)
						{
							if(teamA != teamB)
								Matches.Add(new FootballMatch(Matches.Count, teamA, teamB));
						}
					}
					break;

				case ELeagueRoundMode.QuadrupleRound:
					foreach(var teamA in Teams)
					{
						foreach(var teamB in Teams)
						{
							if(teamA != teamB)
								Matches.Add(new FootballMatch(Matches.Count, teamA, teamB));
						}
					}
					foreach(var teamA in Teams)
					{
						foreach(var teamB in Teams)
						{
							if(teamA != teamB)
								Matches.Add(new FootballMatch(Matches.Count, teamA, teamB));
						}
					}
					break;
			}

			SimpleLog.Info("Matches Created");
		}

		/// <summary>
		/// Simuliert alle Spiele in der Liga
		/// </summary>
		public void Simulate()
		{
			SimpleLog.Info("Simulate Matches");
			foreach(var match in Matches)
				match.Simulate();
		}

		/// <summary>
		/// Simuliert alle Spiele in der Liga asynchron
		/// </summary>
		public async Task SimulateAsync()
		{
			await Task.Run(() => Simulate());
		}

		/// <summary>
		/// Berechnet die Tabelle asynchron
		/// </summary>
		public async Task CalculateTableAsync()
		{
			await Task.Run(() => CalculateTable());
		}

		/// <summary>
		/// Berechnet die Tabelle
		/// </summary>
		public void CalculateTable()
		{
			SimpleLog.Info("Calculate Table");
			CreateTable();

			foreach(var team in Teams)
			{
				var row = Table.NewRow();

				int win, drawn, lose, goalsFor, goalsAgainst;
				win = drawn = lose = goalsFor = goalsAgainst = 0;

				foreach(var match in Matches)
				{
					if(match.TeamA == team)
					{
						if(match.ResultA > match.ResultB)
							win++;
						else if(match.ResultA == match.ResultB)
							drawn++;
						else
							lose++;

						goalsFor += match.ResultA;
						goalsAgainst += match.ResultB;
					}
					else if(match.TeamB == team)
					{
						if(match.ResultB > match.ResultA)
							win++;
						else if(match.ResultB == match.ResultA)
							drawn++;
						else
							lose++;

						goalsFor += match.ResultB;
						goalsAgainst += match.ResultA;
					}
				}

				row["Team"] = team;
				row["Matches"] = win + drawn + lose;
				row["Win"] = win;
				row["Drawn"] = drawn;
				row["Lose"] = lose;
				row["GoalsFor"] = goalsFor;
				row["GoalsAgainst"] = goalsAgainst;
				row["GoalDiff"] = goalsFor - goalsAgainst;
				row["Points"] = win * 3 + drawn;

				Table.Rows.Add(row);
			}

			DataView dv = Table.DefaultView;
			dv.Sort = "Points DESC, GoalDiff DESC, GoalsFor DESC";
			Table = dv.ToTable();
		}

		/// <summary>
		/// Erstellt die Ergebnistabelle
		/// </summary>
		private void CreateTable()
		{
			var table = new DataTable();

			table.Columns.Add("Team", typeof(FootballTeam));
			table.Columns.Add("Matches", typeof(int));
			table.Columns.Add("Win", typeof(int));
			table.Columns.Add("Drawn", typeof(int));
			table.Columns.Add("Lose", typeof(int));
			table.Columns.Add("GoalsFor", typeof(int));
			table.Columns.Add("GoalsAgainst", typeof(int));
			table.Columns.Add("GoalDiff", typeof(int));
			table.Columns.Add("Points", typeof(int));

			Table = table;
		}

		#endregion

		#region INotifyPropertyChanged
		
		public event PropertyChangedEventHandler PropertyChanged;
		protected void Notify([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
