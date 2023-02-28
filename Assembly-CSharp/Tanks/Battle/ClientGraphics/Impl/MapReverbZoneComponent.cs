using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MapReverbZoneComponent : Component {
        public MapReverbZoneComponent(GameObject reverbZoneRoot) => ReverbZoneRoot = reverbZoneRoot;

        public GameObject ReverbZoneRoot { get; set; }
    }
}