using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [SerialVersionUID(1504003994489L)]
    public class TutorialGroupComponent : GroupComponent {
        public TutorialGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public TutorialGroupComponent(long key)
            : base(key) { }
    }
}