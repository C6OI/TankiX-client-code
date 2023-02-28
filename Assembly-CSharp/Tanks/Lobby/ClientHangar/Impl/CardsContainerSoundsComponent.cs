using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class CardsContainerSoundsComponent : BehaviourComponent {
        [SerializeField] CardsSoundBehaviour cardsSounds;

        public CardsSoundBehaviour CardsSounds => cardsSounds;
    }
}