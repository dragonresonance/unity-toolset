#if UNITASK


using Cysharp.Threading.Tasks;
using UnityEngine.Audio;
using UnityEngine.Scripting;
using UnityEngine;


namespace DragonResonance.Sounder
{
	[Preserve]
	public class Sounder
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

			public static void PlayOnce(AudioResource resource, AudioMixerGroup mixerGroup) => PlayOnceAsync(resource, mixerGroup).Forget();
			public static async UniTask PlayOnceAsync(AudioResource resource, AudioMixerGroup mixerGroup)
			{
				await _starting.Task;
				AudioSource audioSource = SounderPool.Current.Get().GetComponent<AudioSource>();
				audioSource.resource = resource;
				audioSource.outputAudioMixerGroup = mixerGroup;
				audioSource.Play();
				await SounderPool.Current.ReleaseWhenDoneAsync(audioSource);
			}

		#endregion


		#region Privates

			private static void Test()
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