using System.Collections.Generic;
using System.Runtime.InteropServices;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class SummaryBonusComponent : LocalizedControl, Component {
        [SerializeField] List<StaticBonusUI> bonuses = new();

        [SerializeField] Text totalBonusText;

        readonly Dictionary<MapKey, StaticBonusUI> effectToInstance = new();

        int usedBonuses;

        [Inject] public new static EngineService EngineService { get; set; }

        public string TotalBonusText {
            set => totalBonusText.text = value;
        }

        protected override void Awake() {
            base.Awake();

            if (usedBonuses == 0) {
                gameObject.SetActive(false);
            }
        }

        void OnDisable() {
            foreach (StaticBonusUI bonuse in bonuses) {
                bonuse.gameObject.SetActive(false);
            }

            effectToInstance.Clear();
            usedBonuses = 0;
            gameObject.SetActive(false);
        }

        static string GetItemIcon(long marketItem) {
            string empty = string.Empty;
            Entity entity = Flow.Current.EntityRegistry.GetEntity(marketItem);
            return entity.GetComponent<ItemIconComponent>().SpriteUid;
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        struct MapKey {
            public long MarketItem { get; set; }
        }
    }
}