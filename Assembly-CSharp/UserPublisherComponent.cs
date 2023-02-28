using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientUserProfile.API;

[Shared]
[SerialVersionUID(32195187150433L)]
public class UserPublisherComponent : Component {
    public Publisher Publisher { get; set; }
}