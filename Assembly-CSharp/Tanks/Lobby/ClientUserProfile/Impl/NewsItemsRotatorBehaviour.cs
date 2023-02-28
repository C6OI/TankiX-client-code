using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class NewsItemsRotatorBehaviour : UIBehaviour {
        public float swapPeriod = 10f;

        public float swapTime = 0.5f;

        public bool swapTrigger;

        public RectMask2D mask;

        float swapBeginTime;

        bool swapping;

        void Update() {
            if (swapTrigger || Time.time >= swapBeginTime + swapPeriod || IsManualSwap()) {
                SwapItems();
                swapTrigger = false;
            }

            float num = 0f;
            float itemSize = 0f;

            if (transform.childCount > 1) {
                RectTransform rectTransform = (RectTransform)transform.GetChild(0);
                itemSize = rectTransform.rect.width;

                if (swapping) {
                    if (Time.time < swapBeginTime + swapTime) {
                        num = (Time.time - swapBeginTime) / swapTime;
                    } else {
                        swapping = false;
                        rectTransform.SetAsLastSibling();
                        rectTransform.gameObject.SetActive(false);
                        mask.enabled = false;
                    }
                }
            }

            for (int i = 0; i < transform.childCount; i++) {
                float offset = i - num;
                Transform child = transform.GetChild(i);
                int num2;

                switch (i) {
                    case 1:
                        num2 = swapping ? 1 : 0;
                        break;

                    default:
                        num2 = 0;
                        break;

                    case 0:
                        num2 = 1;
                        break;
                }

                bool flag = (byte)num2 != 0;

                if (flag != child.gameObject.activeSelf) {
                    child.gameObject.SetActive(flag);

                    if (flag) {
                        child.GetComponent<EntityBehaviour>().Entity.GetComponent<NewsItemComponent>().ShowCount++;
                    }
                }

                SetOffset(child, itemSize, offset);
                OverlapFix(child);
            }
        }

        protected override void OnEnable() {
            swapBeginTime = Time.time;
        }

        public void SwapItems() {
            if (transform.childCount >= 2 && !swapping) {
                RectTransform rectTransform = (RectTransform)transform.GetChild(0);
                NewsItemUIComponent component = rectTransform.GetComponent<NewsItemUIComponent>();

                if (!component.noSwap || IsManualSwap()) {
                    swapBeginTime = Time.time;
                    swapping = true;
                    mask.enabled = true;
                }
            }
        }

        void OverlapFix(Transform child) {
            if (swapping && child.gameObject.activeSelf) {
                child.GetComponent<Animator>().SetTrigger("Normal");
            }
        }

        void SetOffset(Transform child, float itemSize, float offset) {
            RectTransform rectTransform = (RectTransform)child;
            float num = itemSize * offset;
            Vector2 anchoredPosition = rectTransform.anchoredPosition;

            if (!Mathf.Approximately(anchoredPosition.x, num)) {
                anchoredPosition.x = num;
                rectTransform.anchoredPosition = anchoredPosition;
            }
        }

        static bool IsManualSwap() => Input.GetMouseButtonDown(1);
    }
}