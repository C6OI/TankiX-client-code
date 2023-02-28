using UnityEngine;

public struct P3D_Matrix {
    public float m00;

    public float m10;

    public float m20;

    public float m01;

    public float m11;

    public float m21;

    public float m02;

    public float m12;

    public float m22;

    public static P3D_Matrix Identity {
        get {
            P3D_Matrix result = default;
            result.m00 = 1f;
            result.m10 = 0f;
            result.m20 = 0f;
            result.m01 = 0f;
            result.m11 = 1f;
            result.m21 = 0f;
            result.m02 = 0f;
            result.m12 = 0f;
            result.m22 = 1f;
            return result;
        }
    }

    public P3D_Matrix Inverse {
        get {
            double num = m00 * (m11 * m22 - m21 * m12) - m01 * (m10 * m22 - m12 * m20) + m02 * (m10 * m21 - m11 * m20);

            if (num != 0.0) {
                float num2 = (float)(1.0 / num);
                P3D_Matrix result = default;
                result.m00 = (m11 * m22 - m21 * m12) * num2;
                result.m10 = (0f - (m10 * m22 - m12 * m20)) * num2;
                result.m20 = (m10 * m21 - m20 * m11) * num2;
                result.m01 = (0f - (m01 * m22 - m02 * m21)) * num2;
                result.m11 = (m00 * m22 - m02 * m20) * num2;
                result.m12 = (0f - (m00 * m12 - m10 * m02)) * num2;
                result.m02 = (m01 * m12 - m02 * m11) * num2;
                result.m22 = (m00 * m11 - m10 * m01) * num2;
                result.m21 = (0f - (m00 * m21 - m20 * m01)) * num2;
                return result;
            }

            return Identity;
        }
    }

    public Matrix4x4 Matrix4x4 {
        get {
            Matrix4x4 identity = Matrix4x4.identity;
            identity.m00 = m00;
            identity.m10 = m10;
            identity.m20 = m20;
            identity.m01 = m01;
            identity.m11 = m11;
            identity.m21 = m21;
            identity.m02 = m02;
            identity.m12 = m12;
            identity.m22 = m22;
            return identity;
        }
    }

    public static P3D_Matrix Translation(float x, float y) {
        P3D_Matrix result = default;
        result.m00 = 1f;
        result.m10 = 0f;
        result.m20 = 0f;
        result.m01 = 0f;
        result.m11 = 1f;
        result.m21 = 0f;
        result.m02 = x;
        result.m12 = y;
        result.m22 = 1f;
        return result;
    }

    public static P3D_Matrix Scaling(float x, float y) {
        P3D_Matrix result = default;
        result.m00 = x;
        result.m10 = 0f;
        result.m20 = 0f;
        result.m01 = 0f;
        result.m11 = y;
        result.m21 = 0f;
        result.m02 = 0f;
        result.m12 = 0f;
        result.m22 = 1f;
        return result;
    }

    public static P3D_Matrix Rotation(float a) {
        float num = Mathf.Sin(a);
        float num2 = Mathf.Cos(a);
        P3D_Matrix result = default;
        result.m00 = num2;
        result.m10 = 0f - num;
        result.m20 = 0f;
        result.m01 = num;
        result.m11 = num2;
        result.m21 = 0f;
        result.m02 = 0f;
        result.m12 = 0f;
        result.m22 = 1f;
        return result;
    }

    public Vector2 MultiplyPoint(Vector2 v) {
        Vector2 result = default;
        result.x = m00 * v.x + m01 * v.y + m02;
        result.y = m10 * v.x + m11 * v.y + m12;
        return result;
    }

    public Vector2 MultiplyPoint(float x, float y) {
        Vector2 result = default;
        result.x = m00 * x + m01 * y + m02;
        result.y = m10 * x + m11 * y + m12;
        return result;
    }

    public static P3D_Matrix operator *(P3D_Matrix lhs, P3D_Matrix rhs) {
        P3D_Matrix result = default;
        result.m00 = lhs.m00 * rhs.m00 + lhs.m01 * rhs.m10 + lhs.m02 * rhs.m20;
        result.m01 = lhs.m00 * rhs.m01 + lhs.m01 * rhs.m11 + lhs.m02 * rhs.m21;
        result.m02 = lhs.m00 * rhs.m02 + lhs.m01 * rhs.m12 + lhs.m02 * rhs.m22;
        result.m10 = lhs.m10 * rhs.m00 + lhs.m11 * rhs.m10 + lhs.m12 * rhs.m20;
        result.m11 = lhs.m10 * rhs.m01 + lhs.m11 * rhs.m11 + lhs.m12 * rhs.m21;
        result.m12 = lhs.m10 * rhs.m02 + lhs.m11 * rhs.m12 + lhs.m12 * rhs.m22;
        result.m20 = lhs.m20 * rhs.m00 + lhs.m21 * rhs.m10 + lhs.m22 * rhs.m20;
        result.m21 = lhs.m20 * rhs.m01 + lhs.m21 * rhs.m11 + lhs.m22 * rhs.m21;
        result.m22 = lhs.m20 * rhs.m02 + lhs.m21 * rhs.m12 + lhs.m22 * rhs.m22;
        return result;
    }
}