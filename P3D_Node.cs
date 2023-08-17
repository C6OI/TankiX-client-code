using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class P3D_Node {
    public Bounds Bound;

    public int PositiveIndex;

    public int NegativeIndex;

    public int TriangleIndex;

    public int TriangleCount;

    public void CalculateBound(List<P3D_Triangle> tris) {
        if (tris.Count <= 0) {
            return;
        }

        Vector3 vector = tris[0].Min;
        Vector3 vector2 = tris[0].Max;

        foreach (P3D_Triangle tri in tris) {
            vector = Vector3.Min(vector, tri.Min);
            vector2 = Vector3.Max(vector2, tri.Max);
        }

        Bound.SetMinMax(vector, vector2);
    }
}