#if UNITASK


using Cysharp.Threading.Tasks;
using System.IO;
using UnityEngine;


namespace DragonResonance.Localizer
{
	public partial class Localizer	// Local
	{
		#region Publics

			public static async UniTask LoadLocalData()
			{
				foreach (SResourceSource source in _settings.ResourceSources)
					_dataSheet.JoinTSV(source.FileAsset.text);

				foreach (SStreamingSource source in _settings.StreamingSources)
					_dataSheet.JoinTSV(await File.ReadAllTextAsync(
						Path.Join(Application.streamingAssetsPath, source.FilePath)));
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