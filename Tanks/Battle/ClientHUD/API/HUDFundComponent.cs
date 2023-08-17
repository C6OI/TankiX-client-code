using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientHUD.API {
    public class HUDFundComponent : MonoBehaviour, Component {
        public Text fundText;
    }
}