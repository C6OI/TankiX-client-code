using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [Shared]
    [SerialVersionUID(1504160222978L)]
    public class ChangeScreenEvent : Event {
        public ChangeScreenEvent(string currentScreen, string nextScreen, double duration) {
            CurrentScreen = currentScreen;
            NextScreen = nextScreen;
            Duration = duration;
        }

        public string CurrentScreen { get; set; }

        public string NextScreen { get; set; }

        public double Duration { get; set; }
    }
}