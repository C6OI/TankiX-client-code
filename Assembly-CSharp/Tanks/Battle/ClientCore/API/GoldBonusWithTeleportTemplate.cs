using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(636404649605276028L)]
    public interface GoldBonusWithTeleportTemplate : GoldBonusTemplate, BonusTemplate, Template { }
}