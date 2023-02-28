namespace Tanks.Battle.ClientCore.Impl {
    public class CameraRotationActions : InputActionBase {
        public static readonly string ROTATE_CAMERA_UP_LEFT = "rotate_camera_up_left";

        public static readonly string ROTATE_CAMERA_DOWN_RIGHT = "rotate_camera_down_right";

        public static readonly string MOUSEWHEEL_MOVE = "move and rotate camera by mouse wheel";

        public static readonly string MOUSEX_ROTATE = "rotate by mouse x";

        public static readonly string MOUSEY_ROTATE = "rotate by mouse y";

        public static readonly string MOUSEY_MOVE_SHAFT_AIM = "move shaft aim by mouse y";
    }
}