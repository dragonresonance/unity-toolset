#if UNITASK


using Cysharp.Threading.Tasks;
using DragonResonance.Extensions;
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

			[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
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

			public static void Play(SAudioSourceConfig audioSourceConfig) =>
				PlayAndAwait(audioSourceConfig).Forget();

			public static void Play(SAudioSourceConfig audioSourceConfig, AudioSource audioSource) =>
				SetupPooledAudioSource(audioSourceConfig, audioSource);


			public static async UniTask PlayAndAwait(SAudioSourceConfig audioSourceConfig)
			{
				await _starting.Task;
				AudioSource audioSource = SetupPooledAudioSource(audioSourceConfig);
				await SounderPool.Current.ReleaseWhenDoneAsync(audioSource);
			}

			public static async UniTask PlayAndAwait(SAudioSourceConfig audioSourceConfig, AudioSource audioSource)
			{
				SetupPooledAudioSource(audioSourceConfig, audioSource);
				await UniTask.WaitUntil(audioSource.IsStopped);
			}


			public static async UniTask<AudioSource> PlayAndGet(SAudioSourceConfig audioSourceConfig)
			{	// ReSharper disable MethodHasAsyncOverload
				await _starting.Task;
				AudioSource audioSource = SetupPooledAudioSource(audioSourceConfig);
				SounderPool.Current.ReleaseWhenDone(audioSource);
				return audioSource;
			}	// ReSharper restore MethodHasAsyncOverload


			public static void Stop(AudioSource audioSource) => audioSource.Stop();
			public static void Release(AudioSource audioSource) => SounderPool.Current.Release(audioSource);

		#endregion


		#region Privates

			private static AudioSource SetupPooledAudioSource(SAudioSourceConfig audioSourceConfig) =>
				SetupPooledAudioSource(audioSourceConfig, SounderPool.Instance.Get());
			private static AudioSource SetupPooledAudioSource(SAudioSourceConfig audioSourceConfig, AudioSource audioSource)
			{
				audioSource.resource = audioSourceConfig.AudioResource;
				audioSource.outputAudioMixerGroup = audioSourceConfig.AudioMixerGroup;
				audioSource.Play();
				return audioSource;
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