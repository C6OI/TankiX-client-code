using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [Shared]
    [SerialVersionUID(1505212162608L)]
    public class TutorialIdResultEvent : Event {
        public long Id { get; set; }

        public bool ActionExecuted { get; set; }

        public bool ActionSuccess { get; set; }
    }
}