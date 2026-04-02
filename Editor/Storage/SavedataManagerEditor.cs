#if UNITY_EDITOR && SIMPLEJSON


using System.Collections.Generic;
using Tabernero.SimpleJSON;
using UnityEditor;
using UnityEngine;


namespace DragonResonance.Storage
{
	[CustomEditor(typeof(SavedataManager))]
	public class SavedataManagerEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			SavedataManager savedataManager = (SavedataManager)base.target;

			EditorGUILayout.Separator();
			EditorGUILayout.LabelField("Controls", EditorStyles.boldLabel);
			if (GUILayout.Button("Open Save Directory"))
				EditorUtility.RevealInFinder(savedataManager.OptimizedPersistentDataPath);
			if (GUILayout.Button(nameof(SavedataManager.Load)))
				savedataManager.Load();
			if (GUILayout.Button(nameof(SavedataManager.Save)))
				savedataManager.Save();
			if (GUILayout.Button("Save and Reload"))
				savedataManager.SaveReload();

			EditorGUILayout.Separator();
			EditorGUILayout.LabelField("Data", EditorStyles.boldLabel);
			EditorGUILayout.TextField($"Current Save Directory", savedataManager.OptimizedPersistentDataPath);
			foreach (KeyValuePair<string, JSONNode> dataKeyValuePair in savedataManager.Data) {
				EditorGUILayout.LabelField(dataKeyValuePair.Key, EditorStyles.miniLabel);
				EditorGUILayout.TextArea(dataKeyValuePair.Value.ToString(savedataManager.UseCompactData));
			}
		}
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