using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.API {
    public class DecalManagerComponent : Component {
        public LinkedList<DecalEntry> DecalsQueue { get; } = new();

        public BulletHoleDecalManager BulletHoleDecalManager { get; set; }

        public GraffitiDynamicDecalManager GraffitiDynamicDecalManager { get; set; }

        public DecalMeshBuilder DecalMeshBuilder { get; set; }

        public bool EnableDecals { get; set; }
    }
}