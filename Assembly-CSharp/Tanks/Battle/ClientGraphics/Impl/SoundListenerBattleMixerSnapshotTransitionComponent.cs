using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class SoundListenerBattleMixerSnapshotTransitionComponent : BehaviourComponent {
        [SerializeField] float transitionTimeToSilentAfterRoundFinish = 0.25f;

        [SerializeField] float transitionTimeToSilentAfterExitBattle = 0.5f;

        [SerializeField] float transitionTimeToMelodySilent = 0.5f;

        [SerializeField] float transitionToLoudTimeInBattle = 0.5f;

        [SerializeField] float transitionToLoudTimeInSelfUserMode = 0.5f;

        public float TransitionTimeToSilentAfterRoundFinish => transitionTimeToSilentAfterRoundFinish;

        public float TransitionTimeToSilentAfterExitBattle => transitionTimeToSilentAfterExitBattle;

        public float TransitionToLoudTimeInBattle => transitionToLoudTimeInBattle;

        public float TransitionToLoudTimeInSelfUserMode => transitionToLoudTimeInSelfUserMode;

        public float TransitionTimeToMelodySilent => transitionTimeToMelodySilent;
    }
}