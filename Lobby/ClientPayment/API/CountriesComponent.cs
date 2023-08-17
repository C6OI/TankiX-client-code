using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientPayment.API {
    public class CountriesComponent : Component {
        public Dictionary<string, string> Names { get; set; }
    }
}