using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    public class ShotValidateComponent : Component {
        public ShotValidateComponent() {
            BlockValidateMask = LayerMasks.STATIC;
            UnderGroundValidateMask = LayerMasks.STATIC;
        }

        public int BlockValidateMask { get; set; }

        public int UnderGroundValidateMask { get; set; }

        public GameObject[] RaycastExclusionGameObjects { get; set; }
    }
}