using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientHUD.Impl;

namespace Tanks.Battle.ClientHUD.API {
    [SerialVersionUID(1440050388188L)]
    public interface HUDWorldSpaceCanvasTemplate : Template {
        HUDWorldSpaceCanvas HUDWorldSpaceCanvas();
    }
}