using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(6955808089218759626L)]
    public class TeamGroupComponent : GroupComponent {
        public TeamGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public TeamGroupComponent(long key)
            : base(key) { }
    }
}