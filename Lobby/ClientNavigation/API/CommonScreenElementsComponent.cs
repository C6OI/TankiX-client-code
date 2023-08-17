using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientNavigation.API {
    public class CommonScreenElementsComponent : MonoBehaviour, ISerializationCallbackReceiver, Component {
        [SerializeField] List<string> names;

        [SerializeField] List<Animator> items;

        readonly Dictionary<string, Animator> itemsMap = new();

        public void OnBeforeSerialize() {
            names = itemsMap.Keys.ToList();
            items = itemsMap.Values.ToList();
        }

        public void OnAfterDeserialize() {
            itemsMap.Clear();

            for (int i = 0; i < names.Count; i++) {
                itemsMap.Add(names[i], items[i]);
            }
        }

        public void ShowItem(string name) => itemsMap[name].SetBool("Visible", true);

        public void HideItem(string name) => itemsMap[name].SetBool("Visible", false);

        public void ActivateItems(ICollection<string> names) {
            foreach (KeyValuePair<string, Animator> item in itemsMap) {
                if (item.Value != null && item.Value.gameObject.activeSelf) {
                    item.Value.SetBool("Visible", false);
                }
            }

            foreach (string name in names) {
                if (!itemsMap.ContainsKey(name)) {
                    throw new ArgumentException("TopPanel item with name " + name + " not found!");
                }

                Animator animator = itemsMap[name];

                if (animator != null) {
                    animator.gameObject.SetActive(true);
                    animator.SetBool("Visible", true);
                }
            }
        }
    }
}