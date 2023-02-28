using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class TutorialWeaponHideTrigger : TutorialHideTriggerComponent {
        [SerializeField] float showTime = 5f;

        float timer;

        void Update() {
            timer += Time.deltaTime;

            if (timer >= showTime) {
                Triggered();
            }
        }

        void OnEnable() {
            timer = 0f;
        }
    }
}