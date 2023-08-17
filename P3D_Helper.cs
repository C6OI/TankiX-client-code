using System.Collections.Generic;
using UnityEngine;

public static class P3D_Helper {
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

    public static Mesh GetMesh(GameObject gameObject) {
        if (gameObject != null) {
            MeshFilter component = gameObject.GetComponent<MeshFilter>();

            if (component != null) {
                return component.sharedMesh;
            }
        }

        return null;
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

    public static void Destroy(Object o) => Object.Destroy(o);

    public static bool IntersectBarycentric(Vector3 start, Vector3 end, P3D_Triangle triangle, out Vector3 weights,
        out float distance01) {
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
        Vector3 lhs2 = start - triangle.PositionA;
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
            case TextureFormat.Alpha8:
                return true;

            case TextureFormat.ARGB32:
                return true;

            case TextureFormat.BGRA32:
                return true;

            case TextureFormat.RGB24:
                return true;

            default:
                return false;
        }
    }
}