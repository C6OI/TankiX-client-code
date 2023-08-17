using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientCommunicator.API {
    [SerialVersionUID(1446186921366L)]
    [Shared]
    public class SectionGroupComponent : GroupComponent {
        public SectionGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public SectionGroupComponent(long key)
            : base(key) { }
    }
}