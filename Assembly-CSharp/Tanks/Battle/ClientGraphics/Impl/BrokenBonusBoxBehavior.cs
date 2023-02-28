using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class BrokenBonusBoxBehavior : MonoBehaviour {
        [SerializeField] GameObject brokenBonusGameObject;

        public GameObject BrokenBonusGameObject => brokenBonusGameObject;
    }
}