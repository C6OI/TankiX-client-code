using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.src.main.csharp.Impl.OSGi {
    public class BattleLabelActivator : UnityAwareActivator<AutoCompleting> {
        [SerializeField] public GameObject BattleLabel;

        protected override void Activate() {
            if (BattleLabel == null) {
                Debug.LogError("BattleLabelActivator: not set prefab UserLabel");
            } else {
                BattleLabelBuilder.battleLabelPrefab = BattleLabel;
            }
        }
    }
}