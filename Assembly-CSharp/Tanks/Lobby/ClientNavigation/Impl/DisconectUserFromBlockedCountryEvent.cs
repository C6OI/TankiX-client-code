using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientNavigation.Impl {
    [Shared]
    [SerialVersionUID(1537373781529L)]
    public class DisconectUserFromBlockedCountryEvent : Event { }
}