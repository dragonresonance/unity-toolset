#if UNITASK


using Cysharp.Threading.Tasks;
using DragonResonance.Databases;
using DragonResonance.Extensions;
using DragonResonance.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System;
using UnityEngine.Events;
using UnityEngine.Scripting;
using UnityEngine;


namespace DragonResonance.Sounder
{
	[Preserve]
	public partial class Sounder
	{
		private static SounderSettings _settings = null;
		private static readonly UniTaskCompletionSource _starting = new();


		#region Events

			[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
			private static void Initialize() => OnStartup();

			private static async void OnStartup()
			{
				Logging.Log.Info("Starting up...");

				_settings = await SounderSettings.GetInstanceAsync();
				_starting.TrySetResult();

				Logging.Log.Info("Started!");
			}

		#endregion


		#region Publics

			public static async UniTaskVoid Localize(string template)
			{
				await _starting.Task;
				//
			}

		#endregion


		#region Privates

			private static void Test(SystemLanguage fallback)
			{
				//
			}

		#endregion


		#region Properties

			public static UniTaskCompletionSource Starting => _starting;

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