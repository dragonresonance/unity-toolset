using System.IO;
using System;
using UnityEngine;


namespace DragonResonance.Savedata
{
	public partial class Savedata
	{
		#region Publics

			public string GetOptimizedPersistentDataPath() => GetOptimizedPersistentDataPath(".");
			public string GetOptimizedPersistentDataPath(string path) => GetOptimizedPersistentDataPath(".", path);
			public string GetOptimizedPersistentDataPath(string path, string filename)
			{
				string optimizedPersistentDataPath = Application.persistentDataPath;

				#if UNITY_STANDALONE_WIN
					optimizedPersistentDataPath = Path.Combine(
						Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
						Application.companyName, Application.productName);
				#elif UNITY_STANDALONE_LINUX
					optimizedPersistentDataPath = Path.Combine(
						Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
						Application.companyName, Application.productName);
				#elif UNITY_STANDALONE_OSX
					optimizedPersistentDataPath = Path.Combine(
						Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
						Application.companyName, Application.productName);
				#endif

				return Path.GetFullPath(Path.Combine(optimizedPersistentDataPath, path, filename));
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