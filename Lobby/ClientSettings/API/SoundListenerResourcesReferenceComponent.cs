using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Lobby.ClientSettings.API {
    public class SoundListenerResourcesReferenceComponent : BehaviourComponent {
        [SerializeField] AssetReference resources;

        public AssetReference Resources => resources;
    }
}