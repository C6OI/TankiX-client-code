using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientGraphics.API {
    [SerialVersionUID(636268131710680176L)]
    public abstract class CameraShakerConfigComponent : Component {
        public bool Enabled { get; set; }

        public float Magnitude { get; set; }

        public float Roughness { get; set; }

        public float FadeInTime { get; set; }

        public float FadeOutTime { get; set; }

        public float PosInfluenceX { get; set; }

        public float PosInfluenceY { get; set; }

        public float PosInfluenceZ { get; set; }

        public float RotInfluenceX { get; set; }

        public float RotInfluenceY { get; set; }

        public float RotInfluenceZ { get; set; }
    }
}