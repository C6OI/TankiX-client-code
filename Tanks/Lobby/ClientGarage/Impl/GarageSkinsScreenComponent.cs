using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GarageSkinsScreenComponent : BehaviourComponent {
        [SerializeField] SkinsButtonComponent skinsButton;

        public SkinsButtonComponent SkinsButton => skinsButton;
    }
}