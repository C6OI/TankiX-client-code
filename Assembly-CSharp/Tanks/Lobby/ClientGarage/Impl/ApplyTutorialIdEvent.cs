using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [Shared]
    [SerialVersionUID(1505212007257L)]
    public class ApplyTutorialIdEvent : Event {
        public ApplyTutorialIdEvent(long id) => Id = id;

        public long Id { get; }
    }
}