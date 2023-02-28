using System;
using System.Collections;
using System.Collections.Generic;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class ItemContainerComponent : BehaviourComponent {
        [SerializeField] GameObject itemContainer;

        [SerializeField] GameObject itemPrefab;

        protected void InstantiateItems(List<SpecialOfferItem> items) {
            foreach (SpecialOfferItem item in items) {
                GameObject gameObject = Instantiate(itemPrefab, itemContainer.transform, false);
                SpecialOfferItemUiComponent component = gameObject.GetComponent<SpecialOfferItemUiComponent>();
                component.title.text = item.Title;

                if (item.Quantity == 0) {
                    component.quantity.enabled = false;
                } else {
                    component.quantity.text = "x" + item.Quantity;
                }

                if (item.RibbonLabel == string.Empty) {
                    component.ribbon.gameObject.SetActive(false);
                } else {
                    component.ribbon.gameObject.SetActive(true);
                    component.ribbonLabel.text = item.RibbonLabel;
                }

                component.imageSkin.SpriteUid = item.SpriteUid;
            }
        }

        protected void ClearItems() {
            IEnumerator enumerator = itemContainer.transform.GetEnumerator();

            try {
                while (enumerator.MoveNext()) {
                    Transform transform = (Transform)enumerator.Current;
                    Destroy(transform.gameObject);
                }
            } finally {
                IDisposable disposable;

                if ((disposable = enumerator as IDisposable) != null) {
                    disposable.Dispose();
                }
            }
        }
    }
}