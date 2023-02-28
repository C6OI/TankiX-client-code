using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [Shared]
    [SerialVersionUID(1505286737090L)]
    public class TutorialCompleteIdsComponent : Component {
        public List<long> CompletedIds { get; set; }

        public bool TutorialSkipped { get; set; }
    }
}