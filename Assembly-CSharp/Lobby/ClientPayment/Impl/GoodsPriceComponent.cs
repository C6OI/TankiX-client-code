using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientPayment.Impl {
    [SerialVersionUID(1453891891716L)]
    public class GoodsPriceComponent : Component {
        const double roundRatio = 100.0;

        public string Currency { get; set; }

        public double Price { get; set; }

        public double Round(double value) => Math.Round(value * 100.0) / 100.0;
    }
}