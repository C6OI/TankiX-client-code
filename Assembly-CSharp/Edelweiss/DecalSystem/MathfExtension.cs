using UnityEngine;

namespace Edelweiss.DecalSystem {
    internal static class MathfExtension {
        public static bool Approximately(float a_Float1, float a_Float2, float a_MaximumAbsoluteError, float a_MaximumRelativeError) {
            bool result = false;
            float num = Mathf.Abs(a_Float1 - a_Float2);

            if (num <= a_MaximumAbsoluteError) {
                result = true;
            } else {
                float a = Mathf.Abs(a_Float1);
                float b = Mathf.Abs(a_Float2);
                float num2 = Mathf.Max(a, b);

                if (num <= num2 * a_MaximumRelativeError) {
                    result = true;
                }
            }

            return result;
        }
    }
}