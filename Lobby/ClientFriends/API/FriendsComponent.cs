using System.Collections.Generic;
using System.Linq;
using System.Text;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientFriends.API {
    public class FriendsComponent : Component {
        public HashSet<long> AcceptedFriendsIds { get; set; }

        public HashSet<long> IncommingFriendsIds { get; set; }

        public HashSet<long> OutgoingFriendsIds { get; set; }

        public override string ToString() {
            StringBuilder stringBuilder = new();

            stringBuilder.Append("\n[" +
                                 string.Join(", ", AcceptedFriendsIds.Select(id => id.ToString()).ToArray()) +
                                 "]\n");

            stringBuilder.Append(
                "\n[" + string.Join(", ", IncommingFriendsIds.Select(id => id.ToString()).ToArray()) + "]\n");

            stringBuilder.Append("\n[" +
                                 string.Join(", ", OutgoingFriendsIds.Select(id => id.ToString()).ToArray()) +
                                 "]\n");

            return stringBuilder.ToString();
        }
    }
}