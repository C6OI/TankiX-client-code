using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class LoginRewardAllItemsContainer : MonoBehaviour {
        public int currentDay;

        [SerializeField] LoginRewardItemUI itemPrefab;

        [SerializeField] LoginRewardDialog dialog;

        public void InitItems(Dictionary<int, List<LoginRewardItem>> allRewards, int currentDay) {
            this.currentDay = currentDay;

            foreach (int key in allRewards.Keys) {
                LoginRewardItemUI loginRewardItemUI = CreateDay(key);

                foreach (LoginRewardItem item in allRewards[key]) {
                    Entity entity = Flow.Current.EntityRegistry.GetEntity(item.MarketItemEntity);

                    if (!entity.HasComponent<PremiumQuestItemComponent>()) {
                        loginRewardItemUI.AddItem(entity.GetComponent<ImageItemComponent>().SpriteUid, dialog.GetRewardItemNameWithAmount(entity, item.Amount));
                    }
                }

                loginRewardItemUI.fillType = key == currentDay ? LoginRewardProgressBar.FillType.Half :
                                             currentDay > key ? LoginRewardProgressBar.FillType.Full : LoginRewardProgressBar.FillType.Empty;

                loginRewardItemUI.gameObject.SetActive(true);
            }

            CheckLines();
        }

        public LoginRewardItemUI CreateDay(int day) {
            RectTransform content = GetComponentInChildren<ScrollRect>().content;
            LoginRewardItemUI loginRewardItemUI = Instantiate(itemPrefab, content.transform);
            loginRewardItemUI.Day = day;
            return loginRewardItemUI;
        }

        public void SetCurrentDay(int day) {
            Debug.Log(string.Format("Current day: {0}", day));
        }

        public void CheckLines() {
            ScrollRect componentInChildren = GetComponentInChildren<ScrollRect>();
            LoginRewardItemUI[] componentsInChildren = componentInChildren.GetComponentsInChildren<LoginRewardItemUI>();

            for (int i = 0; i < componentsInChildren.Length; i++) {
                componentsInChildren[i].SetupLines(i == 0, i == componentsInChildren.Length - 1);
            }
        }

        public void ScrollToCurrentDay() {
            ScrollRect componentInChildren = GetComponentInChildren<ScrollRect>();
            componentInChildren.horizontalNormalizedPosition = Mathf.Max(0f, (currentDay - 2) / (float)componentInChildren.content.childCount);
        }

        public void Clear() {
            LoginRewardItemUI[] componentsInChildren = GetComponentsInChildren<LoginRewardItemUI>(true);
            LoginRewardItemUI[] array = componentsInChildren;

            foreach (LoginRewardItemUI loginRewardItemUI in array) {
                loginRewardItemUI.transform.SetParent(null);
                Destroy(loginRewardItemUI.gameObject);
            }
        }
    }
}