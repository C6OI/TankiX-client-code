using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ChassisTrackControllerComponent : Component {
        public TrackController LeftTrack { get; set; }

        public TrackController RightTrack { get; set; }
    }
}