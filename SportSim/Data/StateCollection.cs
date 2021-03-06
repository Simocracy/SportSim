﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Simocracy.SportSim
{
	/// <summary>
	/// Stellt verschiedene Methoden zum Verwalten von Staaten zur Verfügung
	/// </summary>
	[CollectionDataContract(Name = "States")]
	public class StateCollection : ObservableCollection<State>, ICollection<State>
	{
		#region Constructors

		/// <summary>
		/// Erstellt eine neue, leere StateCollection
		/// </summary>
		public StateCollection()
			: base()
		{ }

		/// <summary>
		/// Erstellt eine neue StateCollection mit Staaten
		/// </summary>
		/// <param name="stateCollection">IEnumerable mit Teams</param>
		public StateCollection(IEnumerable<State> stateCollection)
			: base(stateCollection)
		{ }

		/// <summary>
		/// Erstellt eine neue StateCollection mit Staaten
		/// </summary>
		/// <param name="stateList">IEnumerable mit Teams</param>
		public StateCollection(List<State> stateList)
			: base(stateList)
		{ }

		#endregion

		#region Manage States

		/// <summary>
		/// Erstellt einen neuen Staat und fügt ihn der Liste hinzu
		/// </summary>
		/// <param name="name">Name des Staates</param>
		public void Create(string name)
		{
			Add(new State(GetNewID(), name));
		}

		/// <summary>
		/// Erstellt einen neuen Staat und fügt ihn der Liste hinzu
		/// </summary>
		/// <param name="name">Name des Staates</param>
		/// <param name="flag">Flaggenkürzel des Staates</param>
		/// <param name="continent">Kontinent des Staates</param>
		public void Create(string name, string flag, EContinent continent)
		{
			Add(new State(GetNewID(), name, flag, continent));
		}

		/// <summary>
		/// Gibt den Staat mit dem angegebenen Namen zurück
		/// </summary>
		/// <param name="name">Name des Staates</param>
		/// <returns>Staat mit dem angegebenen Namen</returns>
		public State Get(string name)
		{
			var states = this.Where(x => x.Name == name);
			return (states.Count() != 1) ? null : states.First();
		}

		/// <summary>
		/// Gibt den Staat mit der angegebenen ID zurück
		/// </summary>
		/// <param name="id">ID des Staates</param>
		/// <returns>Staat mit der angegebenen ID</returns>
		public State Get(int id)
		{
			var states = this.Where(x => x.ID == id);
			return (states.Count() != 1) ? null : states.First();
		}

		#endregion

		#region Utilities

		/// <summary>
		/// Gibt die höchste bestehende ID eines Teams zurück oder -1, wenn kein Team vorhanden ist
		/// </summary>
		/// <returns>Höchste ID</returns>
		public int GetMaxID()
		{
			if(Count > 0)
				return this.Max(x => x.ID);
			else
				return -1;
		}

		/// <summary>
		/// Gibt eine neue ID für ein Team zurück
		/// </summary>
		/// <returns>Neue ID</returns>
		public int GetNewID()
		{
			return GetMaxID() + 1;
		}

		#endregion

		#region IExtensibleDataObject

		/// <summary>
		/// Erweiterungsdaten
		/// </summary>
		public ExtensionDataObject ExtensionData { get; set; }

		#endregion
	}
}
