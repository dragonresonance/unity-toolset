#if UNITY_EDITOR


using UnityEditor;


namespace DragonResonance.Editor.Settings
{
	public class DragonResonanceEditorSettingsProvider : SettingsProvider
	{
		private const string SettingsPath = "Project/Dragon Resonance";


		private static DragonResonanceEditorSettings _settings;


		#region Constructors

			public DragonResonanceEditorSettingsProvider(string path, SettingsScope scope) : base(path, scope) { }

		#endregion


		#region Publics

			[SettingsProvider]
			public static SettingsProvider Create()
			{
				return new DragonResonanceEditorSettingsProvider(SettingsPath, SettingsScope.Project);
			}

			public override void OnGUI(string searchContext)
			{
				_settings = DragonResonanceEditorSettings.instance;
				EditorGUI.BeginChangeCheck();
				{
					_settings.EditorTestBool = EditorGUILayout.Toggle("Editor Test Bool", _settings.EditorTestBool);
					_settings.EditorTestString = EditorGUILayout.TextField("Editor Test String", _settings.EditorTestString);
				}
				if (EditorGUI.EndChangeCheck())
					_settings.Save();
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
/*                  Copyright © 2021-2025. All rights reserved.                 */
/*                Licensed under the Apache License, Version 2.0.               */
/*                         See LICENSE.md for more info.                        */
/*       ________________________________________________________________       */
/*                                                                              */