using UnityEngine;
using UnityEngine.EventSystems;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class NotificationsStackContainerComponent : UIBehaviour, Component {
        [SerializeField] float resetDelay = 15f;

        [SerializeField] float yOffset = 100f;

        float lastOffset;

        float lastResetTime;

        public GameObject CreateNotification(GameObject prefab) {
            if (lastResetTime + resetDelay < Time.time) {
                lastOffset = 0f;
                lastResetTime = Time.time;
            } else {
                lastOffset += yOffset;
            }

            GameObject gameObject = Instantiate(prefab, transform, false);
            RectTransform rectTransform = (RectTransform)gameObject.transform;
            Vector2 anchoredPosition = rectTransform.anchoredPosition;
            anchoredPosition.y += lastOffset;
            rectTransform.anchoredPosition = anchoredPosition;
            gameObject.SetActive(true);
            return gameObject;
        }
    }
}