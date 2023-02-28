using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.Impl {
    public class CaptchaSystem : ECSSystem {
        [OnEventFire]
        public void SendUpdateEvent(NodeAddedEvent e, SingleNode<CaptchaComponent> node) {
            ScheduleEvent<UpdateCaptchaEvent>(node);
        }

        [OnEventFire]
        public void SendUpdateCaptcha(ButtonClickEvent e, SingleNode<CaptchaComponent> node) {
            ScheduleEvent<UpdateCaptchaEvent>(node);
        }

        [OnEventFire]
        public void TransitionToWaitState(ShowCaptchaWaitAnimationEvent e, SingleNode<CaptchaComponent> node) {
            node.component.Animator.SetTrigger("Wait");
        }

        [OnEventFire]
        public void ParseCaptcha(NodeAddedEvent e, CaptchaNode node) {
            node.captcha.Animator.SetTrigger("Normal");
            byte[] bytes = node.captchaBytes.bytes;
            Texture2D texture2D = new(1, 1);
            texture2D.LoadImage(bytes);
            Sprite captchaSprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), default);
            node.captcha.CaptchaSprite = captchaSprite;
            node.Entity.RemoveComponent<CaptchaBytesComponent>();
        }

        public class CaptchaNode : Node {
            public CaptchaComponent captcha;

            public CaptchaBytesComponent captchaBytes;
        }
    }
}