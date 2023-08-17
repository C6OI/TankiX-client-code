using System.Collections.Generic;
using UnityEngine;

public class P3D_Painter {
    public delegate void PaintOperation(Texture2D texture, int x, int y);

    static readonly List<P3D_Triangle> closestTriangles = new();

    [SerializeField] readonly List<P3D_Node> nodes = new();

    [SerializeField] readonly List<P3D_Triangle> triangles = new();

    [SerializeField] int materialIndex;

    [SerializeField] Mesh mesh;

    [SerializeField] Vector2 offset;

    PaintOperation paintOperation;

    [SerializeField] bool ready;

    [SerializeField] Texture2D texture;

    [SerializeField] Vector2 tiling = Vector2.one;

    [SerializeField] int vertexCount;

    public void SetMesh(Mesh newMesh, int newMaterialIndex = 0, bool forceUpdate = false) {
        if (newMesh != null) {
            if (forceUpdate || newMesh != mesh || newMesh.vertexCount != vertexCount || newMaterialIndex != materialIndex) {
                Clear();
            }

            if (!ready) {
                ready = true;
                mesh = newMesh;
                materialIndex = newMaterialIndex;
                vertexCount = newMesh.vertexCount;
                P3D_Node p3D_Node = new();
                nodes.Add(p3D_Node);
                List<P3D_Triangle> tris = GetTriangles();
                Pack(p3D_Node, tris);
            }
        } else {
            Clear();
        }
    }

    public void Clear() {
        ready = false;
        mesh = null;
        vertexCount = 0;

        for (int num = triangles.Count - 1; num >= 0; num--) {
            P3D_Triangle.Destroy(triangles[num]);
        }

        triangles.Clear();
        nodes.Clear();
    }

    public void BeginPaint(P3D_Brush brush, float opacity, Texture2D newTexture, Vector2 newTiling, Vector2 newOffset) {
        PaintOperation newPaintOperation = null;

        switch (brush.Blend) {
            case P3D_BlendMode.AlphaBlend:
                newPaintOperation = P3D_PaintOperation.AlphaBlend(brush.Color,
                    opacity,
                    brush.Size,
                    brush.Size,
                    brush.Shape,
                    brush.Pattern,
                    brush.PatternScale,
                    brush.Texture,
                    brush.TextureScale);

                break;

            case P3D_BlendMode.AlphaBlendRgb:
                newPaintOperation = P3D_PaintOperation.AlphaBlendRgb(brush.Color,
                    opacity,
                    brush.Size,
                    brush.Size,
                    brush.Shape,
                    brush.Pattern,
                    brush.PatternScale,
                    brush.Texture,
                    brush.TextureScale);

                break;

            case P3D_BlendMode.AlphaErase:
                newPaintOperation = P3D_PaintOperation.AlphaErase(opacity,
                    brush.Size,
                    brush.Size,
                    brush.Shape,
                    brush.Pattern,
                    brush.PatternScale);

                break;

            case P3D_BlendMode.AdditiveBlend:
                newPaintOperation = P3D_PaintOperation.AdditiveBlend(brush.Color,
                    opacity,
                    brush.Size,
                    brush.Size,
                    brush.Shape,
                    brush.Pattern,
                    brush.PatternScale,
                    brush.Texture,
                    brush.TextureScale);

                break;

            case P3D_BlendMode.SubtractiveBlend:
                newPaintOperation = P3D_PaintOperation.SubtractiveBlend(brush.Color,
                    opacity,
                    brush.Size,
                    brush.Size,
                    brush.Shape,
                    brush.Pattern,
                    brush.PatternScale,
                    brush.Texture,
                    brush.TextureScale);

                break;

            case P3D_BlendMode.NormalBlend:
                newPaintOperation = P3D_PaintOperation.NormalBlend(brush.Direction,
                    opacity,
                    brush.Size,
                    brush.Size,
                    brush.Shape,
                    brush.Pattern,
                    brush.PatternScale,
                    brush.NormalTexture,
                    brush.NormalTextureScale);

                break;
        }

        BeginPaint(newPaintOperation, newTexture, newTiling, newOffset);
    }

    public void BeginPaint(PaintOperation newPaintOperation, Texture2D newTexture, Vector2 newTiling, Vector2 newOffset) {
        if (newTexture != null && tiling.x != 0f && tiling.y != 0f && P3D_Helper.IsWritableFormat(newTexture.format)) {
            paintOperation = newPaintOperation;
            texture = newTexture;
            tiling = newTiling;
            offset = newOffset;
        } else {
            texture = null;
            paintOperation = null;
        }
    }

    public void EndPaint() {
        if (texture != null) {
            texture.Apply();
        }
    }

    public void Paint(int x, int y) {
        if (texture != null && paintOperation != null) {
            paintOperation(texture, x, y);
        }
    }

