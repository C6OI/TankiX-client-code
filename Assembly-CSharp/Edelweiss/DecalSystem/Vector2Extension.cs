using UnityEngine;

namespace Edelweiss.DecalSystem {
    internal static class Vector2Extension {
        public static bool Approximately(Vector2 a_Vector1, Vector2 a_Vector2) => Mathf.Approximately(a_Vector1.x, a_Vector2.x) && Mathf.Approximately(a_Vector1.y, a_Vector2.y);

        public static bool Approximately(Vector2 a_Vector1, Vector2 a_Vector2, float a_MaximumAbsoluteError, float a_MaximumRelativeError) =>
            MathfExtension.Approximately(a_Vector1.x, a_Vector2.x, a_MaximumAbsoluteError, a_MaximumRelativeError) &&
            MathfExtension.Approximately(a_Vector1.y, a_Vector2.y, a_MaximumAbsoluteError, a_MaximumRelativeError);
    }
}