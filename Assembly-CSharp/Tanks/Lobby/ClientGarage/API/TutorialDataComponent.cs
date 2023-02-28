using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class TutorialDataComponent : Component {
        public string StepsPath { get; set; }

        public long TutorialId { get; set; }

        public Dictionary<string, long> Steps { get; set; }

        [ProtocolOptional] public bool ForNewPlayer { get; set; }

        [ProtocolOptional] public bool ForOldPlayer { get; set; }
    }
}