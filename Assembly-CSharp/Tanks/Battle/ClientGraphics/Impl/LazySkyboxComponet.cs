using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class LazySkyboxComponet : BehaviourComponent {
        [SerializeField] AssetReference skyBoxReference;

        public AssetReference SkyBoxReference => skyBoxReference;
    }
}