#if UNITASK


using Cysharp.Threading.Tasks;
using DragonResonance.Behaviours;
using PossumScream.Databases;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using UnityEngine.Events;
using UnityEngine;


namespace DragonResonance.Localization
{
	public partial class LocalizationManager : PersistentSingletonPossumBehaviour<LocalizationManager>
	{
		[SerializeField] private SystemLanguage _currentLanguage = SystemLanguage.English;
		[SerializeField] private string[] _onlineSources = { };


		private readonly UniTaskCompletionSource _resourceFetching = new();
		private readonly HeaderedSheet<string> _dataSheetV2 = new();
		private readonly DynamicSheet<string> _dataSheet = new();

		public static event Action OnLanguageChange = null;


		#region Events

			private new async void Awake()
			{
				base.Awake();
				await foreach (string fileContent in FetchResources())
					_dataSheet.TryJoinTSV(fileContent);
				_resourceFetching.TrySetResult();
			}

		#endregion


		#region Publics

			public void ChangeLanguage(SystemLanguage language)
			{
				_currentLanguage = language;
				OnLanguageChange?.Invoke();
			}


			public async UniTaskVoid Localize(string rawText, UnityEvent<string> handler)
			{
				await _resourceFetching.Task;
				handler.Invoke(await Localize(rawText));
			}

			public async UniTask<string> Localize(string rawText)
			{
				await _resourceFetching.Task;
				foreach (string key in GetKeys(rawText)) {
					string language = _currentLanguage.ToString();
					try {
						string value = _dataSheet[key, language];
						//Log($"key:{key}, value:{value}, language:{language}");
						rawText = rawText.Replace($"{{{key}}}", value);
					}
					catch (ArgumentOutOfRangeException) {
						Error($"Key {key} ({language}) not found");
					}
				}
				return rawText;
			}

		#endregion


		#region Privates

			private IEnumerable<string> GetKeys(string rawText) => Regex.Matches(rawText, @"\{(\w+)\}").Select(match => match.Groups[1].Value);

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