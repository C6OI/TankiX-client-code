using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class P3D_Node {
    static List<P3D_Node> pool = new();

    public Bounds Bound;

    public bool Split;

    public int PositiveIndex;

    public int NegativeIndex;

    public int TriangleIndex;

    public int TriangleCount;

    public static P3D_Node Spawn() {
        if (pool.Count > 0) {
            int index = pool.Count - 1;
            P3D_Node result = pool[index];
            pool.RemoveAt(index);
            return result;
        }

        return new P3D_Node();
    }

    public static P3D_Node Despawn(P3D_Node node) {
        pool.Add(node);
        node.Bound = default;
        node.Split = false;
        node.PositiveIndex = 0;
        node.NegativeIndex = 0;
        node.TriangleIndex = 0;
        node.TriangleCount = 0;
        return null;
    }

    public void CalculateBound(List<P3D_Triangle> triangles) {
        if (triangles.Count > 0 && TriangleCount > 0) {
            Vector3 vector = triangles[TriangleIndex].Min;
            Vector3 vector2 = triangles[TriangleIndex].Max;

            for (int num = TriangleIndex + TriangleCount - 1; num > TriangleIndex; num--) {
                P3D_Triangle p3D_Triangle = triangles[num];
                vector = Vector3.Min(vector, p3D_Triangle.Min);
                vector2 = Vector3.Max(vector2, p3D_Triangle.Max);
            }

            Bound.SetMinMax(vector, vector2);
        }
    }
}