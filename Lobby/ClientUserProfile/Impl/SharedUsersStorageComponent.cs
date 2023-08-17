using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;

namespace Lobby.ClientUserProfile.Impl {
    public class SharedUsersStorageComponent : Component {
        public SharedUsersStorageComponent() => UserId2EntityIdMap = new HashMultiMap<long, long>();

        public HashMultiMap<long, long> UserId2EntityIdMap { get; private set; }
    }
}