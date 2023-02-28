using UnityEngine;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public interface ContentWithOrder {
        int Order { get; }

        bool CanFillBigRow { get; }

        bool CanFillSmallRow { get; }

        void SetParent(Transform parent);
    }
}