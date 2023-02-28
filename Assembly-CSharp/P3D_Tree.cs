using System;
using System.Collections.Generic;
using UnityEngine;

public class P3D_Tree {
    static readonly List<P3D_Triangle> potentials = new();

    static readonly List<P3D_Result> results = new();

    static P3D_Tree tempInstance;

    [SerializeField] Mesh mesh;

    [SerializeField] readonly List<P3D_Node> nodes = new();

    [SerializeField] int subMeshIndex;

    [SerializeField] readonly List<P3D_Triangle> triangles = new();

    [SerializeField] int vertexCount;

    public static P3D_Tree TempInstance {
        get {
            if (tempInstance == null) {
                tempInstance = new P3D_Tree();
            }

            return tempInstance;
        }
    }

    public bool IsReady => nodes.Count > 0;

    public void Clear() {
        mesh = null;
        vertexCount = 0;
        subMeshIndex = 0;

        for (int num = triangles.Count - 1; num >= 0; num--) {
            P3D_Triangle.Despawn(triangles[num]);
        }

        triangles.Clear();

        for (int num2 = nodes.Count - 1; num2 >= 0; num2--) {
            P3D_Node.Despawn(nodes[num2]);
        }

        nodes.Clear();
    }

    public void ClearResults() {
        for (int num = results.Count - 1; num >= 0; num--) {
            P3D_Result.Despawn(results[num]);
        }

        results.Clear();
        potentials.Clear();
    }

    public void SetMesh(Mesh newMesh, int newSubMeshIndex = 0, bool forceUpdate = false) {
        if (newMesh != null) {
            if (forceUpdate || !(newMesh == mesh) || newSubMeshIndex != subMeshIndex || newMesh.vertexCount != vertexCount) {
                Clear();
                mesh = newMesh;
                subMeshIndex = newSubMeshIndex;
                vertexCount = newMesh.vertexCount;
                ExtractTriangles();
                ConstructNodes();
            }
        } else {
            Clear();
        }
    }

    public void SetMesh(GameObject gameObject, int subMeshIndex = 0, bool forceUpdate = false) {
        Mesh bakedMesh = null;
        Mesh newMesh = P3D_Helper.GetMesh(gameObject, ref bakedMesh);

        if (bakedMesh != null) {
            P3D_Helper.Destroy(bakedMesh);
            throw new Exception("P3D_Tree cannot manage baked meshes, call SetMesh with the Mesh directly to use animated meshes");
        }

        SetMesh(newMesh, subMeshIndex, forceUpdate);
    }

    public P3D_Result FindNearest(Vector3 point, float maxDistance) {
        ClearResults();

        if (IsReady && maxDistance > 0f) {
            float num = maxDistance * maxDistance;
            P3D_Triangle p3D_Triangle = null;
            Vector3 weights = default;
            BeginSearchDistance(point, num);

            for (int num2 = potentials.Count - 1; num2 >= 0; num2--) {
                P3D_Triangle p3D_Triangle2 = potentials[num2];
                Vector3 weights2 = default;
                float num3 = P3D_Helper.ClosestBarycentric(point, p3D_Triangle2, out weights2);

                if (num3 <= num) {
                    num = num3;
                    p3D_Triangle = p3D_Triangle2;
                    weights = weights2;
                }
            }

            if (p3D_Triangle != null) {
                return GetResult(p3D_Triangle, weights, Mathf.Sqrt(num) / maxDistance);
            }
        }

        return null;
    }

    public P3D_Result FindBetweenNearest(Vector3 startPoint, Vector3 endPoint) {
        ClearResults();

        if (IsReady) {
            float num = float.PositiveInfinity;
            P3D_Triangle p3D_Triangle = null;
            Vector3 weights = default;
            BeginSearchBetween(startPoint, endPoint);

            for (int num2 = potentials.Count - 1; num2 >= 0; num2--) {
                P3D_Triangle p3D_Triangle2 = potentials[num2];
                Vector3 weights2 = default;
                float distance = 0f;

                if (P3D_Helper.IntersectBarycentric(startPoint, endPoint, p3D_Triangle2, out weights2, out distance) && distance < num) {
                    num = distance;
                    p3D_Triangle = p3D_Triangle2;
                    weights = weights2;
                }
            }

            if (p3D_Triangle != null) {
                return GetResult(p3D_Triangle, weights, num);
            }
        }

        return null;
    }

