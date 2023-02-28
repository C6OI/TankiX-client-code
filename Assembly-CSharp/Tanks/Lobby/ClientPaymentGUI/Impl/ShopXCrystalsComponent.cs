using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientPayment.Impl;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class ShopXCrystalsComponent : PurchaseItemComponent {
        [SerializeField] XCrystalPackage packPrefab;

        [SerializeField] RectTransform packsRoot;

        readonly Dictionary<long, XCrystalPackage> packs = new();

        void Awake() {
            RectMask2D componentInParent = GetComponentInParent<RectMask2D>();

            if (componentInParent != null) {
                componentInParent.enabled = false;
            }
        }

        public void AddPackage(Entity entity, List<string> images) {
            if (packs.ContainsKey(entity.Id)) {
                Destroy(packs[entity.Id].gameObject);
                packs.Remove(entity.Id);
            }

            XCrystalPackage xCrystalPackage = Instantiate(packPrefab);
            xCrystalPackage.transform.SetParent(packsRoot, false);

            xCrystalPackage.GetComponentInChildren<Button>().onClick.AddListener(delegate {
                OnPackClick(entity);
            });

            packs.Add(entity.Id, xCrystalPackage);
            xCrystalPackage.Init(entity, images);
            xCrystalPackage.UpdateData();
        }

        void OnPackClick(Entity entity) {
            OnPackClick(entity, true);
        }

        public void Arange() {
            List<XCrystalPackage> list = packs.Values.ToList();

            list.Sort(delegate(XCrystalPackage a, XCrystalPackage b) {
                if (!a.Entity.HasComponent<XCrystalsPackComponent>()) {
                    return -1;
                }

                return !b.Entity.HasComponent<XCrystalsPackComponent>() ? 1
                           : a.Entity.GetComponent<XCrystalsPackComponent>().Amount.CompareTo(b.Entity.GetComponent<XCrystalsPackComponent>().Amount);
            });

            for (int i = 0; i < list.Count; i++) {
                list[i].transform.SetSiblingIndex(i);
            }
        }

        public void Clear() {
            foreach (KeyValuePair<long, XCrystalPackage> pack in packs) {
                Destroy(pack.Value.gameObject);
            }

            packs.Clear();
            methods.Clear();
        }

        public void UpdatePackage(Entity entity) {
            shopDialogs.Cancel();

            if (packs.ContainsKey(entity.Id)) {
                packs[entity.Id].UpdateData();
            }
        }
    }
}