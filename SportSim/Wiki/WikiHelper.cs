﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Simocracy.SportSim
{
	public class WikiHelper
	{
		#region Basics

		/// <summary>
		/// Entfernt den Wiki-Dateinamensraum aus dem angegeben String
		/// </summary>
		/// <param name="filename">Voller Dateiname mit Namensraum</param>
		/// <returns>Dateiname ohne Namensraum</returns>
		public static string RemoveFileNamespace(string filename)
		{
			return filename.TrimStart("Datei:".ToCharArray());
		}

		/// <summary>
		/// Prüft, ob der String eine gültige HTTP(S)-URL ist
		/// </summary>
		/// <param name="url">URL</param>
		/// <returns>True wenn gültige HTTP(S)-URL</returns>
		public static bool CheckValidHttpUrl(string url)
		{
			Uri uriResult;
			return Uri.TryCreate(url, UriKind.Absolute, out uriResult)
				&& (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
		}

		#endregion

		#region Football

		/// <summary>
		/// Erstellt den Code für die Tabelle der angegebenen <see cref="FootballLeague"/> asynchron
		/// </summary>
		/// <param name="league"><see cref="FootballLeague"/> der zu generierenden Tabelle</param>
		/// <param name="qual1Count">Anzahl der Qual1-Plätze</param>
		/// <param name="qual2Count">Anzahl der Qual2-Plätze</param>
		/// <returns>Generierten Tabellencode</returns>
		public async static Task<string> GenerateTableCodeAsync(FootballLeague league, int qual1Count, int qual2Count)
		{
			return await Task.Run(() => GenerateTableCode(league, qual1Count, qual2Count));
		}

		/// <summary>
		/// Erstellt den Code für die Tabelle der angegebenen <see cref="FootballLeague"/>
		/// </summary>
		/// <param name="league"><see cref="FootballLeague"/> der zu generierenden Tabelle</param>
		/// <param name="qual1Count">Anzahl der Qual1-Plätze</param>
		/// <param name="qual2Count">Anzahl der Qual2-Plätze</param>
		/// <returns>Generierten Tabellencode</returns>
		public static string GenerateTableCode(FootballLeague league, int qual1Count, int qual2Count)
		{
			StringBuilder sb = new StringBuilder(Settings.WikiStrings.FootballLeagueTableHeader);

			int position = 1;
			foreach(DataRow row in league.Table.Rows)
			{
				try
				{
					var team = row["Team"] as FootballTeam;
					var matches = (int) row["Matches"];
					var win = (int) row["Win"];
					var drawn = (int) row["Drawn"];
					var lose = (int) row["Lose"];
					var goalsFor = (int) row["GoalsFor"];
					var goalsAgainst = (int) row["GoalsAgainst"];
					var goalsString = String.Format("{0}:{1}", goalsFor, goalsAgainst);
					var points = (int) row["Points"];

					string classString = String.Empty;
					if(position <= qual1Count)
						classString = Settings.WikiStrings.ClassQual1;
					else if(position - qual1Count <= qual2Count)
						classString = Settings.WikiStrings.ClassQual2;

					sb.AppendFormat(Settings.WikiStrings.FootballLeagueTableElememt, classString, position++, team.Name, matches, win, drawn, lose, goalsString, points);
				}
				catch(Exception e)
				{
					System.Diagnostics.Debug.WriteLine(e);
				}
			}

			sb.Append(Settings.WikiStrings.TableEnd);
			return sb.ToString();
		}


		/// <summary>
		/// Generiert den Code für die Darstellung der Ergebnisse der angegebenen <see cref="FootballLeague"/> und nutzt dazu die angegebene <see cref="LeagueWikiTemplate"/> asynchron
		/// </summary>
		/// <param name="league"><see cref="FootballLeague"/> der darzustellenden Ergebnisse</param>
		/// <param name="template"><see cref="LeagueWikiTemplate"/> der darzustellenden Ergebnisse</param>
		/// <returns>Generierten Ergebniscode</returns>
		public async static Task<string> GenerateResultsCodeAsync(FootballLeague league, LeagueWikiTemplate template)
		{
			return await Task.Run(() => GenerateResultsCode(league, template));
		}

		/// <summary>
		/// Generiert den Code für die Darstellung der Ergebnisse der angegebenen <see cref="FootballLeague"/> und nutzt dazu die angegebene <see cref="LeagueWikiTemplate"/>
		/// </summary>
		/// <param name="league"><see cref="FootballLeague"/> der darzustellenden Ergebnisse</param>
		/// <param name="template"><see cref="LeagueWikiTemplate"/> der darzustellenden Ergebnisse</param>
		/// <returns>Generierten Ergebniscode</returns>
		public static string GenerateResultsCode(FootballLeague league, LeagueWikiTemplate template)
		{
			if(template == null)
				throw new NotImplementedException("Results output without template not implemented");

			var templateRegex = new Regex(@"\|?\s*([^=]*)\s*=");
			var teamRegex = new Regex(@"(A\d+)");
			var teamIndexRegex = new Regex(@"(\d+)");
			var templateCode = Regex.Replace(template.TemplateCode, @"\r\n?|\n", String.Empty);
			string[] templateLines = templateCode.Split('|');
			string[] filledLines = new string[templateLines.Length + 1];

			// Vorlagenzeilen einzeln durchgehen
			for(int i = 0; i < templateLines.Length; i++)
			{
				var line = templateLines[i].Replace("}}", String.Empty);

				var templateMatch = templateRegex.Match(line);
				var teamMatches = teamRegex.Matches(line);
				
				// Wenn nur ein Team enthalten ist: Teamnamen
				if(teamMatches.Count == 1)
				{
					var index = Int32.Parse(teamMatches[0].Value.Substring(1));
					var team = league.Teams[index - 1];
					line = String.Format("|{0}{1}", line, team.Name);
				}

				// Wenn 2 Teams enthalten sind: Ergebnisse oder Zusatzdaten
				else if(teamMatches.Count == 2)
				{
					var index1 = Int32.Parse(teamMatches[0].Value.Substring(1));
					var team1 = league.Teams[index1 - 1];
					var index2 = Int32.Parse(teamMatches[1].Value.Substring(1));
					var team2 = league.Teams[index2 - 1];

					var match = league.Matches.Where(x => x.TeamA == team1 && x.TeamB == team2).FirstOrDefault();
					line = String.Format("|{0}", line);

					// Datumsangabe
					if(line.Contains("Datum"))
					{
						line += match.Date.ToShortDateString();
					}
					// Ortsangabe
					else if(line.Contains("Ort"))
					{
						line += match.TeamA.Stadium.City;
					}
					// Spielergebnis
					else
					{
						line += String.Format("{0}:{1}", match.ResultA, match.ResultB);
					}
				}
				
				// Wenn nicht benötigte Variable
				else if(templateMatch.Success)
				{
					line = String.Format("|{0}", line);
				}

				filledLines[i] = line;

			}
			filledLines[filledLines.Length - 1] = "}}";

			return String.Join(Environment.NewLine, filledLines);
		}

		#endregion
	}
}
