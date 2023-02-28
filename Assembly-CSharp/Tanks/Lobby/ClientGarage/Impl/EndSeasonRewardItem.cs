using System.Collections.Generic;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class EndSeasonRewardItem {
        public long StartPlace { get; set; }

        public long EndPlace { get; set; }

        [ProtocolOptional] public List<DroppedItem> Items { get; set; }
    }
}