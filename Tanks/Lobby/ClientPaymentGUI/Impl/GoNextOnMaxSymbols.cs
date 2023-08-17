using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    [RequireComponent(typeof(InputField))]
    public class GoNextOnMaxSymbols : MonoBehaviour {
        void Awake() => GetComponent<InputField>().onValueChanged.AddListener(ValueChanged);

        void ValueChanged(string val) {
            InputField component = GetComponent<InputField>();

            if (component.text.Length == component.characterLimit) {
                component.navigation.selectOnDown.Select();
            }
        }
    }
}