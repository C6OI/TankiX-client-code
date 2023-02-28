using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class P3D_Triangle {
    static List<P3D_Triangle> pool = new();

    public Vector3 PointA;

    public Vector3 PointB;

    public Vector3 PointC;

    public Vector2 Coord1A;

    public Vector2 Coord1B;

    public Vector2 Coord1C;

    public Vector2 Coord2A;

    public Vector2 Coord2B;

    public Vector2 Coord2C;

    public Vector3 Edge1 => PointB - PointA;

    public Vector3 Edge2 => PointC - PointA;

    public Vector3 Min => Vector3.Min(PointA, Vector3.Min(PointB, PointC));

    public Vector3 Max => Vector3.Max(PointA, Vector3.Max(PointB, PointC));

    public float MidX => (PointA.x + PointB.x + PointC.x) / 3f;

    public float MidY => (PointA.y + PointB.y + PointC.y) / 3f;

    public float MidZ => (PointA.z + PointB.z + PointC.z) / 3f;

    public static P3D_Triangle Spawn() {
        if (pool.Count > 0) {
            int index = pool.Count - 1;
            P3D_Triangle result = pool[index];
            pool.RemoveAt(index);
            return result;
        }

        return new P3D_Triangle();
    }

    public static P3D_Triangle Despawn(P3D_Triangle triangle) {
        pool.Add(triangle);
        return null;
    }
}