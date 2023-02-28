using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarCameraFlightState {
        public class EmptyState : Node { }

        public class ArcFlightState : Node {
            public HangarCameraArcFlightComponent hangarCameraArcFlight;
        }

        public class LinearFlightState : Node {
            public HangarCameraLinearFlightComponent hangarCameraLinearFlight;
        }
    }
}