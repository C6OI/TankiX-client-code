using System.Collections.Generic;
using UnityEngine;

public class P3D_Paintable : MonoBehaviour {
    public static List<P3D_Paintable> AllPaintables = new();

    [Tooltip("The submesh in the attached renderer we want to paint to")]
    public int SubMeshIndex;

    [Tooltip("The amount of seconds it takes for the mesh data to be updated (useful for animated meshes). -1 = No updates")]
    public float UpdateInterval = -1f;

    [Tooltip("The amount of seconds it takes for texture modifications to get applied")]
    public float ApplyInterval = 0.01f;

    [Tooltip("All the textures this paintable is associated with")]
    public List<P3D_PaintableTexture> Textures;

    float applyCooldown;

    Mesh bakedMesh;

    P3D_Tree tree;

    float updateCooldown;

    public bool IsReady {
        get {
            if (tree != null && tree.IsReady) {
                return true;
            }

            return false;
        }
    }

    protected virtual void Awake() {
        if (Textures != null) {
            for (int num = Textures.Count - 1; num >= 0; num--) {
                P3D_PaintableTexture p3D_PaintableTexture = Textures[num];

                if (p3D_PaintableTexture != null) {
                    p3D_PaintableTexture.Awake(gameObject);
                }
            }
        }

        UpdateTree();
    }

    protected virtual void Update() {
        applyCooldown -= Time.deltaTime;

        if (applyCooldown <= 0f && Textures != null) {
            applyCooldown = ApplyInterval;

            for (int num = Textures.Count - 1; num >= 0; num--) {
                P3D_PaintableTexture p3D_PaintableTexture = Textures[num];

                if (p3D_PaintableTexture != null && p3D_PaintableTexture.Painter != null) {
                    p3D_PaintableTexture.Painter.Apply();
                }
            }
        }

        updateCooldown -= Time.deltaTime;
    }

    protected virtual void OnEnable() {
        AllPaintables.Add(this);
    }

    protected virtual void OnDisable() {
        AllPaintables.Remove(this);
    }

    public static void ScenePaintNearest(P3D_Brush brush, Vector3 position, float maxDistance, int layerMask = -1, int groupMask = -1) {
        P3D_Paintable p3D_Paintable = null;
        P3D_Result result = null;

        for (int num = AllPaintables.Count - 1; num >= 0; num--) {
            P3D_Paintable p3D_Paintable2 = AllPaintables[num];

            if (P3D_Helper.IndexInMask(p3D_Paintable2.gameObject.layer, layerMask)) {
                P3D_Tree p3D_Tree = p3D_Paintable2.GetTree();

                if (p3D_Tree != null) {
                    Transform transform = p3D_Paintable2.transform;
                    float uniformScale = P3D_Helper.GetUniformScale(transform);

                    if (uniformScale != 0f) {
                        Vector3 point = transform.InverseTransformPoint(position);
                        P3D_Result p3D_Result = p3D_Tree.FindNearest(point, maxDistance);

                        if (p3D_Result != null) {
                            p3D_Paintable = p3D_Paintable2;
                            result = p3D_Result;
                            maxDistance *= p3D_Result.Distance01;
                        }
                    }
                }
            }
        }

        if (p3D_Paintable != null) {
            p3D_Paintable.Paint(brush, result, groupMask);
        }
    }

    public static void ScenePaintBetweenNearestRaycast(P3D_Brush brush, Vector3 startPosition, Vector3 endPosition, int layerMask = -1, int groupMask = -1) {
        float num = Vector3.Distance(startPosition, endPosition);

        if (num == 0f) {
            return;
        }

        P3D_Paintable p3D_Paintable = null;
        RaycastHit hitInfo = default;
        P3D_Result p3D_Result = null;

        if (Physics.Raycast(startPosition, endPosition - startPosition, out hitInfo, num, layerMask)) {
            p3D_Paintable = hitInfo.collider.GetComponent<P3D_Paintable>();
            num = hitInfo.distance;
        }

        for (int num2 = AllPaintables.Count - 1; num2 >= 0; num2--) {
            P3D_Paintable p3D_Paintable2 = AllPaintables[num2];

            if (P3D_Helper.IndexInMask(p3D_Paintable2.gameObject.layer, layerMask)) {
                P3D_Tree p3D_Tree = p3D_Paintable2.GetTree();

                if (p3D_Tree != null) {
                    Transform transform = p3D_Paintable2.transform;
                    Vector3 vector = transform.InverseTransformPoint(startPosition);
                    Vector3 vector2 = transform.InverseTransformPoint(endPosition);
                    Vector3 normalized = (vector2 - vector).normalized;
                    P3D_Result p3D_Result2 = p3D_Tree.FindBetweenNearest(vector, vector + normalized * num);

                    if (p3D_Result2 != null) {
                        p3D_Paintable = p3D_Paintable2;
                        p3D_Result = p3D_Result2;
                        num *= p3D_Result2.Distance01;
                    }
                }
            }
        }

        if (p3D_Paintable != null) {
            if (p3D_Result != null) {
                p3D_Paintable.Paint(brush, p3D_Result, groupMask);
            } else {
                p3D_Paintable.Paint(brush, hitInfo, groupMask);
            }
        }
    }

