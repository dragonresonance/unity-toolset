#if SIMPLEJSON


using DragonResonance.Behaviours;
using DragonResonance.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System;
using Tabernero.SimpleJSON;
using UnityEngine;




namespace DragonResonance.Savedata
{
	public partial class Savedata : PersistentSingletonPossumBehaviour<Savedata>
	{
		[SerializeField] private bool _loadOnStart = true;
		[SerializeField] private bool _useCompactData = false;
		[SerializeField] private string _defaultFilePath = "savedata.json";
		[SerializeField] private SFilePathOverride[] _overrides = { };


		private bool _ready = false;
		private readonly Dictionary<string, JSONNode> _data = new();
		private readonly Dictionary<string, Action<JSONNode>> _events = new();




		#region Events


			private void Start()
			{
				if (_loadOnStart)
					Load();
			}


		#endregion




		#region Publics - Files


			[ContextMenu(nameof(Load))]
			public void Load()
			{
				_data.Clear();
				foreach (string filePath in this.FilePaths) {
					string dataFilePath = GetOptimizedPersistentDataPath(filePath);
					if (!File.Exists(dataFilePath)) return;

					string content = File.ReadAllText(dataFilePath, Encoding.UTF8);
					JSONNode jsonNode = JSONNode.Parse(content);

					foreach (KeyValuePair<string, JSONNode> jsonDataKeyValuePair in jsonNode)
						Set(jsonDataKeyValuePair.Key, jsonDataKeyValuePair.Value);
				}

				_ready = true;
			}


			[ContextMenu(nameof(Save))]
			public void Save()
			{
				HashSet<string> processedKeys = new();
				JSONNode temporalJsonNode = null;
				string persistentDataPath = GetOptimizedPersistentDataPath();
				if (!Directory.CreateDirectory(persistentDataPath).Exists) return;

				// Overrides
				foreach (SFilePathOverride savableOverride in _overrides) {
					temporalJsonNode = JSONNode.New();
					foreach (string key in savableOverride.Keys) {
						if (_data.ContainsKey(key))
							temporalJsonNode.Add(key, _data[key]);
						processedKeys.Add(key);
					}
					File.WriteAllText(Path.Combine(persistentDataPath, savableOverride.FilePath), temporalJsonNode.ToString(_useCompactData));
				}

				// Default
				temporalJsonNode = JSONNode.New();
				foreach (KeyValuePair<string, JSONNode> keyValuePair in _data.Where(dataEntryPair => !processedKeys.Contains(dataEntryPair.Key))) {
					temporalJsonNode.Add(keyValuePair.Key, keyValuePair.Value);
				}
				File.WriteAllText(Path.Combine(persistentDataPath, _defaultFilePath), temporalJsonNode.ToString(_useCompactData));
			}


			[ContextMenu(nameof(SaveReload))]
			public void SaveReload()
			{
				Save();
				Load();
			}


		#endregion




		#region Publics - Data


			public bool Get(string key, out JSONNode json)
			{
				if (!_ready) Load();
				return _data.TryGetValue(key, out json);
			}

			public void Set(string key, JSONNode json)
			{
				_data.AddOrSet(key, json);
				Publish(key, json);
			}


		#endregion




		#region Publics - Events


			public void Subscribe(string key, Action<JSONNode> handler)
			{
				if (_events.TryGetValue(key, out Action<JSONNode> current))
					_events[key] = current + handler;
				else
					_events[key] = handler;
			}

			public void Unsubscribe(string key, Action<JSONNode> handler)
			{
				if (_events.TryGetValue(key, out Action<JSONNode> current))
					_events[key] = current - handler;
			}


		#endregion




		#region Privates


			private void Publish(string key, JSONNode eventData)
			{
				if (_events.TryGetValue(key, out Action<JSONNode> current))
					current?.Invoke(eventData);
			}


		#endregion




		#region Properties


			private SavedataSettings Settings => SavedataSettings.Instance;

			public bool Ready => _ready;
			public bool UseCompactData => _useCompactData;
			public string DefaultFilePath => _defaultFilePath;
			public string OptimizedPersistentDataPath => GetOptimizedPersistentDataPath();
			public Dictionary<string, JSONNode> Data => _data;

			public IEnumerable<string> FilePaths => _overrides.Select(savable => savable.FilePath).Prepend(_defaultFilePath);


		#endregion
	}
}


#endif




/*       ________________________________________________________________       */
/*           _________   _______ ________  _______  _______  ___    _           */
/*           |        \ |______/ |______| |  _____ |       | |  \   |           */
/*           |________/ |     \_ |      | |______| |_______| |   \__|           */
/*           ______ _____ _____ _____ __   _ _____ __   _ _____ _____           */
/*           |____/ |____ [___  |   | | \  | |___| | \  | |     |____           */
/*           |    \ |____ ____] |___| |  \_| |   | |  \_| |____ |____           */
/*       ________________________________________________________________       */
/*                                                                              */
/*           David Tabernero M.  <https://github.com/davidtabernerom>           */
/*           Dragon Resonance    <https://github.com/dragonresonance>           */
/*                  Copyright © 2021-2026. All rights reserved.                 */
/*                Licensed under the Apache License, Version 2.0.               */
/*                         See LICENSE.md for more info.                        */
/*       ________________________________________________________________       */
/*                                                                              */