using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    [Shared]
    [SerialVersionUID(636445697188126867L)]
    public class GiftPromoUIDataComponent : Component {
        public string PromoKey { get; set; }

        public Dictionary<string, string> Texts { get; set; }

        public string Get(string local) {
            if (Texts.ContainsKey(local)) {
                return Texts[local].Replace("\\n", "\n");
            }

            return string.Empty;
        }
    }
}