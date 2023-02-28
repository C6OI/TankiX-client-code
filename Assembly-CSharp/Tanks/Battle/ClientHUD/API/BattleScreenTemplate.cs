using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientHUD.API {
    [SerialVersionUID(635914005280250438L)]
    public interface BattleScreenTemplate : Template {
        BattleScreenComponent battleScreen();
    }
}