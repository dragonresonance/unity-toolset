#if UNITASK


using UnityEngine;


namespace DragonResonance.Sounder
{
	public class AnimatorStateSounderPlayer : StateMachineBehaviour
	{
		[SerializeField] private SAudioSourceConfig _stateEnterConfig = default;
		[SerializeField] private SAudioSourceConfig _stateExitConfig = default;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) =>
			Sounder.Play(_stateEnterConfig);
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) =>
			Sounder.Play(_stateExitConfig);
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