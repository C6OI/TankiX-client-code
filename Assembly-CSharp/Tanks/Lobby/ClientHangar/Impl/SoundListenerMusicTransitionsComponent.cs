using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class SoundListenerMusicTransitionsComponent : BehaviourComponent {
        [SerializeField] float musicTransitionSec = 0.5f;

        [SerializeField] float transitionCardsContainerTheme = 0.2f;

        [SerializeField] float transitionModuleTheme = 0.2f;

        public float MusicTransitionSec => musicTransitionSec;

        public float TransitionCardsContainerTheme => transitionCardsContainerTheme;

        public float TransitionModuleTheme => transitionModuleTheme;
    }
}