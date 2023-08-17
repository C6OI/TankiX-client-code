using UnityEngine;

namespace Lobby.ClientControls.API {
    public interface ILazyList {
        int ItemsCount { get; set; }

        IndexRange VisibleItemsRange { get; }

        RectTransform GetItemContent(int itemIndex);

        void SetItemContent(int itemIndex, RectTransform content);

        void Scroll(int deltaItems);

        void ClearItems();
    }
}