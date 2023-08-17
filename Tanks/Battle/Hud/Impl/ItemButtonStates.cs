using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.Hud.Impl {
    public class ItemButtonStates {
        public class ItemButtonEnabledStateNode : Node {
            public ItemButtonEnabledStateComponent itemButtonEnabledState;
        }

        public class ItemButtonDisabledStateNode : Node {
            public ItemButtonDisabledStateComponent itemButtonDisabledState;
        }

        public class ItemButtonActivatedStateNode : Node {
            public ItemButtonActivatedStateComponent itemButtonActivatedState;
        }

        public class ItemButtonCooldownStateNode : Node {
            public ItemButtonCooldownStateComponent itemButtonCooldownState;
        }
    }
}