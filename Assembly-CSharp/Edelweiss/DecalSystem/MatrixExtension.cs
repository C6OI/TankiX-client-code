using UnityEngine;

namespace Edelweiss.DecalSystem {
    public static class MatrixExtension {
        public static float Determinant(this Matrix4x4 a_Matrix) =>
            a_Matrix.m00 * (a_Matrix.m11 * (a_Matrix.m22 * a_Matrix.m33 - a_Matrix.m32 * a_Matrix.m23) - a_Matrix.m12 * (a_Matrix.m21 * a_Matrix.m33 - a_Matrix.m31 * a_Matrix.m23) +
                            a_Matrix.m13 * (a_Matrix.m21 * a_Matrix.m32 - a_Matrix.m31 * a_Matrix.m22)) -
            a_Matrix.m01 * (a_Matrix.m10 * (a_Matrix.m22 * a_Matrix.m33 - a_Matrix.m32 * a_Matrix.m23) - a_Matrix.m12 * (a_Matrix.m20 * a_Matrix.m33 - a_Matrix.m30 * a_Matrix.m23) +
                            a_Matrix.m13 * (a_Matrix.m20 * a_Matrix.m32 - a_Matrix.m30 * a_Matrix.m22)) +
            a_Matrix.m02 * (a_Matrix.m10 * (a_Matrix.m21 * a_Matrix.m33 - a_Matrix.m31 * a_Matrix.m23) - a_Matrix.m11 * (a_Matrix.m20 * a_Matrix.m33 - a_Matrix.m30 * a_Matrix.m23) +
                            a_Matrix.m13 * (a_Matrix.m20 * a_Matrix.m31 - a_Matrix.m30 * a_Matrix.m21)) - a_Matrix.m03 *
            (a_Matrix.m10 * (a_Matrix.m21 * a_Matrix.m32 - a_Matrix.m31 * a_Matrix.m22) - a_Matrix.m11 * (a_Matrix.m20 * a_Matrix.m32 - a_Matrix.m30 * a_Matrix.m22) +
             a_Matrix.m12 * (a_Matrix.m20 * a_Matrix.m31 - a_Matrix.m30 * a_Matrix.m21));

        public static Matrix4x4 Lerp(Matrix4x4 a_From, Matrix4x4 a_To, float a_Value) {
            Matrix4x4 result = default;

            for (int i = 0; i < 16; i++) {
                result[i] = Mathf.Lerp(a_From[i], a_To[i], a_Value);
            }

            return result;
        }
    }
}