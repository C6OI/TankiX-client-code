using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    public class RequestInfoForItemsEvent : Event {
        public readonly List<long> itemIds;

        public readonly Dictionary<long, string> previews = new();

        public readonly ICollection<long> purchased = new HashSet<long>();

        public readonly Dictionary<long, ItemRarityType> rarities = new();

        public readonly Dictionary<long, bool> rarityFrames = new();

        public readonly Dictionary<long, string> titles = new();

        public string crystalSprite;

        public string crystalTitle;

        public int mainItemCount;

        public bool mainItemCrystal;

        public string mainItemDescription;

        public long mainItemId;

        public string mainItemSprite;

        public string mainItemTitle;

        public bool mainItemXCrystal;

        public string xCrystalSprite;

        public string xCrystalTitle;

        public RequestInfoForItemsEvent(List<long> itemIds) => this.itemIds = itemIds;
    }
}