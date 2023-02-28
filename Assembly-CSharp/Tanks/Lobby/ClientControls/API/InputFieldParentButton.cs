using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientControls.API {
    public class InputFieldParentButton : MonoBehaviour {
        void Start() {
            InputField componentInChildren = GetComponentInChildren<InputField>();
            Button component = GetComponent<Button>();

            if (componentInChildren != null && component != null) {
                GetComponent<Button>().onClick.AddListener(componentInChildren.Select);
            }
        }
    }
}