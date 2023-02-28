using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientGraphics.API {
    [SerialVersionUID(636269177906259140L)]
    public class ImpactCameraShakerConfigComponent : CameraShakerConfigComponent {
        public float MinDistanceMagnitude { get; set; }

        public float MaxDistanceMagnitude { get; set; }
    }
}