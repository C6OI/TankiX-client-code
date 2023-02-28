using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class GetInstanceFromPoolEvent : Event {
        public float AutoRecycleTime = -1f;

        public Transform Instance;
        public GameObject Prefab;
    }
}