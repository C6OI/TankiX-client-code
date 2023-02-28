using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class AvatarDialogComponent : ConfirmDialogComponent {
        [SerializeField] Button cancelButton;

        [SerializeField] Button closeButton;

        void Awake() {
            cancelButton.onClick.AddListener(Hide);
            closeButton.onClick.AddListener(Hide);
        }
    }
}