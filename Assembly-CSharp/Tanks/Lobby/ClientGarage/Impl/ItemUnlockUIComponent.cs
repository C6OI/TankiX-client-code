using System.Collections.Generic;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ItemUnlockUIComponent : MonoBehaviour, Component {
        public AnimatedProgress experienceProgressBar;

        public TextMeshProUGUI text;

        public GameObject rewardPrefab;

        public GameObject rewardContainer;

        public List<GameObject> rewardPreviews = new();

        public LocalizedField recievedText;

        public LocalizedField levelText;

        public LocalizedField maxText;
    }
}