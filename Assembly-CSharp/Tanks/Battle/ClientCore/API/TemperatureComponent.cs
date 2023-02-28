using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(6673681254298647708L)]
    public class TemperatureComponent : Component {
        public float Temperature { get; set; }
    }
}