using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    [Shared]
    [SerialVersionUID(636404707971995843L)]
    public class CustomDiscountTextComponent : Component {
        public Dictionary<string, string> LocalizedText;

        public Dictionary<string, string> SteamLocalizedText;

        public string Get(string local, bool steam) {
            Dictionary<string, string> dictionary = !steam ? LocalizedText : SteamLocalizedText;

            if (dictionary.ContainsKey(local)) {
                return dictionary[local].Replace("\\n", "\n");
            }

            return string.Empty;
        }
    }
}