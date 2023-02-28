using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class BackhitDamageTutorialTrigger : MonoBehaviour {
        [SerializeField] float showDelay = 60f;

        [HideInInspector] public bool canShow;

        float timer;

        void Update() {
            timer += Time.deltaTime;

            if (timer >= showDelay) {
                canShow = true;
                GetComponent<TutorialShowTriggerComponent>().Triggered();
                enabled = false;
            }
        }

        void OnEnable() {
            timer = 0f;
        }
    }
}