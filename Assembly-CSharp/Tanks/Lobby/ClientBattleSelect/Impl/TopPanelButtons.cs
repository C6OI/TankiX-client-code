using System;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class TopPanelButtons : MonoBehaviour {
        TopPanelButton[] buttons;

        int lastActivatedButtonIndex;

        TopPanelButton[] Buttons {
            get {
                if (buttons == null) {
                    buttons = GetComponentsInChildren<TopPanelButton>(true);
                }

                return buttons;
            }
        }

        public void ActivateButton(int index) {
            if (index < Buttons.Length) {
                ActivateButton(Buttons[index]);
            }
        }

        public void ActivateButton(TopPanelButton button) {
            TopPanelButton[] array = Buttons;

            foreach (TopPanelButton topPanelButton in array) {
                topPanelButton.Activated = false;
            }

            button.Activated = true;
            int num = Array.IndexOf(Buttons, button);
            bool flag = num > lastActivatedButtonIndex;
            Buttons[lastActivatedButtonIndex].ImageFillToRight = !flag;
            Buttons[num].ImageFillToRight = flag;
            lastActivatedButtonIndex = num;
        }
    }
}