#if UNITASK


using Cysharp.Threading.Tasks;
using DragonResonance.Logging;
using PossumScream.Databases;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using UnityEngine.Events;
using UnityEngine.Scripting;
using UnityEngine;


namespace DragonResonance.Localizer
{
	[Preserve]
	public partial class Localizer
	{
		private static LocalizerSettings _settings => LocalizerSettings.Instance;
		private static readonly UniTaskCompletionSource _resourceFetching = new();
		private static readonly DynamicSheet<string> _dataSheet = new();

		public static event Action OnLanguageChange = null;


		#region Events

			[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
			private static void Initialize() => OnStartup();

			private static async void OnStartup()
			{
				await foreach (string fileContent in FetchResources())
					_dataSheet.TryJoinTSV(fileContent);
				_resourceFetching.TrySetResult();
			}

		#endregion


		#region Publics

			public void ChangeLanguage(SystemLanguage language)
			{
				_settings.CurrentLanguage = language;
				OnLanguageChange?.Invoke();
			}


			public static async UniTaskVoid Localize(string rawText, UnityEvent<string> handler)
			{
				await _resourceFetching.Task;
				handler.Invoke(await Localize(rawText));
			}

			public static async UniTask<string> Localize(string rawText)
			{
				await _resourceFetching.Task;
				foreach (string key in GetKeys(rawText)) {
					string language = _settings.CurrentLanguage.ToString();
					try {
						string value = _dataSheet[key, language];
						//Log($"key:{key}, value:{value}, language:{language}");
						rawText = rawText.Replace($"{{{key}}}", value);
					}
					catch (ArgumentOutOfRangeException) {
						HLogger.LogError($"Key {key} ({language}) not found");
					}
				}
				return rawText;
			}

		#endregion


		#region Privates

			private static IEnumerable<string> GetKeys(string rawText) =>
				Regex.Matches(rawText, @"\{(\w+)\}").Select(match => match.Groups[1].Value);

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