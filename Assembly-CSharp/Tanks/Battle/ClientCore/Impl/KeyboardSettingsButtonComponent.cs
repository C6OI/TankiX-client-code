using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1479371711138L)]
    public class KeyboardSettingsButtonComponent : ButtonMappingComponentBase<ShowScreenLeftEvent<KeyboardSettingsScreenComponent>> { }
}