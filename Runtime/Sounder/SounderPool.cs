using Cysharp.Threading.Tasks;
using DragonResonance.Behaviours;
using System.Collections.Generic;
using UnityEngine.Pool;
using UnityEngine;


namespace DragonResonance.Sounder
{
	public class SounderPool : PersistentSingletonPossumBehaviour<SounderPool>
	{
		private const string POOLED_AUDIOSOURCE_NAME = "PooledAudioSource";


		private ObjectPool<AudioSource> _pool = null;


		#region Events

			protected override async void LateAwake()
			{
				SounderSettings settings = await SounderSettings.GetInstanceAsync();
				Log($"Generating {settings.StartingPoolAmount} items...");
				_pool = new ObjectPool<AudioSource>(
					createFunc: CreateItem,
					actionOnGet: OnItemGet,
					actionOnRelease: OnItemRelease,
					actionOnDestroy: OnItemDestroy,
					defaultCapacity: settings.StartingPoolSize,
					maxSize: settings.MaxPoolSize,
					collectionCheck: false
				);
				Populate(settings.StartingPoolAmount);
			}

		#endregion


		#region Publics

			public AudioSource Get() => _pool.Get();
			public void Release(AudioSource item) => _pool.Release(item);

			public void ReleaseWhenDone(AudioSource item) => ReleaseWhenDoneAsync(item).Forget();
			public async UniTask ReleaseWhenDoneAsync(AudioSource audioSourceItem)
			{
				await UniTask.WaitWhile(() => audioSourceItem.isPlaying, cancellationToken: this.GetCancellationTokenOnDestroy());
				Release(audioSourceItem);
			}

		#endregion


		#region Privates

			private void Populate(int amount)
			{
				Queue<AudioSource> populatedItems = new(amount);
				for (int itemIndex = 0; itemIndex < amount; itemIndex++)
					populatedItems.Enqueue(_pool.Get());
				while (populatedItems.TryDequeue(out AudioSource item))
					_pool.Release(item);
			}

			private AudioSource CreateItem()
			{
				GameObject gameObject = new(POOLED_AUDIOSOURCE_NAME);
				gameObject.transform.SetParent(this.transform);

				AudioSource audioSource = gameObject.AddComponent<AudioSource>();
				audioSource.enabled = false;
				audioSource.playOnAwake = false;

				UpdatePooledAudioSourceName(audioSource);
				return audioSource;
			}

			private void OnItemGet(AudioSource audioSource)
			{
				audioSource.enabled = true;
				UpdatePooledAudioSourceName(audioSource);
			}

			private void OnItemRelease(AudioSource audioSource)
			{
				audioSource.enabled = false;
				UpdatePooledAudioSourceName(audioSource);
			}

			private void OnItemDestroy(AudioSource audioSource)
			{
				DestroyDynamically(audioSource.gameObject);
			}

			private void UpdatePooledAudioSourceName(AudioSource audioSource)
			{
				audioSource.name = $"{POOLED_AUDIOSOURCE_NAME} ({(audioSource.enabled ? "Playing" : "Idle")})";
			}

		#endregion
	}
}


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