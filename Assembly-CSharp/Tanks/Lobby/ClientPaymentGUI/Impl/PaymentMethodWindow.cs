using System;
using System.Collections;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.Impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class PaymentMethodWindow : MonoBehaviour {
        static readonly int CancelHash = Animator.StringToHash("cancel");

        [SerializeField] RectTransform methodsRoot;

        [SerializeField] TextMeshProUGUI info;

        [SerializeField] TextMeshProUGUI description;

        [SerializeField] PaymentMethodContent methodPrefab;

        Action onBack;

        Action<Entity> onForward;

        void OnDisable() {
            IEnumerator enumerator = methodsRoot.GetEnumerator();

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

        public void Show(Entity item, List<Entity> methods, Action onBack, Action<Entity> onForward, string desc = "") {
            MainScreenComponent.Instance.OverrideOnBack(Cancel);
            this.onBack = onBack;
            this.onForward = onForward;
            gameObject.SetActive(true);
            info.text = ShopDialogs.FormatItem(item);

            if (!string.IsNullOrEmpty(desc)) {
                description.text = "\n" + desc;
                description.gameObject.SetActive(true);
            } else {
                description.text = desc;
                description.gameObject.SetActive(false);
            }

            foreach (Entity method in methods) {
                PaymentMethodContent paymentMethodContent = Instantiate(methodPrefab, methodsRoot, false);
                paymentMethodContent.SetDataProvider(method);
                Entity target = method;

                paymentMethodContent.GetComponent<Button>().onClick.AddListener(delegate {
                    MainScreenComponent.Instance.ClearOnBackOverride();
                    GetComponent<Animator>().SetTrigger(CancelHash);
                    onForward(target);
                });
            }
        }

        public void Cancel() {
            MainScreenComponent.Instance.ClearOnBackOverride();
            GetComponent<Animator>().SetTrigger(CancelHash);
            onBack();
        }
    }
}