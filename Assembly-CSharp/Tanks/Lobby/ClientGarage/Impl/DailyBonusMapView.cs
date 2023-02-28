using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DailyBonusMapView : MonoBehaviour {
        static readonly string DAILY_BONUS_CYCLE_KEY = "DAILY_BONUS_CYCLE_KEY";

        static readonly string DAILY_BONUS_ELEMENT_LOCATION_KEY = "DAILY_BONUS_ELEMENT_LOCATION_KEY";
        public List<GameObject> zones;

        public GameObject bonusElementPrefab;

        public List<MapViewBonusElement> bonusElements;

        public MapViewBonusElement selectedBonusElement;

        public ImageListSkin back;

        public Action<MapViewBonusElement> onSelectedBonusElementChanged;

        public void OnBonusElementToggleValueChanged(MapViewBonusElement bonusElement, bool isChecked) {
            ToggleGroup component = GetComponent<ToggleGroup>();

            if (!isChecked) {
                if (!component.AnyTogglesOn()) {
                    selectedBonusElement = null;
                    onSelectedBonusElementChanged(selectedBonusElement);
                }

                return;
            }

            selectedBonusElement = bonusElement;
            onSelectedBonusElementChanged(selectedBonusElement);

            foreach (Toggle item in component.ActiveToggles()) {
                if (item != selectedBonusElement.Toggle) {
                    item.isOn = false;
                }
            }

            if (isChecked) {
                DailyBonusScreenSoundsRoot component2 = UISoundEffectController.UITransformRoot.GetComponent<DailyBonusScreenSoundsRoot>();
                component2.dailyBonusSoundsBehaviour.PlayClick();
            }
        }

        public void UpdateView(DailyBonusCycleComponent dailyBonusCycleComponent, DailyBonusScreenSystem.UserDailyBonusNode userDailyBonusNode) {
            DailyBonusData[] dailyBonuses = dailyBonusCycleComponent.DailyBonuses;
            int num = (int)userDailyBonusNode.userDailyBonusCycle.CycleNumber;

            if (bonusElements.Count == 0) {
                CreateBonusElements(dailyBonusCycleComponent, userDailyBonusNode);
            }

            UpdateBonusElementsPositions(num);
            PlayerPrefs.SetInt(DAILY_BONUS_CYCLE_KEY, num);
            PlayerPrefs.Save();
            int zoneIndex = (int)userDailyBonusNode.userDailyBonusZone.ZoneNumber;
            UpdateZoneRadiusView(zoneIndex);
            UpdateBonusElementView(dailyBonusCycleComponent, userDailyBonusNode, zoneIndex, dailyBonuses);
            UpdateBackView(userDailyBonusNode);
        }

        void UpdateBackView(DailyBonusScreenSystem.UserDailyBonusNode userDailyBonusNode) {
            Entity entity = userDailyBonusNode.Entity;
            long cycleNumber = userDailyBonusNode.userDailyBonusCycle.CycleNumber;
            long num = (entity.Id + cycleNumber) % back.Count;
            back.SelectedSpriteIndex = (int)num;
        }

        void UpdateBonusElementsPositions(int cycleNumber) {
            List<List<Vector2>> zoneToPossibleNormalizedPositions = GetZoneToPossibleNormalizedPositions();

            for (int i = 0; i < bonusElements.Count; i++) {
                bonusElements[i].GetComponent<RectTransform>().anchoredPosition = GetLocation(i, bonusElements[i].ZoneIndex, cycleNumber, zoneToPossibleNormalizedPositions);
            }
        }

        List<List<Vector2>> GetZoneToPossibleNormalizedPositions() {
            List<List<Vector2>> list = new();

            for (int i = 0; i < zones.Count; i++) {
                list.Add(new List<Vector2>());
            }

            Rect rect = bonusElementPrefab.GetComponent<RectTransform>().rect;
            int[] zoneRadiuses = GetZoneRadiuses();
            int num = zoneRadiuses[zoneRadiuses.Length - 1];
            int num2 = (int)rect.width;

            for (int j = -num; j < num; j += num2) {
                for (int k = -num; k < num; k += num2) {
                    Vector2 vector = new(j, k);
                    int zoneIndex = GetZoneIndex(vector, num2, zoneRadiuses);

                    if (zoneIndex >= 0) {
                        vector /= zoneRadiuses[zoneIndex];
                        list[zoneIndex].Add(vector);
                    }
                }
            }

            for (int l = 0; l < list.Count; l++) {
                Shuffle(list[l]);
            }

            return list;
        }

        int[] GetZoneRadiuses() {
            int[] array = new int[zones.Count];

            for (int i = 0; i < array.Length; i++) {
                array[i] = GetZoneRadius(i);
            }

            return array;
        }

        int GetZoneRadius(int zoneIndex) => (int)(zones[zoneIndex].GetComponent<RectTransform>().rect.width / 2f);

        public static void Shuffle<T>(List<T> list) {
            int num = list.Count;

            while (num > 1) {
                num--;
                int index = (int)(Random.value * (num + 1));
                T value = list[index];
                list[index] = list[num];
                list[num] = value;
            }
        }

        int GetZoneIndex(Vector2 pos, int elementRadius, int[] zoneRadiuses) {
            for (int i = 0; i < zoneRadiuses.Length; i++) {
                if (pos.magnitude <= zoneRadiuses[i] - elementRadius) {
                    return i;
                }

                if (pos.magnitude <= zoneRadiuses[i] + elementRadius) {
                    return -1;
                }
            }

            return -1;
        }

        void UpdateBonusElementView(DailyBonusCycleComponent dailyBonusCycleComponent, DailyBonusScreenSystem.UserDailyBonusNode userDailyBonusNode, int zoneIndex,
            DailyBonusData[] dailyBonuses) {
            List<long> receivedRewards = userDailyBonusNode.userDailyBonusReceivedRewards.ReceivedRewards;
            int num = dailyBonusCycleComponent.Zones[zoneIndex];

            for (int i = 0; i < dailyBonuses.Length; i++) {
                if (i > num) {
                    bonusElements[i].UpdateView(dailyBonuses[i], BonusElementState.INACCESSIBLE);
                    continue;
                }

                if (receivedRewards.Contains(dailyBonuses[i].Code)) {
                    bonusElements[i].UpdateView(dailyBonuses[i], BonusElementState.TAKEN);
                    continue;
                }

                bonusElements[i].UpdateView(dailyBonuses[i], BonusElementState.ACCESSIBLE);
                bonusElements[i].transform.SetAsLastSibling();
            }

            if (selectedBonusElement != null) {
                selectedBonusElement = null;
                onSelectedBonusElementChanged(selectedBonusElement);
            }
        }

        void CreateBonusElements(DailyBonusCycleComponent dailyBonusCycleComponent, DailyBonusScreenSystem.UserDailyBonusNode userDailyBonusNode) {
            int num = 0;
            int[] array = dailyBonusCycleComponent.Zones;
            int seed = (int)userDailyBonusNode.userDailyBonusCycle.CycleNumber;

            for (int i = 0; i < array.Length; i++) {
                Random.InitState(seed);

                for (int j = num; j <= array[i]; j++) {
                    GameObject gameObject = Instantiate(bonusElementPrefab);
                    gameObject.transform.SetParent(transform, false);
                    MapViewBonusElement component = gameObject.GetComponent<MapViewBonusElement>();
                    component.ZoneIndex = i;
                    bonusElements.Add(component);
                    component.OnValueChanged(OnBonusElementToggleValueChanged);
                    component.SetToggleGroup(GetComponent<ToggleGroup>());
                }

                num = array[i] + 1;
            }
        }

        Vector2 GetLocation(int elementIndex, int elementZoneIndex, int cycleNumber, List<List<Vector2>> zoneToPossiblePositions) {
            Vector2 vector;

            if (HasSavedLocation(cycleNumber, elementIndex)) {
                vector = GetSavedLocation(elementIndex);
            } else {
                vector = CreateRandomLocation(elementZoneIndex, zoneToPossiblePositions);
                SaveLocation(elementIndex, vector);
            }

            return vector * GetZoneRadius(elementZoneIndex);
        }

        Vector2 GetSavedLocation(int elementIndex) {
            string @string = PlayerPrefs.GetString(DAILY_BONUS_ELEMENT_LOCATION_KEY + elementIndex);

            try {
                string[] array = @string.Split('|');
                float x = float.Parse(array[0]);
                float y = float.Parse(array[1]);
                return new Vector2(x, y);
            } catch (Exception exception) {
                Debug.LogException(exception);
                PlayerPrefs.DeleteKey(DAILY_BONUS_ELEMENT_LOCATION_KEY + elementIndex);
                return default;
            }
        }

        void SaveLocation(int elementIndex, Vector2 location) {
            PlayerPrefs.SetString(GetPrefsKey(elementIndex), location.x + "|" + location.y);
        }

        static string GetPrefsKey(int elementIndex) => DAILY_BONUS_ELEMENT_LOCATION_KEY + elementIndex;

        bool HasSavedLocation(int cycleNumber, int elementIndex) => CurrentCycleSavedInPlayerPrefs(cycleNumber) && PlayerPrefs.HasKey(GetPrefsKey(elementIndex));

        bool CurrentCycleSavedInPlayerPrefs(int cycleNumber) => PlayerPrefs.HasKey(DAILY_BONUS_CYCLE_KEY) && PlayerPrefs.GetInt(DAILY_BONUS_CYCLE_KEY).Equals(cycleNumber);

        Vector2 CreateRandomLocation(int elementZoneIndex, List<List<Vector2>> zoneToPossiblePositions) {
            List<Vector2> list = zoneToPossiblePositions[elementZoneIndex];

            if (list.Count == 0) {
                return Vector2.zero;
            }

            int index = (int)Random.value * (list.Count - 1);
            Vector2 result = list[index];
            list.RemoveAt(index);
            return result;
        }

        void UpdateZoneRadiusView(int zoneIndex) {
            zoneIndex = Math.Min(zoneIndex, zones.Count - 1);
            int num = -1;

            for (int i = 0; i < zones.Count; i++) {
                if (zones[i].activeSelf) {
                    num = i;
                    break;
                }
            }

            if (num == zoneIndex) {
                return;
            }

            if (num >= 0) {
                GameObject activeZone = zones[num];

                activeZone.GetComponent<AnimationEventListener>().SetHideHandler(delegate {
                    activeZone.SetActive(false);
                });

                activeZone.GetComponent<Animator>().SetTrigger("hide");
            }

            zones[zoneIndex].SetActive(true);
        }

        public void UpdateInteractable(DailyBonusTeleportState state) {
            foreach (MapViewBonusElement bonusElement in bonusElements) {
                bonusElement.Interactable = state == DailyBonusTeleportState.Active;
            }
        }
    }
}