#if UNITASK


using Cysharp.Threading.Tasks;
using DragonResonance.Extensions;
using DragonResonance.Logging;
using PossumScream.Databases;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
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

		private static SystemLanguage _currentLanguage = SystemLanguage.Unknown;
		private static readonly UniTaskCompletionSource _loading = new();
		private static readonly HeaderedSheet<string> _dataSheet = new();

		public static event Action OnLanguageChange = null;


		#region Events

			[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
			private static void Initialize() => OnStartup();

			private static async void OnStartup()
			{
				#if ENABLE_UNITYWEBREQUEST && (UNITY_EDITOR || DEVELOPMENT_BUILD)
					await RetrieveOnlineData();
				#endif
				await LoadLocalData();
				_currentLanguage = FirstPreferredLanguage(_currentLanguage);
				_loading.TrySetResult();
			}

		#endregion


		#region Publics

			public static void ChangeLanguage(SystemLanguage language)
			{
				Log.Emphasis($"Language changed to {language}");
				_currentLanguage = language;
				OnLanguageChange?.Invoke();
			}


			public static async UniTaskVoid Localize(string template, UnityEvent<string> handler)
			{
				await _loading.Task;
				handler.Invoke(await Localize(template));
			}

			public static async UniTask<string> Localize(string template)
			{
				await _loading.Task;

				string language = _currentLanguage.ToString();
				foreach (string key in GetKeys(template)) {

					// Simple keys
					if (_dataSheet.TryGet(language, key, out string simpleValue) && !string.IsNullOrWhiteSpace(simpleValue)) {
						template = template.Replace($"{{{key}}}", simpleValue);
						continue;
					}

					// Composite keys
					int partialIndex = 1;
					if (_dataSheet.HeadingColumn.Contains($"{key}:{partialIndex}")) {
						StringBuilder compositeValueBuilder = new();
						while (_dataSheet.TryGet(language, $"{key}:{partialIndex++}", out string partialValue))
							compositeValueBuilder.AppendLine(partialValue);

						string compositeValue = compositeValueBuilder.ToString();
						if (!string.IsNullOrWhiteSpace(compositeValue)) {
							template = template.Replace($"{{{key}}}", compositeValue);
							continue;
						}
					}

					Log.Error($"Key {key} not found or empty in {language} language!");
				}
				return template;
			}

		#endregion


		#region Privates

			private static SystemLanguage FirstPreferredLanguage(SystemLanguage fallback)
			{
				SystemLanguage[] preferredLanguages = _settings.PreferredLanguages.ToArray();
				SystemLanguage[] availableLanguages = Localizer.AvailableLanguages.ToArray();
				return preferredLanguages.FirstMatchOrFallback(availableLanguages, fallback);
			}

			private static IEnumerable<string> GetKeys(string rawText) =>
				Regex.Matches(rawText, @"\{(\w+)\}").Select(match => match.Groups[1].Value);

		#endregion


		#region Properties

			public static SystemLanguage CurrentLanguage => _currentLanguage;
			public static UniTaskCompletionSource Loading => _loading;
			public static HeaderedSheet<string> DataSheet => _dataSheet;

			public static bool IsDefaultLanguage => (_currentLanguage == _settings.PreferredLanguages.First());
			public static IEnumerable<string> AvailableLanguageNames => Localizer.AvailableLanguages.Select(language => language.ToString());
			public static IEnumerable<SystemLanguage> AvailableLanguages => _dataSheet.HeadingRow
				.Select(cellString => (success: Enum.TryParse<SystemLanguage>(cellString, out var language), language))
				.Where(parsing => parsing.success)
				.Select(parsing => parsing.language);

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