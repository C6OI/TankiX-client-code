using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientNavigation.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientLoading.API {
    public class BattleLoadScreenComponent : MonoBehaviour, Component, NoScaleScreen {
        public static string MAP_FLAVOR_ID_PREFS_KEY = "MAP_FLAVOR_ID_PREFS_KEY";

        public Image mapPreview;

        public TextMeshProUGUI mapName;

        public TextMeshProUGUI battleInfo;

        public ResourcesLoadProgressBarComponent progressBar;

        public TextMeshProUGUI flavorText;

        public TextMeshProUGUI initialization;

        public LocalizedField arcadeBattleText;

        public LocalizedField energyBattleText;

        public LocalizedField ratingBattleText;

        public LoadingStatusView loadingStatusView;

        Entity battle;

        Map map;

        bool needUpdate;

        public bool isReadyToHide => progressBar.ProgressBar.ProgressValue > 0f;

        void Awake() {
            mapName.text = string.Empty;
            battleInfo.text = string.Empty;
            flavorText.text = string.Empty;
            GetComponent<LoadBundlesTaskProviderComponent>().OnDataChange = UpdateView;
        }

        void Update() {
            if (needUpdate) {
                needUpdate = !UpdateBattleInfo();
                needUpdate &= !UpdateMapPreview();
            }
        }

        public void InitView(Entity battle, Map map) {
            needUpdate = true;
            mapName.text = map.Name.ToUpper();
            flavorText.text = GetFlavorText(map);
            this.battle = battle;
            this.map = map;
            Update();
        }

        void UpdateView(LoadBundlesTaskComponent loadBundlesTaskComponent) {
            progressBar.UpdateView(loadBundlesTaskComponent);
            initialization.gameObject.SetActive(loadBundlesTaskComponent.BytesToLoad - loadBundlesTaskComponent.BytesLoaded <= 5242880);
            loadingStatusView.UpdateView(loadBundlesTaskComponent);
        }

        static string GetFlavorText(Map map) {
            if (map.FlavorTextList.Count <= 0) {
                return string.Empty;
            }

            int num = 0;
            string key = MAP_FLAVOR_ID_PREFS_KEY + map.Name;

            if (PlayerPrefs.HasKey(key)) {
                int @int = PlayerPrefs.GetInt(key);
                num = @int + 1 < map.FlavorTextList.Count ? @int + 1 : 0;
            }

            PlayerPrefs.SetInt(key, num);
            PlayerPrefs.Save();
            return map.FlavorTextList[num];
        }

        bool UpdateBattleInfo() {
            if (!battle.HasComponent<EnergyBattleComponent>() && !battle.HasComponent<ArcadeBattleComponent>() && !battle.HasComponent<RatingBattleComponent>()) {
                return false;
            }

            BattleMode battleMode = battle.GetComponent<BattleModeComponent>().BattleMode;
            battleInfo.text = string.Concat(GetTypeText(), " (", battleMode, ")");
            return true;
        }

        string GetTypeText() {
            if (battle.HasComponent<ArcadeBattleComponent>()) {
                return arcadeBattleText.Value;
            }

            if (battle.HasComponent<EnergyBattleComponent>()) {
                return energyBattleText.Value;
            }

            return ratingBattleText.Value;
        }

        bool UpdateMapPreview() {
            if (map.LoadPreview == null) {
                mapPreview.gameObject.SetActive(false);
                return false;
            }

            mapPreview.gameObject.SetActive(true);
            mapPreview.sprite = map.LoadPreview;
            return true;
        }
    }
}