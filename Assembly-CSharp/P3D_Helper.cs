using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public static class P3D_Helper {
    public const string ComponentMenuPrefix = "Paint in 3D/P3D ";

    static Material clearMaterial;

    public static Material ClearMaterial {
        get {
            if (clearMaterial == null) {
                clearMaterial = new Material(Shader.Find("Transparent/Diffuse"));
                clearMaterial.color = Color.clear;
            }

            return clearMaterial;
        }
    }

    public static TextureFormat GetTextureFormat(P3D_Format format) {
        switch (format) {
            case P3D_Format.TruecolorRGBA:
                return TextureFormat.RGBA32;

            case P3D_Format.TruecolorRGB:
                return TextureFormat.RGB24;

            case P3D_Format.TruecolorA:
                return TextureFormat.Alpha8;

            default:
                return 0;
        }
    }

    public static bool IndexInMask(int index, LayerMask mask) {
        mask = mask & 1 << index;
        return mask != 0;
    }

    public static Texture2D CreateTexture(int width, int height, TextureFormat format, bool mipMaps) {
        if (width > 0 && height > 0) {
            return new Texture2D(width, height, format, mipMaps);
        }

        return null;
    }

    public static void ClearTexture(Texture2D texture2D, Color color, bool apply = true) {
        if (!(texture2D != null)) {
            return;
        }

        for (int num = texture2D.height - 1; num >= 0; num--) {
            for (int num2 = texture2D.width - 1; num2 >= 0; num2--) {
                texture2D.SetPixel(num2, num, color);
            }
        }

        if (apply) {
            texture2D.Apply();
        }
    }

    public static Mesh GetMesh(GameObject gameObject, ref Mesh bakedMesh) {
        Mesh mesh = null;

        if (gameObject != null) {
            MeshFilter component = gameObject.GetComponent<MeshFilter>();

            if (component != null) {
                mesh = component.sharedMesh;
            } else {
                SkinnedMeshRenderer component2 = gameObject.GetComponent<SkinnedMeshRenderer>();

                if (component2 != null) {
                    mesh = component2.sharedMesh;

                    if (mesh != null) {
                        if (bakedMesh == null) {
                            bakedMesh = new Mesh();
                            bakedMesh.name = "Baked Mesh";
                        }

                        component2.BakeMesh(bakedMesh);
                        return bakedMesh;
                    }
                }
            }
        }

        DestroyMesh(ref bakedMesh);
        return mesh;
    }

    static void DestroyMesh(ref Mesh mesh) {
        if (mesh != null) {
            Destroy(mesh);
            mesh = null;
        }
    }

    public static Material GetMaterial(GameObject gameObject, int materialIndex = 0) {
        if (gameObject != null && materialIndex >= 0) {
            Renderer component = gameObject.GetComponent<Renderer>();

            if (component != null) {
                Material[] sharedMaterials = component.sharedMaterials;

                if (materialIndex < sharedMaterials.Length) {
                    return sharedMaterials[materialIndex];
                }
            }
        }

        return null;
    }

    public static Material CloneMaterial(GameObject gameObject, int materialIndex = 0) {
        if (gameObject != null && materialIndex >= 0) {
            Renderer component = gameObject.GetComponent<Renderer>();

            if (component != null) {
                Material[] sharedMaterials = component.sharedMaterials;

                if (materialIndex < sharedMaterials.Length) {
                    Material o = sharedMaterials[materialIndex];
                    o = sharedMaterials[materialIndex] = Clone(o);
                    component.sharedMaterials = sharedMaterials;
                    return o;
                }
            }
        }

        return null;
    }

    public static Material AddMaterial(Renderer renderer, Shader shader, int materialIndex = -1) {
        if (renderer != null) {
            List<Material> list = new(renderer.sharedMaterials);
            Material material = new(shader);

            if (materialIndex <= 0) {
                materialIndex = list.Count;
            }

            list.Insert(materialIndex, material);
            renderer.sharedMaterials = list.ToArray();
            return material;
        }

        return null;
    }

    public static Rect SplitHorizontal(ref Rect rect, int separation) {
        Rect rect2 = rect;
        rect2.xMax -= rect.width / 2f + separation;
        Rect result = rect;
        result.xMin += rect.width / 2f + separation;
        rect = rect2;
        return result;
    }

    public static Rect SplitVertical(ref Rect rect, int separation) {
        Rect rect2 = rect;
        rect2.yMax -= rect.height / 2f + separation;
        Rect result = rect;
        result.yMin += rect.height / 2f + separation;
        rect = rect2;
        return result;
    }

    public static bool Zero(float v) => v == 0f;

    public static float Divide(float a, float b) => Zero(b) ? 0f : a / b;

    public static float Reciprocal(float a) => Zero(a) ? 0f : 1f / a;

    public static float GetUniformScale(Transform transform) {
        Vector3 lossyScale = transform.lossyScale;
        return (lossyScale.x + lossyScale.y + lossyScale.z) / 3f;
    }

    public static Vector2 GetUV(RaycastHit hit, P3D_CoordType coord) {
        switch (coord) {
            case P3D_CoordType.UV1:
                return hit.textureCoord;

            case P3D_CoordType.UV2:
                return hit.textureCoord2;

            default:
                return default;
        }
    }

    public static float DampenFactor(float dampening, float elapsed) => 1f - Mathf.Pow((float)Math.E, (0f - dampening) * elapsed);

    public static Vector2 CalculatePixelFromCoord(Vector2 uv, Vector2 tiling, Vector2 offset, int width, int height) {
        uv.x = Mathf.Repeat(uv.x * tiling.x + offset.x, 1f);
        uv.y = Mathf.Repeat(uv.y * tiling.y + offset.y, 1f);
        uv.x = Mathf.Clamp(Mathf.RoundToInt(uv.x * width), 0, width - 1);
        uv.y = Mathf.Clamp(Mathf.RoundToInt(uv.y * height), 0, height - 1);
        return uv;
    }

    public static P3D_Matrix CreateMatrix(Vector2 position, Vector2 size, float angle) {
        P3D_Matrix p3D_Matrix = P3D_Matrix.Translation(position.x, position.y);
        P3D_Matrix p3D_Matrix2 = P3D_Matrix.Rotation(angle);
        P3D_Matrix p3D_Matrix3 = P3D_Matrix.Translation(size.x * -0.5f, size.y * -0.5f);
        P3D_Matrix p3D_Matrix4 = P3D_Matrix.Scaling(size.x, size.y);
        return p3D_Matrix * p3D_Matrix2 * p3D_Matrix3 * p3D_Matrix4;
    }

    public static float Dampen(float current, float target, float dampening, float elapsed, float minStep = 0f) {
        float num = DampenFactor(dampening, elapsed);
        float maxDelta = Mathf.Abs(target - current) * num + minStep * elapsed;
        return Mathf.MoveTowards(current, target, maxDelta);
    }

    public static Vector3 Dampen3(Vector3 current, Vector3 target, float dampening, float elapsed, float minStep = 0f) {
        float num = DampenFactor(dampening, elapsed);
        float maxDistanceDelta = (target - current).magnitude * num + minStep * elapsed;
        return Vector3.MoveTowards(current, target, maxDistanceDelta);
    }

    public static T Destroy<T>(T o) where T : Object {
        Object.Destroy(o);
        return null;
    }

    public static bool IntersectBarycentric(Vector3 start, Vector3 end, P3D_Triangle triangle, out Vector3 weights, out float distance01) {
        weights = default;
        distance01 = 0f;
        Vector3 edge = triangle.Edge1;
        Vector3 edge2 = triangle.Edge2;
        Vector3 lhs = end - start;
        Vector3 rhs = Vector3.Cross(lhs, edge2);
        float num = Vector3.Dot(edge, rhs);

        if (Mathf.Abs(num) < float.Epsilon) {
            return false;
        }

        float num2 = 1f / num;
        Vector3 lhs2 = start - triangle.PointA;
        weights.x = Vector3.Dot(lhs2, rhs) * num2;

        if (weights.x < -1E-45f || weights.x > 1f) {
            return false;
        }

        Vector3 rhs2 = Vector3.Cross(lhs2, edge);
        weights.y = Vector3.Dot(lhs, rhs2) * num2;
        float num3 = weights.x + weights.y;

        if (weights.y < -1E-45f || num3 > 1f) {
            return false;
        }

        weights = new Vector3(1f - num3, weights.x, weights.y);
        distance01 = Vector3.Dot(edge2, rhs2) * num2;
        return distance01 >= 0f && distance01 <= 1f;
    }

    public static float ClosestBarycentric(Vector3 point, P3D_Triangle triangle, out Vector3 weights) {
        Vector3 pointA = triangle.PointA;
        Vector3 pointB = triangle.PointB;
        Vector3 pointC = triangle.PointC;
        Quaternion quaternion = Quaternion.Inverse(Quaternion.LookRotation(-Vector3.Cross(pointA - pointB, pointA - pointC)));
        Vector3 vector = quaternion * pointA;
        Vector3 vector2 = quaternion * pointB;
        Vector3 vector3 = quaternion * pointC;
        Vector3 vector4 = quaternion * point;

        if (PointLeftOfLine(vector, vector2, vector4)) {
            float num = ClosestBarycentric(vector4, vector, vector2);
            weights = new Vector3(1f - num, num, 0f);
        } else if (PointLeftOfLine(vector2, vector3, vector4)) {
            float num2 = ClosestBarycentric(vector4, vector2, vector3);
            weights = new Vector3(0f, 1f - num2, num2);
        } else if (PointLeftOfLine(vector3, vector, vector4)) {
            float num3 = ClosestBarycentric(vector4, vector3, vector);
            weights = new Vector3(num3, 0f, 1f - num3);
        } else {
            Vector3 vector5 = vector2 - vector;
            Vector3 vector6 = vector3 - vector;
            Vector3 vector7 = vector4 - vector;
            float num4 = Vector2.Dot(vector5, vector5);
            float num5 = Vector2.Dot(vector5, vector6);
            float num6 = Vector2.Dot(vector6, vector6);
            float num7 = Vector2.Dot(vector7, vector5);
            float num8 = Vector2.Dot(vector7, vector6);
            float num9 = Reciprocal(num4 * num6 - num5 * num5);
            weights.y = (num6 * num7 - num5 * num8) * num9;
            weights.z = (num4 * num8 - num5 * num7) * num9;
            weights.x = 1f - weights.y - weights.z;
        }

        Vector3 vector8 = weights.x * pointA + weights.y * pointB + weights.z * pointC;
        return (point - vector8).sqrMagnitude;
    }

    public static bool ClosestBarycentric(Vector3 point, P3D_Triangle triangle, ref Vector3 weights, ref float distanceSqr) {
        Vector3 pointA = triangle.PointA;
        Vector3 pointB = triangle.PointB;
        Vector3 pointC = triangle.PointC;
        Quaternion quaternion = Quaternion.Inverse(Quaternion.LookRotation(-Vector3.Cross(pointA - pointB, pointA - pointC)));
        Vector3 vector = quaternion * pointA;
        Vector3 vector2 = quaternion * pointB;
        Vector3 vector3 = quaternion * pointC;
        Vector3 vector4 = quaternion * point;

        if (PointRightOfLine(vector, vector2, vector4) && PointRightOfLine(vector2, vector3, vector4) && PointRightOfLine(vector3, vector, vector4)) {
            Vector3 vector5 = vector2 - vector;
            Vector3 vector6 = vector3 - vector;
            Vector3 vector7 = vector4 - vector;
            float num = Vector2.Dot(vector5, vector5);
            float num2 = Vector2.Dot(vector5, vector6);
            float num3 = Vector2.Dot(vector6, vector6);
            float num4 = Vector2.Dot(vector7, vector5);
            float num5 = Vector2.Dot(vector7, vector6);
            float num6 = Reciprocal(num * num3 - num2 * num2);
            weights.y = (num3 * num4 - num2 * num5) * num6;
            weights.z = (num * num5 - num2 * num4) * num6;
            weights.x = 1f - weights.y - weights.z;
            Vector3 vector8 = weights.x * pointA + weights.y * pointB + weights.z * pointC;
            distanceSqr = (point - vector8).sqrMagnitude;
            return true;
        }

        return false;
    }

    public static float ClosestBarycentric(Vector2 point, Vector2 start, Vector2 end) {
        Vector2 vector = end - start;
        float sqrMagnitude = vector.sqrMagnitude;

        if (sqrMagnitude > 0f) {
            return Mathf.Clamp01(Vector2.Dot(point - start, vector / sqrMagnitude));
        }

        return 0.5f;
    }

    public static bool PointLeftOfLine(Vector2 a, Vector2 b, Vector2 p) => (b.x - a.x) * (p.y - a.y) - (p.x - a.x) * (b.y - a.y) >= 0f;

    public static bool PointRightOfLine(Vector2 a, Vector2 b, Vector2 p) => (b.x - a.x) * (p.y - a.y) - (p.x - a.x) * (b.y - a.y) <= 0f;

    public static T Clone<T>(T o, bool keepName = true) where T : Object {
        if (o != null) {
            T val = Object.Instantiate(o);

            if (val != null && keepName) {
                val.name = o.name;
            }

            return val;
        }

        return null;
    }

    public static bool IsWritableFormat(TextureFormat format) {
        switch (format) {
            case TextureFormat.RGBA32:
                return true;

            case TextureFormat.ARGB32:
                return true;

            case TextureFormat.BGRA32:
                return true;

            case TextureFormat.RGB24:
                return true;

            case TextureFormat.Alpha8:
                return true;

            default:
                return false;
        }
    }
}