    public List<P3D_Result> FindBetweenAll(Vector3 startPoint, Vector3 endPoint) {
        ClearResults();

        if (IsReady) {
            BeginSearchBetween(startPoint, endPoint);

            for (int num = potentials.Count - 1; num >= 0; num--) {
                P3D_Triangle triangle = potentials[num];
                Vector3 weights = default;
                float distance = 0f;

                if (P3D_Helper.IntersectBarycentric(startPoint, endPoint, triangle, out weights, out distance)) {
                    AddToResults(triangle, weights, distance);
                }
            }
        }

        return results;
    }

    public P3D_Result FindPerpendicularNearest(Vector3 point, float maxDistance) {
        ClearResults();

        if (IsReady && maxDistance > 0f) {
            float num = maxDistance * maxDistance;
            P3D_Triangle p3D_Triangle = null;
            Vector3 weights = default;
            BeginSearchDistance(point, num);

            for (int num2 = potentials.Count - 1; num2 >= 0; num2--) {
                P3D_Triangle p3D_Triangle2 = potentials[num2];
                Vector3 weights2 = default;
                float distanceSqr = 0f;

                if (P3D_Helper.ClosestBarycentric(point, p3D_Triangle2, ref weights2, ref distanceSqr) && distanceSqr <= num) {
                    num = distanceSqr;
                    p3D_Triangle = p3D_Triangle2;
                    weights = weights2;
                }
            }

            if (p3D_Triangle != null) {
                return GetResult(p3D_Triangle, weights, Mathf.Sqrt(num) / maxDistance);
            }
        }

        return null;
    }

    public List<P3D_Result> FindPerpendicularAll(Vector3 point, float maxDistance) {
        ClearResults();

        if (IsReady && maxDistance > 0f) {
            float num = maxDistance * maxDistance;
            BeginSearchDistance(point, num);

            for (int num2 = potentials.Count - 1; num2 >= 0; num2--) {
                P3D_Triangle triangle = potentials[num2];
                Vector3 weights = default;
                float distanceSqr = 0f;

                if (P3D_Helper.ClosestBarycentric(point, triangle, ref weights, ref distanceSqr) && distanceSqr <= num) {
                    AddToResults(triangle, weights, Mathf.Sqrt(distanceSqr) / maxDistance);
                }
            }
        }

        return results;
    }

    void BeginSearchDistance(Vector3 point, float maxDistanceSqr) {
        SearchDistance(nodes[0], point, maxDistanceSqr);
    }

    void SearchDistance(P3D_Node node, Vector3 point, float maxDistanceSqr) {
        if (!(node.Bound.SqrDistance(point) < maxDistanceSqr)) {
            return;
        }

        if (node.Split) {
            if (node.PositiveIndex != 0) {
                SearchDistance(nodes[node.PositiveIndex], point, maxDistanceSqr);
            }

            if (node.NegativeIndex != 0) {
                SearchDistance(nodes[node.NegativeIndex], point, maxDistanceSqr);
            }
        } else {
            AddToPotentials(node);
        }
    }

    void BeginSearchBetween(Vector3 startPoint, Vector3 endPoint) {
        Vector3 direction = endPoint - startPoint;
        Ray ray = new(startPoint, direction);
        float magnitude = direction.magnitude;
        SearchBetween(nodes[0], ray, magnitude);
    }

    void SearchBetween(P3D_Node node, Ray ray, float maxDistance) {
        float distance = 0f;

        if (!node.Bound.IntersectRay(ray, out distance) || !(distance <= maxDistance)) {
            return;
        }

        if (node.Split) {
            if (node.PositiveIndex != 0) {
                SearchBetween(nodes[node.PositiveIndex], ray, maxDistance);
            }

            if (node.NegativeIndex != 0) {
                SearchBetween(nodes[node.NegativeIndex], ray, maxDistance);
            }
        } else {
            AddToPotentials(node);
        }
    }

    void AddToPotentials(P3D_Node node) {
        for (int i = node.TriangleIndex; i < node.TriangleIndex + node.TriangleCount; i++) {
            potentials.Add(triangles[i]);
        }
    }

    void AddToResults(P3D_Triangle triangle, Vector3 weights, float distance01) {
        P3D_Result p3D_Result = P3D_Result.Spawn();
        p3D_Result.Triangle = triangle;
        p3D_Result.Weights = weights;
        p3D_Result.Distance01 = distance01;
        results.Add(p3D_Result);
    }

    P3D_Result GetResult(P3D_Triangle triangle, Vector3 weights, float distance01) {
        ClearResults();
        AddToResults(triangle, weights, distance01);
        return results[0];
    }

