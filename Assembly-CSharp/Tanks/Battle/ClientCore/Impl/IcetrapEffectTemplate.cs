using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(636384697009346423L)]
    public interface IcetrapEffectTemplate : MineEffectTemplate, EffectBaseTemplate, Template {
        [AutoAdded]
        IcetrapEffectComponent icetrapEffect();
    }
}