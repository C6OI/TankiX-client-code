using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class PacksImagesComponent : Component {
        public Dictionary<long, List<string>> AmountToImages { get; set; }
    }
}