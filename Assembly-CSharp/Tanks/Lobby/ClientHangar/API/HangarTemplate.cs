using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientHangar.Impl;

namespace Tanks.Lobby.ClientHangar.API {
    [SerialVersionUID(1436527950921L)]
    public interface HangarTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        HangarAssetComponent hangarAsset();
    }
}