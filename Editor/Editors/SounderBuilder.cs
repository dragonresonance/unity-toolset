#if UNITY_EDITOR


using DragonResonance.Behaviours;
using DragonResonance.Extensions;
using DragonResonance.Sounder;
using System.IO;
using System.Linq;
using System;
using UnityEditor;
using UnityEngine.Audio;
using UnityEngine;


namespace DragonResonance.Editor.Editors
{
	[Obsolete]
	public class SounderBuilder : PossumBehaviour
	{
		#region Publics

			[ContextMenu(nameof(Rebuild))]
			public void Rebuild()
			{
				AudioMixerGroup[] audioMixerGroups = SounderSettings.CachedInstance.AudioMixer.FindMatchingGroups(string.Empty);

				base.DestroyChildren();
				foreach (AudioMixerGroup group in audioMixerGroups)
					InitializeAudioSource(group);
				DestroyUninitializedAudioSources();

				EditorUtility.SetDirty(base.gameObject);
			}

		#endregion


		#region Privates

			private AudioResource GetAudioResource(string name)
			{
				string[] guids = AssetDatabase.FindAssets($"{name} t:AudioResource", new[] { "Assets" });

				if (!guids.Length.IsZero()) {
					string path = AssetDatabase.GUIDToAssetPath(guids.First());
					string assetName = Path.GetFileNameWithoutExtension(path);
					if (assetName == name)
						return AssetDatabase.LoadAssetAtPath<AudioResource>(path);
				}

				return null;
			}

			private void InitializeAudioSource(AudioMixerGroup audioMixerGroup)
			{
				AudioSource audioSource = new GameObject().AddComponent<AudioSource>();
				audioSource.transform.SetParent(this.transform);
				audioSource.name = audioMixerGroup.name;
				audioSource.resource = GetAudioResource(audioMixerGroup.name);
				audioSource.outputAudioMixerGroup = audioMixerGroup;
				audioSource.playOnAwake = false;
			}

			private void DestroyUninitializedAudioSources()
			{
				foreach (AudioSource audioSource in GetComponentsInChildren<AudioSource>())
					if (audioSource.resource == null)
						DestroyDynamically(audioSource.gameObject);
			}

		#endregion
	}


	[CustomEditor(typeof(SounderBuilder), true)]
	[CanEditMultipleObjects]
	public class SounderBuilderEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			SounderBuilder builder = (SounderBuilder)base.target;

			if (GUILayout.Button(nameof(SounderBuilder.Rebuild)))
				builder.Rebuild();
		}
	}
}


#endif


/*                                                                                */
/*        Windmill                                   Copyright © 2025-2026        */
/*        Praenaris                                    All rights reserved        */
/*                                                                                */