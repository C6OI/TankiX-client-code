using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class UserInteractionsData {
        public bool canRequestFrendship;

        public bool friendshipRequestWasSend;

        public InteractionSource interactionSource;

        public bool isMuted;

        public bool isReported;

        public string OtherUserName;

        public Entity selfUserEntity;

        public long sourceId;

        public long userId;
    }
}