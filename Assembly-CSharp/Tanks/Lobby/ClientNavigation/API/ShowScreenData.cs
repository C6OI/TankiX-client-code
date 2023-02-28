using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;

namespace Tanks.Lobby.ClientNavigation.API {
    public class ShowScreenData {
        public ShowScreenData(Type screenType, AnimationDirection animationDirection = AnimationDirection.NONE) {
            ScreenType = screenType;
            AnimationDirection = animationDirection;
        }

        [Inject] public static EngineService EngineService { get; set; }

        public Type ScreenType { get; set; }

        public Entity Context { get; set; }

        public bool AutoDeleteContext { get; set; }

        public AnimationDirection AnimationDirection { get; set; }

        public ShowScreenData InvertAnimationDirection(AnimationDirection animationDirection) {
            switch (animationDirection) {
                case AnimationDirection.UP:
                    AnimationDirection = AnimationDirection.DOWN;
                    break;

                case AnimationDirection.DOWN:
                    AnimationDirection = AnimationDirection.UP;
                    break;

                case AnimationDirection.LEFT:
                    AnimationDirection = AnimationDirection.RIGHT;
                    break;

                case AnimationDirection.RIGHT:
                    AnimationDirection = AnimationDirection.LEFT;
                    break;
            }

            return this;
        }

        public void FreeContext() {
            if (Context != null && AutoDeleteContext) {
                EngineService.Engine.DeleteEntity(Context);
            }
        }

        public override string ToString() => ScreenType.Name + " Context: " + (Context == null ? "null" : Context.Id.ToString()) + " AutoDeleteContext: " + AutoDeleteContext;
    }
}