    void ExtractTriangles() {
        if (subMeshIndex < 0 || mesh.subMeshCount < 0) {
            return;
        }

        int submesh = Mathf.Min(subMeshIndex, mesh.subMeshCount - 1);
        int[] array = mesh.GetTriangles(submesh);
        Vector3[] vertices = mesh.vertices;
        Vector2[] uv = mesh.uv;
        Vector2[] uv2 = mesh.uv2;

        if (array.Length <= 0) {
            return;
        }

        int num = array.Length / 3;

        for (int num2 = num - 1; num2 >= 0; num2--) {
            P3D_Triangle p3D_Triangle = P3D_Triangle.Spawn();
            int num3 = array[num2 * 3];
            int num4 = array[num2 * 3 + 1];
            int num5 = array[num2 * 3 + 2];
            p3D_Triangle.PointA = vertices[num3];
            p3D_Triangle.PointB = vertices[num4];
            p3D_Triangle.PointC = vertices[num5];
            p3D_Triangle.Coord1A = uv[num3];
            p3D_Triangle.Coord1B = uv[num4];
            p3D_Triangle.Coord1C = uv[num5];

            if (uv2.Length > 0) {
                p3D_Triangle.Coord2A = uv2[num3];
                p3D_Triangle.Coord2B = uv2[num4];
                p3D_Triangle.Coord2C = uv2[num5];
            }

            triangles.Add(p3D_Triangle);
        }
    }

    void ConstructNodes() {
        P3D_Node p3D_Node = P3D_Node.Spawn();
        nodes.Add(p3D_Node);
        Pack(p3D_Node, 0, triangles.Count);
    }

    void Pack(P3D_Node node, int min, int max) {
        int num = max - min;
        node.TriangleIndex = min;
        node.TriangleCount = num;
        node.Split = num >= 5;
        node.CalculateBound(triangles);

        if (node.Split) {
            int num2 = (min + max) / 2;
            SortTriangles(min, max);
            node.PositiveIndex = nodes.Count;
            P3D_Node p3D_Node = P3D_Node.Spawn();
            nodes.Add(p3D_Node);
            Pack(p3D_Node, min, num2);
            node.NegativeIndex = nodes.Count;
            P3D_Node p3D_Node2 = P3D_Node.Spawn();
            nodes.Add(p3D_Node2);
            Pack(p3D_Node2, num2, max);
        }
    }

    void SortTriangles(int minIndex, int maxIndex) {
        potentials.Clear();
        Vector3 vector = triangles[minIndex].Min;
        Vector3 vector2 = triangles[minIndex].Max;
        Vector3 zero = Vector3.zero;

        for (int i = minIndex; i < maxIndex; i++) {
            P3D_Triangle p3D_Triangle = triangles[i];
            vector = Vector3.Min(vector, p3D_Triangle.Min);
            vector2 = Vector3.Max(vector2, p3D_Triangle.Max);
            zero += p3D_Triangle.PointA + p3D_Triangle.PointB + p3D_Triangle.PointC;
            potentials.Add(p3D_Triangle);
        }

        Vector3 vector3 = vector2 - vector;

        if (vector3.x > vector3.y && vector3.x > vector3.z) {
            float num = P3D_Helper.Divide(zero.x, triangles.Count * 3f);

            for (int num2 = potentials.Count - 1; num2 >= 0; num2--) {
                P3D_Triangle p3D_Triangle2 = potentials[num2];
                SortTriangle(p3D_Triangle2, ref minIndex, ref maxIndex, p3D_Triangle2.MidX >= num);
            }
        } else if (vector3.y > vector3.x && vector3.y > vector3.z) {
            float num3 = P3D_Helper.Divide(zero.y, triangles.Count * 3f);

            for (int num4 = potentials.Count - 1; num4 >= 0; num4--) {
                P3D_Triangle p3D_Triangle3 = potentials[num4];
                SortTriangle(p3D_Triangle3, ref minIndex, ref maxIndex, p3D_Triangle3.MidY >= num3);
            }
        } else {
            float num5 = P3D_Helper.Divide(zero.z, triangles.Count * 3f);

            for (int num6 = potentials.Count - 1; num6 >= 0; num6--) {
                P3D_Triangle p3D_Triangle4 = potentials[num6];
                SortTriangle(p3D_Triangle4, ref minIndex, ref maxIndex, p3D_Triangle4.MidZ >= num5);
            }
        }
    }

    void SortTriangle(P3D_Triangle triangle, ref int minIndex, ref int maxIndex, bool abovePivot) {
        if (abovePivot) {
            triangles[maxIndex - 1] = triangle;
            maxIndex--;
        } else {
            triangles[minIndex] = triangle;
            minIndex++;
        }
    }
}