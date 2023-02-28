using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ItemPackButtonComponent : BehaviourComponent {
        [SerializeField] int count;

        public int Count {
            get => count;
            set => count = value;
        }
    }
}