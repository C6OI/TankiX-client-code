namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class SpecialOfferItem {
        public SpecialOfferItem() { }

        public SpecialOfferItem(int quantity, string spriteUid, string title)
            : this(quantity, spriteUid, title, string.Empty) { }

        public SpecialOfferItem(int quantity, string spriteUid, string title, string ribbonLabel) {
            Quantity = quantity;
            SpriteUid = spriteUid;
            Title = title;
            RibbonLabel = ribbonLabel;
        }

        public int Quantity { get; set; }

        public string SpriteUid { get; set; }

        public string Title { get; set; }

        public string RibbonLabel { get; set; }
    }
}