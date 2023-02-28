using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1513249041434L)]
    public class DurationItemComponent : Component {
        public TimeSpan Duration { get; set; }
    }
}