    public static void ScenePaintBetweenNearest(P3D_Brush brush, Vector3 startPosition, Vector3 endPosition, int layerMask = -1, int groupMask = -1) {
        float num = Vector3.Distance(startPosition, endPosition);

        if (num == 0f) {
            return;
        }

        P3D_Paintable p3D_Paintable = null;
        P3D_Result p3D_Result = null;

        for (int num2 = AllPaintables.Count - 1; num2 >= 0; num2--) {
            P3D_Paintable p3D_Paintable2 = AllPaintables[num2];

            if (P3D_Helper.IndexInMask(p3D_Paintable2.gameObject.layer, layerMask)) {
                P3D_Tree p3D_Tree = p3D_Paintable2.GetTree();

                if (p3D_Tree != null) {
                    Transform transform = p3D_Paintable2.transform;
                    Vector3 vector = transform.InverseTransformPoint(startPosition);
                    Vector3 vector2 = transform.InverseTransformPoint(endPosition);
                    Vector3 normalized = (vector2 - vector).normalized;
                    P3D_Result p3D_Result2 = p3D_Tree.FindBetweenNearest(vector, vector + normalized * num);

                    if (p3D_Result2 != null) {
                        p3D_Paintable = p3D_Paintable2;
                        p3D_Result = p3D_Result2;
                        num *= p3D_Result2.Distance01;
                    }
                }
            }
        }

        if (p3D_Paintable != null && p3D_Result != null) {
            p3D_Paintable.Paint(brush, p3D_Result, groupMask);
        }
    }

    public static void ScenePaintBetweenAll(P3D_Brush brush, Vector3 startPosition, Vector3 endPosition, int layerMask = -1, int groupMask = -1) {
        for (int num = AllPaintables.Count - 1; num >= 0; num--) {
            P3D_Paintable p3D_Paintable = AllPaintables[num];

            if (P3D_Helper.IndexInMask(p3D_Paintable.gameObject.layer, layerMask)) {
                p3D_Paintable.PaintBetweenAll(brush, startPosition, endPosition, groupMask);
            }
        }
    }

    public static void ScenePaintPerpedicularNearest(P3D_Brush brush, Vector3 position, float maxDistance, int layerMask = -1, int groupMask = -1) {
        P3D_Paintable p3D_Paintable = null;
        P3D_Result result = null;

        for (int num = AllPaintables.Count - 1; num >= 0; num--) {
            P3D_Paintable p3D_Paintable2 = AllPaintables[num];

            if (P3D_Helper.IndexInMask(p3D_Paintable2.gameObject.layer, layerMask)) {
                P3D_Tree p3D_Tree = p3D_Paintable2.GetTree();

                if (p3D_Tree != null) {
                    Transform transform = p3D_Paintable2.transform;
                    float uniformScale = P3D_Helper.GetUniformScale(transform);

                    if (uniformScale != 0f) {
                        Vector3 point = transform.InverseTransformPoint(position);
                        P3D_Result p3D_Result = p3D_Tree.FindPerpendicularNearest(point, maxDistance);

                        if (p3D_Result != null) {
                            p3D_Paintable = p3D_Paintable2;
                            result = p3D_Result;
                            maxDistance *= p3D_Result.Distance01;
                        }
                    }
                }
            }
        }

        if (p3D_Paintable != null) {
            p3D_Paintable.Paint(brush, result, groupMask);
        }
    }

    public static void ScenePaintPerpedicularAll(P3D_Brush brush, Vector3 position, float maxDistance, int layerMask = -1, int groupMask = -1) {
        for (int num = AllPaintables.Count - 1; num >= 0; num--) {
            P3D_Paintable p3D_Paintable = AllPaintables[num];

            if (P3D_Helper.IndexInMask(p3D_Paintable.gameObject.layer, layerMask)) {
                p3D_Paintable.PaintPerpendicularAll(brush, position, maxDistance, groupMask);
            }
        }
    }

    public void PaintPerpendicularNearest(P3D_Brush brush, Vector3 position, float maxDistance, int groupMask = -1) {
        if (CheckTree()) {
            float uniformScale = P3D_Helper.GetUniformScale(transform);

            if (uniformScale != 0f) {
                Vector3 point = transform.InverseTransformPoint(position);
                P3D_Result result = tree.FindPerpendicularNearest(point, maxDistance / uniformScale);
                Paint(brush, result, groupMask);
            }
        }
    }

