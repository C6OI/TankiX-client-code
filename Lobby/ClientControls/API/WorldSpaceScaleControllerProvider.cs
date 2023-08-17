using UnityEngine;

namespace Lobby.ClientControls.API {
    public class WorldSpaceScaleControllerProvider : MonoBehaviour, BaseElementScaleControllerProvider {
        [SerializeField] BaseElementScaleController baseElementScaleController;

        public BaseElementScaleController BaseElementScaleController => baseElementScaleController;
    }
}