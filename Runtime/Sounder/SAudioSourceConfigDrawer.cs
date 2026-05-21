#if UNITY_EDITOR


using UnityEditor;
using UnityEngine;


namespace DragonResonance.Sounder
{
	[CustomPropertyDrawer(typeof(SAudioSourceConfig))]
	public class SAudioSourceConfigDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			{
				SerializedProperty audioResourceProperty = property.FindPropertyRelative(nameof(SAudioSourceConfig.AudioResource));
				SerializedProperty audioMixerGroupProperty = property.FindPropertyRelative(nameof(SAudioSourceConfig.AudioMixerGroup));

				position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

				float halfWidth = position.width / 2f;
				Rect audioResourcePropertyRect = new(position.x, position.y, halfWidth - 2, EditorGUIUtility.singleLineHeight);
				Rect audioMixerGroupPropertyRect = new(position.x + halfWidth + 2, position.y, halfWidth - 2, EditorGUIUtility.singleLineHeight);

				EditorGUI.PropertyField(audioResourcePropertyRect, audioResourceProperty, GUIContent.none);
				EditorGUI.PropertyField(audioMixerGroupPropertyRect, audioMixerGroupProperty, GUIContent.none);
			}
			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUIUtility.singleLineHeight;
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