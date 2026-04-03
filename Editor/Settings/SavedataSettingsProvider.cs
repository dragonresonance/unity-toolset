#if UNITY_EDITOR


using DragonResonance.Savedata;
using UnityEditor;
using UnityEngine;


namespace DragonResonance.Editor.Settings
{
	public class SavedataSettingsProvider : SettingsProvider
	{
		private const string SettingsPath = "Project/Dragon Resonance/Savedata";
		private const int LargePadding = 24;
		private const int SmallPadding = 12;


		private static SavedataSettings _settings;


		#region Constructors

			[SettingsProvider]
			public static SettingsProvider Create()
			{
				string[] guids = AssetDatabase.FindAssets($"t:{nameof(SavedataSettings)}");

				if (guids.Length > 0) {
					string path = AssetDatabase.GUIDToAssetPath(guids[0]);
					_settings = AssetDatabase.LoadAssetAtPath<SavedataSettings>(path);
				}
				else {
					_settings = ScriptableObject.CreateInstance<SavedataSettings>();
					AssetDatabase.CreateAsset(_settings, $"Assets/SavedataSettings.asset");
					AssetDatabase.SaveAssets();
				}

				return new SavedataSettingsProvider(SettingsPath, SettingsScope.Project);
			}

			public SavedataSettingsProvider(string path, SettingsScope scope) : base(path, scope) { }

		#endregion


		#region Publics

			public override void OnGUI(string searchContext)
			{
				GUIStyle paddedSection = new() { padding = new RectOffset(LargePadding, LargePadding, SmallPadding, SmallPadding) };
				EditorGUILayout.BeginVertical(paddedSection);
				{
					SerializedObject serializedScriptableObject = new(_settings);
					serializedScriptableObject.Update();

					SerializedProperty property = serializedScriptableObject.GetIterator();
					if (property.NextVisible(true)) {
						do {
							if (property.name == "m_Script") continue;
							EditorGUILayout.PropertyField(property, true);
						}
						while (property.NextVisible(false));
					}

					serializedScriptableObject.ApplyModifiedProperties();
				}
				EditorGUILayout.EndVertical();
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