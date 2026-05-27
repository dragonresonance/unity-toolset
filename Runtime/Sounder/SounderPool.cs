using Cysharp.Threading.Tasks;
using DragonResonance.Behaviours;
using DragonResonance.Extensions;
using System.Collections.Generic;
using UnityEngine.Pool;
using UnityEngine;


namespace DragonResonance.Sounder
{
	public class SounderPool : PersistentSingletonPossumBehaviour<SounderPool>
	{
		private const string POOLED_AUDIOSOURCE_NAME = "PooledAudioSource";

		[SerializeField] private SounderSettings _settings = null;

		private ObjectPool<AudioSource> _pool = null;


		#region Events

			protected override void LateAwake() => InitializePool();

		#endregion


		#region Publics

			public AudioSource Get() => this.Pool.Get();
			public void Release(AudioSource item) => this.Pool.Release(item);

			public void ReleaseWhenDone(AudioSource item) => ReleaseWhenDoneAsync(item).Forget();
			public async UniTask ReleaseWhenDoneAsync(AudioSource audioSourceItem)
			{
				if (audioSourceItem.resource is AudioClip)
					await UniTask.WaitUntil(audioSourceItem.HasAudioClipStopped);
				else
					await UniTask.WaitUntil(audioSourceItem.HasAudioResourceStopped);
				Release(audioSourceItem);
			}

		#endregion


		#region Privates

			private void InitializePool()
			{
				if (_pool != null) return;
				Log($"Generating {_settings.StartingPoolAmount} items...");
				_pool = new ObjectPool<AudioSource>(
					createFunc: CreateItem,
					actionOnGet: OnItemGet,
					actionOnRelease: OnItemRelease,
					actionOnDestroy: OnItemDestroy,
					defaultCapacity: _settings.StartingPoolSize,
					maxSize: _settings.MaxPoolSize,
					collectionCheck: false
				);
				Populate(_settings.StartingPoolAmount);
			}


			private void Populate(int amount)
			{
				Queue<AudioSource> populatedItems = new(amount);
				for (int itemIndex = 0; itemIndex < amount; itemIndex++)
					populatedItems.Enqueue(this.Pool.Get());
				while (populatedItems.TryDequeue(out AudioSource item))
					this.Pool.Release(item);
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


		#region Properties

			public ObjectPool<AudioSource> Pool
			{
				get {
					InitializePool();
					return _pool;
				}
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