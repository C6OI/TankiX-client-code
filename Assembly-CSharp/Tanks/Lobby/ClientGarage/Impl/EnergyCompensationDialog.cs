using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class EnergyCompensationDialog : ConfirmDialogComponent {
        [SerializeField] TextMeshProUGUI quantiumsCount;

        [SerializeField] TextMeshProUGUI crysCount;

        public void Show(long charges, long crys, List<Animator> animators = null) {
            quantiumsCount.text = "x" + charges;
            crysCount.text = "x" + crys;
            TextMeshProUGUI[] componentsInChildren = GetComponentsInChildren<TextMeshProUGUI>();

            foreach (TextMeshProUGUI textMeshProUGUI in componentsInChildren) {
                textMeshProUGUI.fontStyle = FontStyles.UpperCase;
            }

            Show(animators);
        }
    }
}