using UnityEngine;

namespace Platform.Library.ClientUnityIntegration.API {
    public static class InputMapping {
        static readonly string HORIZONTAL = "Horizontal";

        static readonly string VERTICAL = "Vertical";

        static readonly string CANCEL = "Cancel";

        static readonly string SUBMIT = "Submit";

        public static float Horizontal => Input.GetAxis(HORIZONTAL);

        public static float Vertical => Input.GetAxis(VERTICAL);

        public static bool Cancel => Input.GetButtonDown(CANCEL);

        public static bool Submit => Input.GetButtonDown(SUBMIT);
    }
}