using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(1463118657532L)]
    public interface ServerShutdownTemplate : Template {
        ServerShutdownComponent serverShutdown();
    }
}