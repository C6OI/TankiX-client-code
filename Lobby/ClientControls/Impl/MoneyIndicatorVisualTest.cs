using System.Collections;
using Lobby.ClientControls.API;
using UnityEngine;

namespace Lobby.ClientControls.Impl {
    public class MoneyIndicatorVisualTest : MonoBehaviour {
        public UserMoneyIndicatorComponent userMoneyIndicator;

        void Start() => StartCoroutine(Test());

        IEnumerator Test() {
            while (true) {
                yield return new WaitForSeconds(Random.Range(2, 10));

                userMoneyIndicator.SetMoneyAnimated(Random.Range(0, 99999999));
            }
        }
    }
}