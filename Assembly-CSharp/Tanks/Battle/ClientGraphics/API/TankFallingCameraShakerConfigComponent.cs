using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientGraphics.API {
    [SerialVersionUID(636270773716874286L)]
    public class TankFallingCameraShakerConfigComponent : CameraShakerConfigComponent {
        public float MinFallingPower { get; set; }

        public float MaxFallingPower { get; set; }

        public float MinFallingPowerForHUD { get; set; }
    }
}