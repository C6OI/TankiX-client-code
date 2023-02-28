using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPayment.API {
    [SerialVersionUID(1495081806742L)]
    public class ExchangeRateComponent : Component {
        public static float ExhchageRate { get; private set; }

        public float Rate {
            get => ExhchageRate;
            set => ExhchageRate = value;
        }
    }
}