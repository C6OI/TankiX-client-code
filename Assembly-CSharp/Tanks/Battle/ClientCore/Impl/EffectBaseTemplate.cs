using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1486041253393L)]
    public interface EffectBaseTemplate : Template {
        EffectComponent effectComponent();
    }
}