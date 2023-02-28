using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientControls.API {
    [RequireComponent(typeof(TMP_InputField))]
    public class TMPInputFieldReturnSelector : InputFieldReturnSelector {
        TMP_InputField tmpInputField;

        protected override void Start() {
            base.Start();
            tmpInputField = GetComponent<TMP_InputField>();
        }

        protected override bool CurrentInputSelected() => EventSystem.current.currentSelectedGameObject == tmpInputField.gameObject;

        protected override void SelectCurrentInput() {
            if (!(selectable is TMP_InputField)) {
                tmpInputField.Select();
            }
        }
    }
}