    public void PaintPerpendicularAll(P3D_Brush brush, Vector3 position, float maxDistance, int groupMask = -1) {
        if (CheckTree()) {
            float uniformScale = P3D_Helper.GetUniformScale(transform);

            if (uniformScale != 0f) {
                Vector3 point = transform.InverseTransformPoint(position);
                List<P3D_Result> results = tree.FindPerpendicularAll(point, maxDistance / uniformScale);
                Paint(brush, results, groupMask);
            }
        }
    }

    public void PaintNearest(P3D_Brush brush, Vector3 position, float maxDistance, int groupMask = -1) {
        if (CheckTree()) {
            float uniformScale = P3D_Helper.GetUniformScale(transform);

            if (uniformScale != 0f) {
                Vector3 point = transform.InverseTransformPoint(position);
                P3D_Result result = tree.FindNearest(point, maxDistance / uniformScale);
                Paint(brush, result, groupMask);
            }
        }
    }

    public void PaintBetweenNearest(P3D_Brush brush, Vector3 startPosition, Vector3 endPosition, int groupMask = -1) {
        if (CheckTree()) {
            Vector3 startPoint = transform.InverseTransformPoint(startPosition);
            Vector3 endPoint = transform.InverseTransformPoint(endPosition);
            P3D_Result result = tree.FindBetweenNearest(startPoint, endPoint);
            Paint(brush, result, groupMask);
        }
    }

    public void PaintBetweenAll(P3D_Brush brush, Vector3 startPosition, Vector3 endPosition, int groupMask = -1) {
        if (CheckTree()) {
            Vector3 startPoint = transform.InverseTransformPoint(startPosition);
            Vector3 endPoint = transform.InverseTransformPoint(endPosition);
            List<P3D_Result> results = tree.FindBetweenAll(startPoint, endPoint);
            Paint(brush, results, groupMask);
        }
    }

    public void Paint(P3D_Brush brush, List<P3D_Result> results, int groupMask = -1) {
        if (results != null) {
            for (int i = 0; i < results.Count; i++) {
                Paint(brush, results[i], groupMask);
            }
        }
    }

    public void Paint(P3D_Brush brush, P3D_Result result, int groupMask = -1) {
        if (result == null || Textures == null) {
            return;
        }

        for (int num = Textures.Count - 1; num >= 0; num--) {
            P3D_PaintableTexture p3D_PaintableTexture = Textures[num];

            if (p3D_PaintableTexture != null && P3D_Helper.IndexInMask(p3D_PaintableTexture.Group, groupMask)) {
                p3D_PaintableTexture.Paint(brush, result.GetUV(p3D_PaintableTexture.Coord));
            }
        }
    }

    public void Paint(P3D_Brush brush, RaycastHit hit, int groupMask = -1) {
        if (Textures == null) {
            return;
        }

        for (int num = Textures.Count - 1; num >= 0; num--) {
            P3D_PaintableTexture p3D_PaintableTexture = Textures[num];

            if (p3D_PaintableTexture != null && P3D_Helper.IndexInMask(p3D_PaintableTexture.Group, groupMask)) {
                p3D_PaintableTexture.Paint(brush, P3D_Helper.GetUV(hit, p3D_PaintableTexture.Coord));
            }
        }
    }

    public void Paint(P3D_Brush brush, Vector2 uv, int groupMask = -1) {
        if (Textures == null) {
            return;
        }

        for (int num = Textures.Count - 1; num >= 0; num--) {
            P3D_PaintableTexture p3D_PaintableTexture = Textures[num];

            if (p3D_PaintableTexture != null && P3D_Helper.IndexInMask(p3D_PaintableTexture.Group, groupMask)) {
                p3D_PaintableTexture.Paint(brush, uv);
            }
        }
    }

    public P3D_Tree GetTree() {
        if (tree != null && UpdateInterval >= 0f && updateCooldown < 0f) {
            updateCooldown = UpdateInterval;
            UpdateTree();
        }

        return tree;
    }

    [ContextMenu("Add Texture")]
    public void AddTexture() {
        P3D_PaintableTexture item = new();

        if (Textures == null) {
            Textures = new List<P3D_PaintableTexture>();
        }

        Textures.Add(item);
    }

    [ContextMenu("Update Tree")]
    public void UpdateTree() {
        bool forceUpdate = false;
        Mesh mesh = P3D_Helper.GetMesh(gameObject, ref bakedMesh);

        if (bakedMesh != null) {
            forceUpdate = true;
        }

        if (tree == null) {
            tree = new P3D_Tree();
        }

        tree.SetMesh(mesh, SubMeshIndex, forceUpdate);
    }

    bool CheckTree() {
        if (tree != null) {
            if (UpdateInterval >= 0f && updateCooldown < 0f) {
                updateCooldown = UpdateInterval;
                UpdateTree();
            }

            return true;
        }

        return false;
    }
}