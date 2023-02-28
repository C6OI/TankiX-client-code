using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Battle.ClientHUD.Impl {
    [RequireComponent(typeof(Animator))]
    public class EffectHUDComponent : BehaviourComponent, AttachToEntityListener, DetachFromEntityListener {
        [SerializeField] ImageSkin icon;

        [SerializeField] Image indicator;

        [SerializeField] Image indicatorLighting;

        [SerializeField] Image durationProgress;

        [SerializeField] PaletteColorField buffColor;

        [SerializeField] PaletteColorField debuffColor;

        [SerializeField] TextMeshProUGUI timerText;

        float duration;

        Entity entity;

        float lastTimer = -1f;

        bool ticking;

        float timer;

        void Update() {
            if (!ticking) {
                return;
            }

            timer += Time.deltaTime;
            timer = Mathf.Min(timer, duration);

            if (timer != lastTimer) {
                lastTimer = timer;
                float num = 1f - timer / duration;
                SetFillAmount(durationProgress, num);

                if (num <= 0f) {
                    ticking = false;
                }

                SetTimerText();
            }
        }

        void OnDisable() {
            if (EngineService != null) {
                if (entity != null && entity.HasComponent<EffectHUDComponent>()) {
                    entity.RemoveComponent<EffectHUDComponent>();
                }

                Destroy(gameObject);
            }
        }

        public void AttachedToEntity(Entity entity) {
            this.entity = entity;
        }

        public void DetachedFromEntity(Entity entity) {
            this.entity = null;
        }

        public void InitBuff(string icon) {
            Init(buffColor, icon);
        }

        public void InitDebuff(string icon) {
            Init(debuffColor, icon);
        }

        public void InitDuration(float duration) {
            SetFillAmount(durationProgress, 1f);
            this.duration = duration;
            timer = 0f;
            ticking = duration != 0f;
            SetTimerText();
        }

        void Init(PaletteColorField color, string icon) {
            Color color2 = color.Color;
            color2.a = 1f;
            indicator.color = color2;
            indicatorLighting.color = color2;
            this.icon.SpriteUid = icon;
        }

        void SetFillAmount(Image image, float fillAmount) {
            image.rectTransform.anchorMax = new Vector2(1f, fillAmount);
        }

        void SetTimerText() {
            timerText.text = string.Format("{0:0}", duration - timer);
        }

        public void Kill() {
            GetComponent<Animator>().SetTrigger("Kill");
        }

        void OnReadyToDie() {
            gameObject.SetActive(false);
        }

        public void SetAllDirty() {
            Graphic[] componentsInChildren = GetComponentsInChildren<Graphic>(true);

            foreach (Graphic graphic in componentsInChildren) {
                graphic.SetAllDirty();
            }
        }
    }
}