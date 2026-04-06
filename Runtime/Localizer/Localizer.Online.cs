#if UNITASK && ENABLE_UNITYWEBREQUEST


using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks;
using DragonResonance.Logging;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine.Networking;
using UnityEngine;


namespace DragonResonance.Localizer
{
	public partial class Localizer	// Online
	{
		#region Publics

			public static async UniTask RetrieveOnlineData()
			{
				//	Fetch online Resource assets
				#if UNITY_EDITOR	// Only during development
					static IUniTaskAsyncEnumerable<(TextAsset, string)> FetchResourceSources() =>
						FetchSources(_settings.ResourceSources,
							GetUrl: resourceSource => resourceSource.Url,
							GetSource: resourceSource => resourceSource.FileAsset);

					await foreach ((TextAsset, string) source in FetchResourceSources())
						await File.WriteAllTextAsync(
							UnityEditor.AssetDatabase.GetAssetPath(source.Item1),
							contents:source.Item2);

					UnityEditor.AssetDatabase.Refresh();
				#endif

				//	Fetch online Streaming assets
				static IUniTaskAsyncEnumerable<(string, string)> FetchStreamingSources() =>
					FetchSources(_settings.StreamingSources,
						GetUrl: streamingSource => streamingSource.Url,
						GetSource: streamingSource => streamingSource.FilePath);

				await foreach ((string, string) source in FetchStreamingSources())
					await File.WriteAllTextAsync(
						Path.Join(Application.streamingAssetsPath, source.Item1),
						contents:source.Item2);
			}

		#endregion


		#region Privates

			private static IUniTaskAsyncEnumerable<(T, string)> FetchSources<TSource, T>(
				IEnumerable<TSource> sources,
				Func<TSource, string> GetUrl,
				Func<TSource, T> GetSource)
			{
				return UniTaskAsyncEnumerable.Create<(T, string)>(async (writer, token) =>
				{
					foreach (TSource source in sources) {
						Log.Info($"Retrieving source {source} ...");
						using UnityWebRequest request = UnityWebRequest.Get(GetUrl(source));

						try {
							await request.SendWebRequest();
							if (request.result == UnityWebRequest.Result.Success)
								await writer.YieldAsync((GetSource(source), request.downloadHandler.text));
							else
								Log.Error($"Error {request.result} fetching the resource \"{source}\"");
						}
						catch (Exception exception) {
							Log.Exception(exception, $"Exception fetching source {source}");
						}
					}
				});
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