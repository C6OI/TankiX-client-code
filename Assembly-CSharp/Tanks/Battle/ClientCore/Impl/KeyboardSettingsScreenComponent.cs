using System.Collections.Generic;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1479371748999L)]
    public class KeyboardSettingsScreenComponent : BehaviourComponent {
        [SerializeField] GameObject defaultButton;

        [SerializeField] GameObject oneKeyOnFewActionsTextLabel;

        void OnEnable() {
            CheckForOneKeyOnFewActions();
        }

        public void EnableButtons() { }

        public void CheckForOneKeyOnFewActions() {
            KeyboardSettingsInputComponent[] componentsInChildren = GetComponentsInChildren<KeyboardSettingsInputComponent>(true);
            List<KeyCode> list = new();
            List<KeyCode> list2 = new();
            KeyboardSettingsInputComponent[] array = componentsInChildren;

            foreach (KeyboardSettingsInputComponent keyboardSettingsInputComponent in array) {
                keyboardSettingsInputComponent.SetInputState(false);
                KeyCode keyCode = keyboardSettingsInputComponent.LoadAction();

                if (keyCode == KeyCode.None) {
                    continue;
                }

                if (list.Contains(keyCode)) {
                    if (!list2.Contains(keyCode)) {
                        list2.Add(keyCode);
                    }
                } else {
                    list.Add(keyCode);
                }
            }

            KeyboardSettingsInputComponent[] array2 = componentsInChildren;

            foreach (KeyboardSettingsInputComponent keyboardSettingsInputComponent2 in array2) {
                KeyCode item = keyboardSettingsInputComponent2.LoadAction();

                if (list2.Contains(item)) {
                    keyboardSettingsInputComponent2.SetInputState(true);
                }
            }

            oneKeyOnFewActionsTextLabel.gameObject.SetActive(list2.Count > 0);
        }
    }
}