using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientHUD.API;
using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Battle.ClientHUD.Impl {
    [RequireComponent(typeof(NormalizedAnimatedValue))]
    public class AnimatedMoneyIndicator : AnimatedIndicatorWithFinishComponent<PersonalBattleResultMoneyIndicatorFinishedComponent> {
        [SerializeField] UserMoneyIndicatorComponent indicator;

        [SerializeField] Text deltaValue;

        NormalizedAnimatedValue animation;

        long Money { get; set; }

        void Awake() {
            animation = GetComponent<NormalizedAnimatedValue>();
        }

        void Update() {
            if (Money > 0) {
                indicator.Suspend((long)(Money * (1f - animation.value)));
                deltaValue.text = "+" + ((long)(animation.value * Money)).ToStringSeparatedByThousands();
                TryToSetAnimationFinished(animation.value, 1f);
            } else {
                TryToSetAnimationFinished();
            }
        }

        public void Init(Entity screenEntity, long money) {
            SetEntity(screenEntity);
            Money = money;
            deltaValue.text = string.Empty;
            GetComponent<Animator>().SetTrigger("Start");
        }
    }
}