using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarCameraViewState {
        public class TankViewState : Node {
            public HangarCameraTankViewComponent hangarCameraTankView;
        }

        public class FlightToLocationState : Node {
            public HangarCameraFlightToLocationComponent hangarCameraFlightToLocation;
        }

        public class LocationViewState : Node {
            public HangarCameraLocationViewComponent hangarCameraLocationView;
        }

        public class FlightToTankState : Node {
            public HangarCameraFlightToTankComponent hangarCameraFlightToTank;
        }
    }
}