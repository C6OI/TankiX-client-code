using System.Collections.Generic;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class PlayScreenEnergyComponent : BehaviourComponent {
        [SerializeField] Color fullColor;

        [SerializeField] Color partColor;

        [SerializeField] List<UIRectClipperY> energyBar;

        [SerializeField] TextMeshProUGUI quantumCountText;

        [SerializeField] TooltipShowBehaviour tooltip;

        long maxEnergy;

        long quantumCost;

        public void Init(long maxEnergy, long cost) {
            this.maxEnergy = maxEnergy;
            quantumCost = cost;
        }

        public void SetEnergy(long currentEnergy) {
            long num = currentEnergy / quantumCost;

            for (int i = 0; i < energyBar.Count; i++) {
                if (i < num) {
                    energyBar[i].ToY = 1f;
                    energyBar[i].gameObject.GetComponent<Image>().color = fullColor;
                } else if (i == num) {
                    energyBar[i].ToY = (currentEnergy - num * quantumCost) / (float)quantumCost;
                    energyBar[i].gameObject.GetComponent<Image>().color = partColor;
                } else {
                    energyBar[i].ToY = 0f;
                }
            }

            quantumCountText.text = "<size=25><sprite=0></size> " + num + " / 3";
            tooltip.TipText = string.Format("{0} / {1}", currentEnergy, maxEnergy);
        }
    }
}