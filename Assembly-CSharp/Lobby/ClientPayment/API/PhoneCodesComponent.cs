using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientPayment.API {
    public class PhoneCodesComponent : Component {
        public Dictionary<string, string> Codes { get; set; }
    }
}