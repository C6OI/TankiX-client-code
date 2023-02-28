using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class NewsContainerComponent : BehaviourComponent {
        public GameObject newsItemPrefab;

        public RectTransform smallItems;

        public RectTransform mediumItems;

        public RectTransform largeItems;

        public RectTransform row1;

        public RectTransform row2;

        void Update() {
            bool active = false;
            bool active2 = false;

            if (smallItems.childCount > 0) {
                smallItems.gameObject.SetActive(true);
                active = true;
            } else {
                smallItems.gameObject.SetActive(false);
            }

            if (mediumItems.childCount > 0) {
                mediumItems.gameObject.SetActive(true);
                active = true;
            } else {
                mediumItems.gameObject.SetActive(false);
            }

            if (largeItems.childCount > 0) {
                largeItems.gameObject.SetActive(true);
                active2 = true;
            } else {
                largeItems.gameObject.SetActive(false);
            }

            row1.gameObject.SetActive(active);
            row2.gameObject.SetActive(active2);
        }

        public Transform GetContainerTransform(NewsItemLayout layout) {
            switch (layout) {
                case NewsItemLayout.SMALL:
                    return smallItems;

                case NewsItemLayout.MEDIUM:
                    return mediumItems;

                case NewsItemLayout.LARGE:
                    return largeItems;

                default:
                    return null;
            }
        }
    }
}