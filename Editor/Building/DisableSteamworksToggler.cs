#if UNITY_EDITOR && STEAMWORKS_INTEGRATION


using UnityEditor;


namespace DragonResonance.Editor.Building
{
	[InitializeOnLoad]
	public static class DisableSteamworksToggler
	{
		private const string DISABLE_STEAMWORKS_DEFINE = "_DISABLESTEAMWORKS";


		#region Constructors

			static DisableSteamworksToggler() => BuildDefines.SetupBuildDefinition(DISABLE_STEAMWORKS_DEFINE, false);

		#endregion


		#region Publics

			#if !DISABLESTEAMWORKS
				[MenuItem("Integration/Steamworks [ON]/Disable integration")]
			#else
				[MenuItem("Integration/Steamworks [OFF]/Enable integration")]
			#endif
			public static void SwitchLogging() => BuildDefines.ToggleBuildDefinition(DISABLE_STEAMWORKS_DEFINE);

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