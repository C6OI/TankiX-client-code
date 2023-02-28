using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class KillAssistTotalScoreComponent : MonoBehaviour {
        public KillAssistComponent killAssist;

        public void SetTotalNumberToZero() {
            killAssist.SetTotalNumberToZero();
        }

        public void SetVisible() {
            killAssist.SetVisible(true);
        }

        public void SetDisappearing() {
            killAssist.SetVisible(false);
        }
    }
}