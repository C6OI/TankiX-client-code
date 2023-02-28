using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(636222333880646188L)]
    public interface SonarEffectTemplate : EffectBaseTemplate, Template {
        [AutoAdded]
        SonarEffectComponent sonarEffect();
    }
}