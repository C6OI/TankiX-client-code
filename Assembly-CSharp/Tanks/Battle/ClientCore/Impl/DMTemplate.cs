using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(-4141404049750078994L)]
    public interface DMTemplate : BattleTemplate, Template {
        DMComponent dmComponent();
    }
}