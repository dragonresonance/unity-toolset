#if UNITY_EDITOR


using DragonResonance.Extensions;
using DragonResonance.Localizer;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;


namespace DragonResonance.Editor.Settings
{
	public class LocalizerSettingsProvider : SettingsProvider
	{
		private const string SettingsPath = "Project/Dragon Resonance/Localizer";
		private const int LargePadding = 24;
		private const int SmallPadding = 12;


		private static LocalizerSettings _settings;
		private static SerializedObject _serializedScriptableObject;


		#region Constructors

			[SettingsProvider]
			public static SettingsProvider Create()
			{
				string[] guids = AssetDatabase.FindAssets($"t:{nameof(LocalizerSettings)}");
				if (guids.Length > 0) {
					string path = AssetDatabase.GUIDToAssetPath(guids[0]);
					_settings = AssetDatabase.LoadAssetAtPath<LocalizerSettings>(path);
				}
				else {
					_settings = ScriptableObject.CreateInstance<LocalizerSettings>();
					AssetDatabase.CreateAsset(_settings, $"Assets/LocalizerSettings.asset");
					AssetDatabase.SaveAssets();

					// Add it to PreloadedAssets so they load ASAP on builds
					List<UnityObject> preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
					preloadedAssets.AddOrIgnore(_settings);
					PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
				}

				_serializedScriptableObject = new SerializedObject(_settings);

				return new LocalizerSettingsProvider(SettingsPath, SettingsScope.Project);
			}

			public LocalizerSettingsProvider(string path, SettingsScope scope) : base(path, scope) { }

		#endregion


		#region Publics

			public override void OnGUI(string searchContext)
			{
				GUIStyle paddedSection = new() { padding = new RectOffset(LargePadding, LargePadding, SmallPadding, SmallPadding) };
				EditorGUILayout.BeginVertical(paddedSection);
				{
					_serializedScriptableObject.Update();

					SerializedProperty property = _serializedScriptableObject.GetIterator();
					if (property.NextVisible(true)) {
						do {
							if (property.name == "m_Script") continue;
							EditorGUILayout.PropertyField(property, true);
						}
						while (property.NextVisible(false));
					}

					_serializedScriptableObject.ApplyModifiedProperties();
				}
				{
					GUILayout.Height(SmallPadding);
					EditorGUILayout.LabelField("Data", EditorStyles.boldLabel);
					EditorGUI.BeginDisabledGroup(true);
					{
						foreach (List<string> rows in Localizer.Localizer.DataSheet.Data)
							EditorGUILayout.TextField(string.Join("\t", rows));
					}
					EditorGUI.EndDisabledGroup();
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