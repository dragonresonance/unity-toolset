#if UNITY_EDITOR


using DragonResonance.Editor.Building;
using DragonResonance.Extensions;
using DragonResonance.Integration;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;


namespace DragonResonance.Editor.Settings
{
	public class IntegrationSettingsProvider : SettingsProvider
	{
		private const string SettingsPath = "Project/Dragon Resonance/Integration";
		private const int LargePadding = 24;
		private const int SmallPadding = 12;


		private static IntegrationSettings _settings;
		private static SerializedObject _serializedScriptableObject;


		#region Constructors

			[SettingsProvider]
			public static SettingsProvider Create()
			{
				string[] guids = AssetDatabase.FindAssets($"t:{nameof(IntegrationSettings)}");
				if (guids.Length.IsZero()) {
					_settings = ScriptableObject.CreateInstance<IntegrationSettings>();
					AssetDatabase.CreateAsset(_settings, $"Assets/{nameof(IntegrationSettings)}.asset");
					AssetDatabase.SaveAssets();
				}
				else {
					string path = AssetDatabase.GUIDToAssetPath(guids.First());
					_settings = AssetDatabase.LoadAssetAtPath<IntegrationSettings>(path);
				}

				PreloadedAssets.Add(_settings);
				_serializedScriptableObject = new SerializedObject(_settings);

				return new IntegrationSettingsProvider(SettingsPath, SettingsScope.Project);
			}

			public IntegrationSettingsProvider(string path, SettingsScope scope) : base(path, scope) { }

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