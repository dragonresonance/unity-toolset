using DragonResonance.Behaviours;
using UnityEngine.Events;
using UnityEngine;


namespace DragonResonance.Localizer
{
	public class LocalizableEndpoint : PossumBehaviour
	{
		[SerializeField] private bool _autoTranslateOnLanguageChange = true;
		[SerializeField] private string _localizationTemplate = "This is a {TEST}";


		public UnityEvent<string> OnLocalize = null;


		#region Events

			private void OnEnable()
			{
				Localize();
				if (_autoTranslateOnLanguageChange)
					Localizer.OnLanguageChange += Localize;
			}

			private void OnDisable()
			{
				if (_autoTranslateOnLanguageChange)
					Localizer.OnLanguageChange -= Localize;
			}

		#endregion


		#region Publics

			[ContextMenu(nameof(Localize))]
			public void Localize() => Localizer.Localize(_localizationTemplate, OnLocalize).Forget();

		#endregion
	}
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