    public void Paint(Transform transform, Vector2 screenPosition, float distance, Camera camera = null,
        P3D_CoordType coord = P3D_CoordType.UV1, bool passThrough = false) {
        if (camera == null) {
            camera = Camera.main;
        }

        if (camera != null) {
            Vector3 position = camera.transform.position;
            Vector3 worldEnd = camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, distance));
            Paint(transform, position, worldEnd, coord, passThrough);
        }
    }

    public void Paint(Transform transform, Vector3 screenPosition, Camera camera = null,
        P3D_CoordType coord = P3D_CoordType.UV1, bool passThrough = false) {
        if (camera == null) {
            camera = Camera.main;
        }

        if (camera != null) {
            Vector3 position = camera.transform.position;
            Vector3 worldEnd = camera.ScreenToWorldPoint(screenPosition);
            Paint(transform, position, worldEnd, coord, passThrough);
        }
    }

    public void Paint(Transform transform, Vector3 worldStart, Vector3 worldEnd, P3D_CoordType coord = P3D_CoordType.UV1,
        bool passThrough = false) {
        if (transform != null) {
            Vector3 localStart = transform.InverseTransformPoint(worldStart);
            Vector3 localEnd = transform.InverseTransformPoint(worldEnd);
            Paint(localStart, localEnd, coord, passThrough);
        }
    }

    public void Paint(Vector3 localStart, Vector3 localEnd, P3D_CoordType coord = P3D_CoordType.UV1,
        bool passThrough = false) {
        if (passThrough) {
            PaintAll(localStart, localEnd, coord);
        } else {
            PaintFirst(localStart, localEnd, coord);
        }
    }

    public void PaintFirst(Vector3 localStart, Vector3 localEnd, P3D_CoordType coord = P3D_CoordType.UV1) {
        if (!(texture != null) || paintOperation == null || nodes.Count <= 0) {
            return;
        }

        Vector3 weights = default;
        float distance = 0f;
        Vector2 vector = default;
        float num = float.PositiveInfinity;
        BeginSearch(nodes[0], localStart, localEnd);

        for (int num2 = closestTriangles.Count - 1; num2 >= 0; num2--) {
            P3D_Triangle p3D_Triangle = closestTriangles[num2];

            if (P3D_Helper.IntersectBarycentric(localStart, localEnd, p3D_Triangle, out weights, out distance) &&
                distance < num) {
                switch (coord) {
                    case P3D_CoordType.UV1:
                        vector = p3D_Triangle.Coord1A * weights.x +
                                 p3D_Triangle.Coord1B * weights.y +
                                 p3D_Triangle.Coord1C * weights.z;

                        break;

                    case P3D_CoordType.UV2:
                        vector = p3D_Triangle.Coord2A * weights.x +
                                 p3D_Triangle.Coord2B * weights.y +
                                 p3D_Triangle.Coord2C * weights.z;

                        break;
                }

                num = distance;
            }
        }

        if (num < float.PositiveInfinity) {
            vector.x = vector.x * tiling.x + offset.x;
            vector.y = vector.y * tiling.y + offset.y;
            float num3 = vector.x % 1f * texture.width;
            float num4 = vector.y % 1f * texture.height;
            Paint((int)num3, (int)num4);
        }
    }

    public void PaintAll(Vector3 localStart, Vector3 localEnd, P3D_CoordType coord = P3D_CoordType.UV1) {
        if (!(texture != null) || paintOperation == null || nodes.Count <= 0) {
            return;
        }

        Vector3 weights = default;
        float distance = 0f;
        BeginSearch(nodes[0], localStart, localEnd);

        for (int num = closestTriangles.Count - 1; num >= 0; num--) {
            P3D_Triangle p3D_Triangle = closestTriangles[num];

            if (P3D_Helper.IntersectBarycentric(localStart, localEnd, p3D_Triangle, out weights, out distance)) {
                Vector2 vector = default;

                switch (coord) {
                    case P3D_CoordType.UV1:
                        vector = p3D_Triangle.Coord1A * weights.x +
                                 p3D_Triangle.Coord1B * weights.y +
                                 p3D_Triangle.Coord1C * weights.z;

                        break;

                    case P3D_CoordType.UV2:
                        vector = p3D_Triangle.Coord2A * weights.x +
                                 p3D_Triangle.Coord2B * weights.y +
                                 p3D_Triangle.Coord2C * weights.z;

                        break;
                }

                float num2 = vector.x % 1f * texture.width;
                float num3 = vector.y % 1f * texture.height;
                Paint((int)num2, (int)num3);
            }
        }
    }

    void BeginSearch(P3D_Node node, Vector3 localStart, Vector3 localEnd) {
        closestTriangles.Clear();
        Vector3 direction = localEnd - localStart;
        Ray ray = new(localStart, direction);
        float magnitude = direction.magnitude;
        Search(node, ray, magnitude);
    }

    void Search(P3D_Node node, Ray ray, float maxDistance) {
        if (node.TriangleCount > 0) {
            AddToResults(node);
            return;
        }

        float distance = 0f;

        if (node.Bound.IntersectRay(ray, out distance) || !(distance <= maxDistance)) {
            if (node.PositiveIndex != 0) {
                Search(nodes[node.PositiveIndex], ray, maxDistance);
            }

            if (node.NegativeIndex != 0) {
                Search(nodes[node.NegativeIndex], ray, maxDistance);
            }
        }
    }

    void AddToResults(P3D_Node node) {
        for (int i = node.TriangleIndex; i < node.TriangleIndex + node.TriangleCount; i++) {
            closestTriangles.Add(triangles[i]);
        }
    }

    List<P3D_Triangle> GetTriangles() {
        List<P3D_Triangle> list = new();

        if (materialIndex >= 0 && mesh.subMeshCount >= 0) {
            int submesh = Mathf.Min(materialIndex, mesh.subMeshCount - 1);
            int[] array = mesh.GetTriangles(submesh);
            Vector3[] vertices = mesh.vertices;
            Vector2[] uv = mesh.uv;
            Vector2[] uv2 = mesh.uv2;

            if (array.Length > 0) {
                int num = array.Length / 3;

                for (int num2 = num - 1; num2 >= 0; num2--) {
                    int num3 = array[num2 * 3];
                    int num4 = array[num2 * 3 + 1];
                    int num5 = array[num2 * 3 + 2];
                    P3D_Triangle p3D_Triangle = P3D_Triangle.Create();
                    p3D_Triangle.PositionA = vertices[num3];
                    p3D_Triangle.PositionB = vertices[num4];
                    p3D_Triangle.PositionC = vertices[num5];
                    p3D_Triangle.Coord1A = uv[num3];
                    p3D_Triangle.Coord1B = uv[num4];
                    p3D_Triangle.Coord1C = uv[num5];

                    if (uv2.Length > 0) {
                        p3D_Triangle.Coord2A = uv2[num3];
                        p3D_Triangle.Coord2B = uv2[num4];
                        p3D_Triangle.Coord2C = uv2[num5];
                    }

                    list.Add(p3D_Triangle);
                }
            }
        }

        return list;
    }

    void Pack(P3D_Node node, List<P3D_Triangle> tris) {
        node.CalculateBound(tris);

        if (tris.Count < 5) {
            node.TriangleIndex = triangles.Count;
            node.TriangleCount = tris.Count;
            triangles.AddRange(tris);
            return;
        }

        List<P3D_Triangle> list = new();
        List<P3D_Triangle> list2 = new();
        int axis = 0;
        float pivot = 0f;
        CalculateAxisAndPivot(tris, ref axis, ref pivot);

        switch (axis) {
            case 0:
                foreach (P3D_Triangle tri in tris) {
                    if (tri.MidX >= pivot) {
                        list.Add(tri);
                    } else {
                        list2.Add(tri);
                    }
                }

                break;

            case 1:
                foreach (P3D_Triangle tri2 in tris) {
                    if (tri2.MidY >= pivot) {
                        list.Add(tri2);
                    } else {
                        list2.Add(tri2);
                    }
                }

                break;

            case 2:
                foreach (P3D_Triangle tri3 in tris) {
                    if (tri3.MidZ >= pivot) {
                        list.Add(tri3);
                    } else {
                        list2.Add(tri3);
                    }
                }

                break;
        }

        if (list.Count == 0 || list2.Count == 0) {
            list.Clear();
            list2.Clear();
            int num = tris.Count / 2;

            for (int i = 0; i < num; i++) {
                list.Add(tris[i]);
            }

            for (int j = num; j < tris.Count; j++) {
                list2.Add(tris[j]);
            }
        }

        node.PositiveIndex = nodes.Count;
        P3D_Node p3D_Node = new();
        nodes.Add(p3D_Node);
        Pack(p3D_Node, list);
        node.NegativeIndex = nodes.Count;
        P3D_Node p3D_Node2 = new();
        nodes.Add(p3D_Node2);
        Pack(p3D_Node2, list2);
    }

    void CalculateAxisAndPivot(List<P3D_Triangle> tris, ref int axis, ref float pivot) {
        Vector3 vector = tris[0].Min;
        Vector3 vector2 = tris[0].Max;
        Vector3 zero = Vector3.zero;

        foreach (P3D_Triangle tri in tris) {
            vector = Vector3.Min(vector, tri.Min);
            vector2 = Vector3.Max(vector2, tri.Max);
            zero += tri.PositionA + tri.PositionB + tri.PositionC;
        }

        Vector3 vector3 = vector2 - vector;

        if (vector3.x > vector3.y && vector3.x > vector3.z) {
            axis = 0;
            pivot = P3D_Helper.Divide(zero.x, tris.Count * 3f);
        } else if (vector3.y > vector3.x && vector3.y > vector3.z) {
            axis = 1;
            pivot = P3D_Helper.Divide(zero.y, tris.Count * 3f);
        } else {
            axis = 2;
            pivot = P3D_Helper.Divide(zero.z, tris.Count * 3f);
        }
    }
}