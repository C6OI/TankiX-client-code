using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(-7944772313373733709L)]
    public class BonusDropTimeComponent : Component {
        public BonusDropTimeComponent() { }

        public BonusDropTimeComponent(Date dropTime) => DropTime = dropTime;

        public Date DropTime { get; set; }
    }
}