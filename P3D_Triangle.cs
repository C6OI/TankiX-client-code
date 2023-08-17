using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class P3D_Triangle {
    static readonly List<P3D_Triangle> pool = new();

    public Vector3 PositionA;

    public Vector3 PositionB;

    public Vector3 PositionC;

    public Vector2 Coord1A;

    public Vector2 Coord1B;

    public Vector2 Coord1C;

    public Vector2 Coord2A;

    public Vector2 Coord2B;

    public Vector2 Coord2C;

    public Vector3 Edge1 => PositionB - PositionA;

    public Vector3 Edge2 => PositionC - PositionA;

    public Vector3 Min => Vector3.Min(PositionA, Vector3.Min(PositionB, PositionC));

    public Vector3 Max => Vector3.Max(PositionA, Vector3.Max(PositionB, PositionC));

    public float MidX => (PositionA.x + PositionB.x + PositionC.x) / 3f;

    public float MidY => (PositionA.y + PositionB.y + PositionC.y) / 3f;

    public float MidZ => (PositionA.z + PositionB.z + PositionC.z) / 3f;

    public static P3D_Triangle Create() {
        if (pool.Count > 0) {
            int index = pool.Count - 1;
            P3D_Triangle result = pool[index];
            pool.RemoveAt(index);
            return result;
        }

        return new P3D_Triangle();
    }

    public static P3D_Triangle Destroy(P3D_Triangle triangle) {
        pool.Add(triangle);
        return null;
    }
}