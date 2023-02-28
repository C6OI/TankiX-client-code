using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientMatchMaking.Impl;

namespace Tanks.Lobby.ClientMatchMaking.API {
    [SerialVersionUID(1496982580733L)]
    public interface MatchMakingModeTemplate : ItemImagedTemplate, Template {
        [AutoAdded]
        MatchMakingModeComponent matchMakingMode();

        [AutoAdded]
        [PersistentConfig]
        MatchMakingModeRestrictionsComponent matchMakingModeRestrictions();

        [AutoAdded]
        [PersistentConfig]
        MatchMakingModeActivationComponent matchMakingModeActivation();

        [AutoAdded]
        [PersistentConfig]
        DescriptionItemComponent descriptionItem();

        [AutoAdded]
        [PersistentConfig("order")]
        OrderItemComponent OrderItem();
    }
}