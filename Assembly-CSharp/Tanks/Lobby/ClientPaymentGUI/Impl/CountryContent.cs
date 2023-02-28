using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class CountryContent : MonoBehaviour, ListItemContent {
        [SerializeField] Text countryName;

        [SerializeField] ImageListSkin flag;

        KeyValuePair<string, string> data;

        [Inject] public static EngineServiceInternal EngineService { get; set; }

        public void SetDataProvider(object data) {
            this.data = (KeyValuePair<string, string>)data;
            countryName.text = this.data.Value;
        }

        public void Select() {
            EngineService.Engine.ScheduleEvent(new SelectCountryEvent {
                CountryCode = data.Key,
                CountryName = data.Value
            }, EngineService.EntityStub);
        }
    }
}