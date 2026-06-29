using Cysharp.Threading.Tasks;
using DragonResonance.Attributes;
using DragonResonance.Localizer;
using UnityEngine;


public class BuildLocalizableEndpoint : LocalizableEndpoint
{
	[SerializeField] private bool _enableEditorTemplate = false;
	[ShowIf(nameof(_enableEditorTemplate))] [SerializeField] protected string _localizationEditorTemplate = "This is a editor {TEST}";

	[SerializeField] private bool _enableDevelopmentTemplate = false;
	[ShowIf(nameof(_enableDevelopmentTemplate))] [SerializeField] protected string _localizationDevelopmentTemplate = "This is a development {TEST}";

	[SerializeField] private bool _enableDemoTemplate = false;
	[ShowIf(nameof(_enableDemoTemplate))] [SerializeField] protected string _localizationDemoTemplate = "This is a demo {TEST}";


	#if UNITY_EDITOR

		public override void Localize()
		{
			if (_enableEditorTemplate)
				Localizer.Localize(_localizationEditorTemplate, OnLocalize).Forget();
		}

	#elif DEVELOPMENT_BUILD

		public override void Localize()
		{
			if (_enableDevelopmentTemplate)
				Localizer.Localize(_localizationDevelopmentTemplate, OnLocalize).Forget();
		}

	#elif DEMO_BUILD

		public override void Localize()
		{
			if (_enableDemoTemplate)
				Localizer.Localize(_localizationDemoTemplate, OnLocalize).Forget();
		}

	#endif
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