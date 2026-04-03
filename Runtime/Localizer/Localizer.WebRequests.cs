#if UNITASK


using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;


namespace DragonResonance.Localizer
{
	public partial class Localizer	// WebRequests
	{
		#region Privates

			private IUniTaskAsyncEnumerable<string> FetchResources()
			{
				return UniTaskAsyncEnumerable.Create<string>(async (writer, token) =>
				{
					foreach (string source in _onlineSources) {
						using UnityWebRequest request = UnityWebRequest.Get(source);
						await request.SendWebRequest().WithCancellation(token);

						if (request.result != UnityWebRequest.Result.Success) {
							Error($"Error {request.result} fetching the resource \"{source}\"");
							continue;
						}

						await writer.YieldAsync(request.downloadHandler.text);
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