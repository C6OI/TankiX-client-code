using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class FreeCameraActions : InputActionBase {
        public static readonly string FORWARD_MOVING = "forward_moving";

        public static readonly string BACK_MOVING = "back_moving";

        public static readonly string LEFT_MOVING = "left_moving";

        public static readonly string RIGHT_MOVING = "right_moving";

        public static readonly string UP_MOVING = "up_moving";

        public static readonly string DOWN_MOVING = "down_moving";

        public static readonly string LEFT_TURN = "left_turn";

        public static readonly string RIGHT_TURN = "right_turn";

        public static readonly string UP_TURN = "up_turn";

        public static readonly string DOWN_TURN = "down_turn";

        public static readonly string FIXED_HEIGHT = "fixed_height";

        public static readonly string MOUSE_BUTTON_DOWN = "mouse_button_down";

        public static readonly string MOUSE_SCROLL_WHEEL_UP = "mouse_scroll_wheel_up";

        public static readonly string MOUSE_SCROLL_WHEEL_DOWN = "mouse_scroll_wheel_down";
    }
}