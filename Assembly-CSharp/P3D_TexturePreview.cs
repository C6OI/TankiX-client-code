using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class P3D_TexturePreview : MonoBehaviour {
    static readonly List<P3D_TexturePreview> AllPreviews = new();

    int age;

    Material material;

    Material[] materials = new Material[1];

    MeshFilter meshFilter;

    MeshRenderer meshRenderer;

    protected virtual void Update() {
        if (age >= 2) {
            P3D_Helper.Destroy(gameObject);
        } else {
            age++;
        }
    }

    protected virtual void OnEnable() {
        AllPreviews.Add(this);
    }

    protected virtual void OnDisable() {
        AllPreviews.Remove(this);
    }

    protected virtual void OnDestroy() {
        P3D_Helper.Destroy(material);
    }

    public static void Show(Mesh mesh, int submeshIndex, Transform transform, float opacity, Texture2D texture, Vector2 tiling, Vector2 offset) {
        for (int num = AllPreviews.Count - 1; num >= 0; num--) {
            P3D_TexturePreview p3D_TexturePreview = AllPreviews[num];

            if (p3D_TexturePreview != null && p3D_TexturePreview.age > 0) {
                p3D_TexturePreview.UpdateShow(mesh, submeshIndex, transform, opacity, texture, tiling, offset);
                return;
            }
        }

        GameObject gameObject = new("P3D_TexturePreview");
        gameObject.hideFlags = HideFlags.HideAndDontSave;
        P3D_TexturePreview p3D_TexturePreview2 = gameObject.AddComponent<P3D_TexturePreview>();
        p3D_TexturePreview2.hideFlags = HideFlags.HideAndDontSave;
        p3D_TexturePreview2.UpdateShow(mesh, submeshIndex, transform, opacity, texture, tiling, offset);
    }

    public static void Mark() {
        for (int num = AllPreviews.Count - 1; num >= 0; num--) {
            P3D_TexturePreview p3D_TexturePreview = AllPreviews[num];

            if (p3D_TexturePreview != null) {
                p3D_TexturePreview.age = 5;
            }
        }
    }

    public static void Sweep() {
        for (int num = AllPreviews.Count - 1; num >= 0; num--) {
            P3D_TexturePreview p3D_TexturePreview = AllPreviews[num];

            if (p3D_TexturePreview != null && p3D_TexturePreview.age > 1) {
                AllPreviews.RemoveAt(num);
                P3D_Helper.Destroy(p3D_TexturePreview.gameObject);
            }
        }
    }

    void UpdateShow(Mesh mesh, int submeshIndex, Transform target, float opacity, Texture2D texture, Vector2 tiling, Vector2 offset) {
        if (target != null) {
            if (meshRenderer == null) {
                meshRenderer = gameObject.AddComponent<MeshRenderer>();
            }

            if (meshFilter == null) {
                meshFilter = gameObject.AddComponent<MeshFilter>();
            }

            if (material == null) {
                material = new Material(Shader.Find("Hidden/P3D_TexturePreview"));
            }

            transform.position = target.position;
            transform.rotation = target.rotation;
            transform.localScale = target.lossyScale;
            material.hideFlags = HideFlags.HideAndDontSave;
            material.SetMatrix("_Matrix", target.localToWorldMatrix);
            material.SetTexture("_Texture", texture);
            material.SetColor("_Tint", new Color(1f, 1f, 1f, opacity));
            material.SetTextureScale("_Texture", tiling);
            material.SetTextureOffset("_Texture", offset);
            Color white = Color.white;
            Color clear = Color.clear;
            Color clear2 = Color.clear;
            white.a = opacity;
            material.SetColor("_Tint", white);
            material.SetColor("_Base", clear);
            material.SetColor("_Opac", clear2);

            if (materials.Length != submeshIndex + 1) {
                materials = new Material[submeshIndex + 1];
            }

            for (int i = 0; i < submeshIndex; i++) {
                materials[i] = P3D_Helper.ClearMaterial;
            }

            materials[submeshIndex] = material;
            meshRenderer.sharedMaterials = materials;
            meshFilter.sharedMesh = mesh;
            age = 0;
        }
    }
}