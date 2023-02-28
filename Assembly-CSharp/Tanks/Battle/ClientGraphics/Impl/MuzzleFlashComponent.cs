using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MuzzleFlashComponent : BehaviourComponent {
        public GameObject muzzleFlashPrefab;

        public float duration = 0.5f;
    }
}