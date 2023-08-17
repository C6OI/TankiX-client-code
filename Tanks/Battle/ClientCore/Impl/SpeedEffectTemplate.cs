using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(-4391676593794419937L)]
    public interface SpeedEffectTemplate : Template, EffectTemplate {
        [PersistentConfig]
        SpeedEffectComponent speedEffect();

        [PersistentConfig]
        DurationConfigComponent duration();
    }
}