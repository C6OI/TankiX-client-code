using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(2298069202679495205L)]
    public class EnterBattleAsSpectatorEvent : Event { }
}