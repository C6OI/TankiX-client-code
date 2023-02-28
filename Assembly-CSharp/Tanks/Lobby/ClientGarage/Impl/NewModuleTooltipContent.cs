using System;
using System.Text;
using Tanks.Lobby.ClientControls.API;
using tanks.modules.lobby.ClientControls.Scripts.API;
using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class NewModuleTooltipContent : MonoBehaviour, ITooltipContent {
        static readonly StringBuilder stringBuilder = new(100);
        public ComplexFillProgressBar progressBar;

        public TextMeshProUGUI nameAndLevel;

        public TextMeshProUGUI definition;

        public LocalizedField levelPrefix;

        public TextMeshProUGUI blueprints;

        public void Init(object data) {
            if (!(data is ModuleItem)) {
                throw new ArgumentException(string.Concat("Incorrect data type ", data.GetType(), ". ModuleItem expected."));
            }

            ModuleItem moduleItem = data as ModuleItem;
            stringBuilder.Length = 0;
            stringBuilder.AppendFormat("{0} ({1}{2})", moduleItem.Name, levelPrefix.Value, moduleItem.Level + 1);
            nameAndLevel.text = stringBuilder.ToString();
            definition.text = moduleItem.Description();
            progressBar.ProgressValue = 0f;
            long userCardCount = moduleItem.UserCardCount;
            int num = moduleItem.UserItem != null ? moduleItem.UpgradePrice : moduleItem.CraftPrice.Cards;
            blueprints.text = string.Format("{0}/{1}", userCardCount, num);
            progressBar.ProgressValue = Mathf.Clamp01(userCardCount / (float)num);
        }
    }
}