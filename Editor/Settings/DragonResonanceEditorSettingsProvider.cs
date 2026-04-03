#if UNITY_EDITOR


using UnityEditor;
using UnityEngine;


namespace DragonResonance.Editor.Settings
{
	public class DragonResonanceEditorSettingsProvider : SettingsProvider
	{
		private const string SettingsPath = "Project/Dragon Resonance";
		private const string BannerGUID = "9a60bf40f7c4aa74d94ea7c32361af13";
		private const int LargePadding = 60;
		private const int MediumPadding = 36;
		private const int SmallPadding = 12;


		private static DragonResonanceEditorSettings _settings;
		private static Texture2D _bannerImage = null;


		#region Constructors

			[SettingsProvider]
			public static SettingsProvider Create() => new DragonResonanceEditorSettingsProvider(SettingsPath, SettingsScope.Project);

			public DragonResonanceEditorSettingsProvider(string path, SettingsScope scope) : base(path, scope)
			{
				string bannerPath = AssetDatabase.GUIDToAssetPath(BannerGUID);
				_bannerImage = AssetDatabase.LoadAssetAtPath<Texture2D>(bannerPath);
			}

		#endregion


		#region Publics

			public override void OnGUI(string searchContext)
			{
				_settings = DragonResonanceEditorSettings.instance;

				Rect bannerRect = GUILayoutUtility.GetAspectRect((float)_bannerImage.width / _bannerImage.height);
				GUIStyle fullSection = new() { padding = new RectOffset(MediumPadding, MediumPadding, MediumPadding, MediumPadding) };
				GUIStyle separatedSection = new() { padding = new RectOffset(MediumPadding, MediumPadding, SmallPadding, SmallPadding) };
				GUIStyle copyrightSection = new() { padding = new RectOffset(LargePadding, LargePadding, SmallPadding, SmallPadding) };

				EditorGUI.DrawTextureTransparent(bannerRect, _bannerImage);

				EditorGUI.BeginChangeCheck();
				EditorGUILayout.BeginVertical(fullSection);
				{
					{
						EditorGUILayout.LabelField("Links", EditorStyles.whiteLargeLabel);
						EditorGUILayout.BeginHorizontal(separatedSection);
						{
							if (GUILayout.Button("Website"))
								Application.OpenURL("https://www.dragonresonance.com/");
							if (GUILayout.Button("GitHub"))
								Application.OpenURL("https://github.com/dragonresonance");
							if (GUILayout.Button("GitLab"))
								Application.OpenURL("https://gitlab.com/dragonresonance");
						}
						EditorGUILayout.EndHorizontal();
					}
					GUILayout.Space(MediumPadding);
					{
						EditorGUILayout.LabelField("Bug reports & Feature requests", EditorStyles.whiteLargeLabel);
						EditorGUILayout.BeginHorizontal(separatedSection);
						{
							if (GUILayout.Button("Submit DevKit issue"))
								Application.OpenURL("https://github.com/dragonresonance/unity-devkit/issues/new/choose");
							if (GUILayout.Button("Submit ToolSet issue"))
								Application.OpenURL("https://github.com/dragonresonance/unity-toolset/issues/new/choose");
						}
						EditorGUILayout.EndHorizontal();
					}
					GUILayout.Space(MediumPadding);
					{
						EditorGUILayout.LabelField("Developers", EditorStyles.whiteLargeLabel);
						{
							EditorGUILayout.BeginVertical(separatedSection);
							{
								EditorGUILayout.LabelField("David Tabernero M.", EditorStyles.largeLabel);
								EditorGUILayout.BeginHorizontal();
								{
									if (GUILayout.Button("Website"))
										Application.OpenURL("https://tabernero.dev/");
									if (GUILayout.Button("X"))
										Application.OpenURL("https://x.com/davidtabernerom");
									if (GUILayout.Button("GitHub"))
										Application.OpenURL("https://github.com/davidtabernerom");
									if (GUILayout.Button("LinkedIn"))
										Application.OpenURL("https://www.linkedin.com/in/davidtabernerom/");
								}
								EditorGUILayout.EndHorizontal();
								if (EditorGUILayout.LinkButton("Buy David a Ko-Fi!"))
									Application.OpenURL("https://ko-fi.com/davidtabernerom");
							}
							EditorGUILayout.EndVertical();
						}
					}
					GUILayout.Space(MediumPadding);
					{
						EditorGUILayout.BeginVertical(copyrightSection);
						{
							GUICenteredLabel("Copyright © 2021-2026. All rights reserved.");
							GUICenteredLabel("Licensed under the Apache License, Version 2.0.");
							GUICenteredLink("See LICENSE.md for more info.", "https://github.com/dragonresonance/unity-toolset/blob/master/LICENSE.md");
						}
						EditorGUILayout.EndVertical();
					}
					//_settings.EditorTestBool = EditorGUILayout.Toggle("Editor Test Bool", _settings.EditorTestBool);
					//_settings.EditorTestString = EditorGUILayout.TextField("Editor Test String", _settings.EditorTestString);
				}
				EditorGUILayout.EndVertical();
				if (EditorGUI.EndChangeCheck())
					_settings.Save();
			}

		#endregion


		#region Privates

			private static void GUICenteredLabel(string text)
			{
				EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				EditorGUILayout.LabelField(text, GUILayout.Width(EditorStyles.label.CalcSize(new GUIContent(text)).x));
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}

			private static void GUICenteredLink(string text, string url)
			{
				EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				if (EditorGUILayout.LinkButton(text))
					Application.OpenURL(url);
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
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