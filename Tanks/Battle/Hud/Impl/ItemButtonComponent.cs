using Platform.Library.ClientProtocol.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.Hud.Impl {
    [SerialVersionUID(635824351532645226L)]
    public class ItemButtonComponent : MonoBehaviour, Component {
        [SerializeField] Animator animator;

        [SerializeField] Text valueText;

        public Animator Animator => animator;

        public Text ValueText => valueText;
    }
}