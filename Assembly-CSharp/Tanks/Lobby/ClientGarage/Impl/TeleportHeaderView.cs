using System;
using System.Collections.Generic;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class TeleportHeaderView : MonoBehaviour {
        public LocalizedField lvl1;

        public LocalizedField lvl2;

        public LocalizedField lvl3;

        public LocalizedField lvl4;

        public LocalizedField lvl5;

        public LocalizedField broken;

        public LocalizedField hint;

        public LocalizedField brokenHint;

        public TextMeshProUGUI labelText;

        public TextMeshProUGUI hintText;

        List<LocalizedField> labels;

        void Awake() {
            labels = new List<LocalizedField> { lvl1, lvl2, lvl3, lvl4, lvl5 };
        }

        public void UpdateView(int zoneIndex) {
            zoneIndex = Math.Min(zoneIndex, labels.Count - 1);
            labelText.text = labels[zoneIndex].Value.ToUpper();
            hintText.text = hint;
        }

        public void SetBrokenView() {
            labelText.text = broken.Value.ToUpper();
            hintText.text = brokenHint;
        }
    }
}