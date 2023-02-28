using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.Impl;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class MVPModuleContainer : MonoBehaviour {
        [SerializeField] GameObject card;

        [SerializeField] TextMeshProUGUI cardInfo;

        [SerializeField] LocalizedField moduleLevelShortLocalizedField;

        public void SetupModuleCard(ModuleInfo m, float moduleSize) {
            card.AddComponent<EntityBehaviour>().handleAutomaticaly = false;
            card.GetComponent<ModuleCardView>().UpdateView(m.ModuleId, m.UpgradeLevel, false, false);
            cardInfo.text = string.Format("{0} ({1} {2})", card.GetComponent<ModuleCardView>().name, moduleLevelShortLocalizedField.Value, m.UpgradeLevel + 1);
            card.transform.localScale = new Vector3(moduleSize, moduleSize, moduleSize);
            card.transform.localPosition = new Vector3(0f, 0f, 20f);
        }
    }
}