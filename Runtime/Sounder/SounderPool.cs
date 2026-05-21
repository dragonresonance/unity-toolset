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


		private ObjectPool<GameObject> _pool = null;


		#region Events

			protected new async void Awake()
			{
				base.Awake();
				SounderSettings settings = await SounderSettings.GetInstanceAsync();
				Log($"Generating {settings.StartingPoolSize} items...");
				_pool = new ObjectPool<GameObject>(
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

			public GameObject Get() => _pool.Get();
			public void Release(AudioSource source) => _pool.Release(source.gameObject);

			public void ReleaseWhenDone(AudioSource source) => ReleaseWhenDoneAsync(source).Forget();
			public async UniTask ReleaseWhenDoneAsync(AudioSource source)
			{
				await UniTask.WaitWhile(() => source.isPlaying, cancellationToken: this.GetCancellationTokenOnDestroy());
				_pool.Release(source.gameObject);
			}

		#endregion


		#region Privates

			private void Populate(int amount)
			{
				Queue<GameObject> populatedItems = new(amount);
				for (int itemIndex = 0; itemIndex < amount; itemIndex++)
					populatedItems.Enqueue(_pool.Get());
				while (populatedItems.TryDequeue(out GameObject item))
					_pool.Release(item);
			}

			private GameObject CreateItem()
			{
				GameObject gameObject = new(POOLED_AUDIOSOURCE_NAME);
				gameObject.transform.SetParent(this.transform);

				AudioSource audioSource = gameObject.AddComponent<AudioSource>();
				audioSource.enabled = false;
				audioSource.playOnAwake = false;

				UpdatePooledAudioSourceName(audioSource);
				return gameObject;
			}

			private void OnItemGet(GameObject gameObject)
			{
				AudioSource audioSource = gameObject.GetComponent<AudioSource>();
				audioSource.enabled = true;
				UpdatePooledAudioSourceName(audioSource);
			}

			private void OnItemRelease(GameObject gameObject)
			{
				AudioSource audioSource = gameObject.GetComponent<AudioSource>();
				audioSource.enabled = false;
				UpdatePooledAudioSourceName(audioSource);
			}

			private void OnItemDestroy(GameObject gameObject)
			{
				DestroyDynamically(gameObject);
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