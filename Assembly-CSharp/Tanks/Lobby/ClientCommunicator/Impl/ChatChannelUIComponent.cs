using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    public class ChatChannelUIComponent : BehaviourComponent, AttachToEntityListener {
        [SerializeField] GameObject tab;

        [SerializeField] Color selectedColor;

        [SerializeField] Color unselectedColor;

        [SerializeField] Image back;

        [SerializeField] ImageSkin icon;

        [SerializeField] new TMP_Text name;

        [SerializeField] GameObject badge;

        [SerializeField] TMP_Text counterText;

        int counter;
        Entity entity;

        public GameObject Tab {
            get => tab;
            set => tab = value;
        }

        public string Name {
            get => name.text;
            set => name.text = value;
        }

        public int Unread {
            get => counter;
            set {
                counter = value;
                badge.SetActive(counter > 0);
                counterText.text = counter.ToString();
            }
        }

        public void AttachedToEntity(Entity entity) {
            this.entity = entity;
        }

        public void SetIcon(string spriteUid) {
            icon.SpriteUid = spriteUid;
        }

        public string GetSpriteUid() => icon.SpriteUid;

        public void Select() {
            back.color = selectedColor;
        }

        public void Deselect() {
            back.color = unselectedColor;
        }

        public void OnClick() {
            if (entity != null) {
                EngineService.Engine.ScheduleEvent(new SelectChannelEvent(), entity);
            }
        }
    }
}