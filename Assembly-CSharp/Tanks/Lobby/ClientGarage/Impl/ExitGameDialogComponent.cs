using System.Collections.Generic;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientUserProfile.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ExitGameDialogComponent : BehaviourComponent {
        public GameObject content;

        public TextMeshProUGUI timer;

        public List<long> ReceivedRewards;

        [SerializeField] GameObject ContainerView;

        [SerializeField] GameObject DetailView;

        [SerializeField] GameObject XCryView;

        [SerializeField] GameObject CryView;

        [SerializeField] GameObject EnergyView;

        [SerializeField] GameObject row1;

        [SerializeField] GameObject row2;

        public GameObject[] textNotReady;

        public GameObject textReady;

        public List<DailyBonusData> dataList;

        public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        void OnDisable() {
            row1.transform.DestroyChildren();
            row2.transform.DestroyChildren();
        }

        public void InstantiateCryBonus(long amount) {
            GameObject gameObject = Instantiate(CryView, row1.transform);
            gameObject.GetComponent<MultipleBonusView>().UpdateView(amount);
        }

        public void InstantiateXCryBonus(long amount) {
            GameObject gameObject = Instantiate(XCryView, row1.transform);
            gameObject.GetComponent<MultipleBonusView>().UpdateView(amount);
        }

        public void InstantiateEnergyBonus(long amount) {
            GameObject gameObject = Instantiate(EnergyView, row1.transform);
            gameObject.GetComponent<MultipleBonusView>().UpdateView(amount);
        }

        public void InstantiateDetailBonus(long marketItem) {
            GameObject gameObject = Instantiate(DetailView, row1.transform);
            gameObject.GetComponent<DetailBonusView>().UpdateViewByMarketItem(marketItem);
            gameObject.GetComponent<Animator>().SetTrigger("show");
        }

        public void InstantiateContainerBonus(long marketItem) {
            GameObject gameObject = Instantiate(ContainerView, row1.transform);
            gameObject.GetComponent<ContainerBonusView>().UpdateViewByMarketItem(marketItem);
            gameObject.GetComponent<Animator>().SetTrigger("show");
        }
    }
}