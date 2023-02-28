using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Platform.Library.ClientUnityIntegration.API {
    public class TextMappingComponent : MonoBehaviour, Component {
        Text text;

        Text TextComponent {
            get {
                if (text == null) {
                    text = GetComponent<Text>();
                }

                return text;
            }
        }

        public virtual string Text {
            get => TextComponent.text;
            set => TextComponent.text = value;
        }
    }
}