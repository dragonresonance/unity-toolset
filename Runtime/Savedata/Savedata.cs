#if SIMPLEJSON


using Cysharp.Threading.Tasks;
using DragonResonance.Behaviours;
using DragonResonance.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System;
using Tabernero.SimpleJSON;
using UnityEngine.Scripting;
using UnityEngine;


namespace DragonResonance.Savedata
{
	[Preserve]
	public partial class Savedata : PersistentSingletonPossumBehaviour<Savedata>
	{
		private static SavedataSettings _settings = null;
		private static readonly Dictionary<string, JSONNode> _data = new();
		private static readonly Dictionary<string, Action<JSONNode>> _events = new();
		private static readonly UniTaskCompletionSource _starting = new();
		private static readonly UniTaskCompletionSource _loading = new();
		private static readonly SemaphoreSlim _saveSemaphore = new(1, 1);


		#region Events

			[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
			private static void Initialize() => OnStartup();

			private static async void OnStartup()
			{
				Logging.Log.Info("Starting up...");
				_settings = await SavedataSettings.GetInstanceAsync();
				_starting.TrySetResult();
				Logging.Log.Info("Started!");

				if (_settings.LoadOnStart)
					await Load();
			}

		#endregion


		#region Publics - Files

			public static async UniTask Load()
			{
				Logging.Log.Info("Loading...");
				await _starting.Task;

				_data.Clear();
				foreach (string filePath in Savedata.FilePaths) {
					string dataFilePath = GetOptimizedPersistentDataPath(filePath);
					if (!File.Exists(dataFilePath)) continue;

					string content = await File.ReadAllTextAsync(dataFilePath, Encoding.UTF8);
					JSONNode jsonNode = JSONNode.Parse(content);

					foreach (KeyValuePair<string, JSONNode> jsonDataKeyValuePair in jsonNode)
						Set(jsonDataKeyValuePair.Key, jsonDataKeyValuePair.Value);
				}

				_loading.TrySetResult();
				Logging.Log.Info("Loaded!");
			}


			public static async UniTask Save()
			{
				//await _saveSemaphore.WaitAsync();
				if (!await _saveSemaphore.WaitAsync(_settings.ThreadTimeoutMilliseconds)) return;
				try {
					Logging.Log.Info("Saving...");
					await _loading.Task;

					HashSet<string> processedKeys = new();
					JSONNode temporalJsonNode = null;
					string persistentDataPath = GetOptimizedPersistentDataPath();
					if (!Directory.CreateDirectory(persistentDataPath).Exists) return;

					// Overrides
					foreach (SFilePathOverride savableOverride in _settings.Overrides) {
						temporalJsonNode = JSONNode.New();
						foreach (string key in savableOverride.Keys) {
							if (_data.ContainsKey(key))
								temporalJsonNode.Add(key, _data[key]);
							processedKeys.Add(key);
						}
						await File.WriteAllTextAsync(
							Path.Combine(persistentDataPath, savableOverride.FilePath),
							temporalJsonNode.ToString(_settings.UseCompactData));
					}

					// Default
					temporalJsonNode = JSONNode.New();
					foreach (KeyValuePair<string, JSONNode> keyValuePair in _data.Where(dataEntryPair => !processedKeys.Contains(dataEntryPair.Key))) {
						temporalJsonNode.Add(keyValuePair.Key, keyValuePair.Value);
					}
					await File.WriteAllTextAsync(
						Path.Combine(persistentDataPath, _settings.DefaultFilePath),
						temporalJsonNode.ToString(_settings.UseCompactData));
				}
				finally {
					_saveSemaphore.Release();
				}

				Logging.Log.Info("Saved!");
			}


			[ContextMenu(nameof(SaveReload))]
			public static async UniTask SaveReload()
			{
				await Save();
				await Load();
			}

		#endregion


		#region Publics - Data

			public static bool Get(string key, out JSONNode json)
			{
				return _data.TryGetValue(key, out json);
			}

			public static void Set(string key, JSONNode json)
			{
				_data.AddOrSet(key, json);
				Publish(key, json);
			}

		#endregion


		#region Publics - Events

			public static void Subscribe(string key, Action<JSONNode> handler)
			{
				if (_events.TryGetValue(key, out Action<JSONNode> current))
					_events[key] = current + handler;
				else
					_events[key] = handler;
			}

			public static void Unsubscribe(string key, Action<JSONNode> handler)
			{
				if (_events.TryGetValue(key, out Action<JSONNode> current))
					_events[key] = current - handler;
			}

		#endregion


		#region Privates

			private static void Publish(string key, JSONNode eventData)
			{
				if (_events.TryGetValue(key, out Action<JSONNode> current))
					current?.Invoke(eventData);
			}

		#endregion


		#region Properties

			public static Dictionary<string, JSONNode> Data => _data;
			public static Dictionary<string, Action<JSONNode>> Events => _events;

			public static UniTaskCompletionSource Starting => _starting;
			public static UniTaskCompletionSource Loading => _loading;

			public static IEnumerable<string> FilePaths => _settings.Overrides
				.Select(savable => savable.FilePath)
				.Prepend(_settings.DefaultFilePath);

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