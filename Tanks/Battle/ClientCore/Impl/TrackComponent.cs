using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class TrackComponent : Component {
        public Track LeftTrack { get; set; }

        public Track RightTrack { get; set; }
    }
}