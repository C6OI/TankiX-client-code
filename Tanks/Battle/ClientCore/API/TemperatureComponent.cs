using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(6673681254298647708L)]
    [Shared]
    public class TemperatureComponent : SharedChangeableComponent {
        float temperature;

        public float Temperature {
            get => temperature;
            set {
                temperature = value;
                OnChange();
            }
        }
    }
}