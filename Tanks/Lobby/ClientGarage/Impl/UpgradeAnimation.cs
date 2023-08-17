using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    [RequireComponent(typeof(Animator))]
    public class UpgradeAnimation : MonoBehaviour {
        public void Upgrade() => GetComponent<Animator>().SetTrigger("Upgrade");
    }
}