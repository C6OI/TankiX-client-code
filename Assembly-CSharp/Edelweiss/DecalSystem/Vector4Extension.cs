using UnityEngine;

namespace Edelweiss.DecalSystem {
    internal static class Vector4Extension {
        public static bool Approximately(Vector4 a_Vector1, Vector4 a_Vector2) => Mathf.Approximately(a_Vector1.x, a_Vector2.x) && Mathf.Approximately(a_Vector1.y, a_Vector2.y) &&
                                                                                  Mathf.Approximately(a_Vector1.z, a_Vector2.z) && Mathf.Approximately(a_Vector1.w, a_Vector2.w);

        public static bool Approximately(Vector4 a_Vector1, Vector4 a_Vector2, float a_MaximumAbsoluteError, float a_MaximumRelativeError) =>
            MathfExtension.Approximately(a_Vector1.x, a_Vector2.x, a_MaximumAbsoluteError, a_MaximumRelativeError) &&
            MathfExtension.Approximately(a_Vector1.y, a_Vector2.y, a_MaximumAbsoluteError, a_MaximumRelativeError) &&
            MathfExtension.Approximately(a_Vector1.z, a_Vector2.z, a_MaximumAbsoluteError, a_MaximumRelativeError) &&
            MathfExtension.Approximately(a_Vector1.w, a_Vector2.w, a_MaximumAbsoluteError, a_MaximumRelativeError);
    }
}