#if SIMPLEJSON


using System.Collections.Generic;
using System;
using Tabernero.SimpleJSON;
using UnityEngine;


namespace DragonResonance.Savedata
{
	public partial class Savedata
	{
		private readonly Dictionary<Delegate, Action<JSONNode>> _wrappers = new();


		#region Publics - Data

			public bool Get<T>(out T data, T fallback = default) where T : struct, ISavableData
			{
				data = fallback;

				if (Get(fallback.Key, out JSONNode json)) {
					data = JsonUtility.FromJson<T>(json.ToString());
					return true;
				}

				return false;
			}

			public void Set<T>(T data) where T : struct, ISavableData
			{
				Set(data.Key, JSONNode.Parse(JsonUtility.ToJson(data)));
			}

		#endregion


		#region Publics - Events

			public void SubscribeAndReload<T>(Action<T> handler) where T : struct, ISavableData => SubscribeAndReload(typeof(T).Name, handler);
			public void SubscribeAndReload<T>(string key, Action<T> handler) where T : struct, ISavableData
			{
				Subscribe(key, handler);
				if (Get(out T data))
					Set(data);
			}

			public void Subscribe<T>(Action<T> handler) where T : struct, ISavableData => Subscribe(typeof(T).Name, handler);
			public void Subscribe<T>(string key, Action<T> handler) where T : struct, ISavableData
			{
				void Wrapper(JSONNode json) => handler.Invoke(JsonUtility.FromJson<T>(json.ToString()));

				_wrappers[handler] = Wrapper;

				if (_events.TryGetValue(key, out Action<JSONNode> current))
					_events[key] = current + Wrapper;
				else
					_events[key] = Wrapper;
			}

			public void Unsubscribe<T>(Action<T> handler) where T : struct, ISavableData => Unsubscribe(typeof(T).Name, handler);
			public void Unsubscribe<T>(string key, Action<T> handler) where T : struct, ISavableData
			{
				if (_events.TryGetValue(key, out Action<JSONNode> current)) {
					if (_wrappers.TryGetValue(handler, out Action<JSONNode> wrapper))
						_events[key] = current - wrapper;

					_wrappers.Remove(handler);
				}
			}

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