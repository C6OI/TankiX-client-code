using System;
using UnityEngine;

[Serializable]
public class P3D_PaintableTexture {
    [Tooltip("If your paintable has more than one texture then you can specify a group to select just one")]
    public P3D_Group Group;

    [Tooltip("The material index we want to paint")]
    public int MaterialIndex;

    [Tooltip("The texture we want to paint")]
    public string TextureName = "_MainTex";

    [Tooltip("The UV set used when painting this texture")]
    public P3D_CoordType Coord;

    [Tooltip("Should the material and texture get duplicated on awake? (useful for prefab clones)")]
    public bool DuplicateOnAwake;

    [Tooltip("Should the texture get created on awake? (useful for saving scene file size)")]
    public bool CreateOnAwake;

    [Tooltip("The width of the created texture")]
    public int CreateWidth = 512;

    [Tooltip("The height of the created texture")]
    public int CreateHeight = 512;

    [Tooltip("The pixel format of the created texture")]
    public P3D_Format CreateFormat;

    [Tooltip("The color of the created texture")]
    public Color CreateColor = Color.white;

    [Tooltip("Should the created etxture have mip maps?")]
    public bool CreateMipMaps = true;

    [Tooltip("Some shaders (e.g. Standard Shader) require you to enable keywords when adding new textures, you can specify that keyword here")]
    public string CreateKeyword;

    [SerializeField] P3D_Painter painter;

    public P3D_Painter Painter => painter;

    public void Paint(P3D_Brush brush, Vector2 uv) {
        if (painter != null) {
            painter.Paint(brush, uv);
        }
    }

    public void UpdateTexture(GameObject gameObject) {
        if (painter == null) {
            painter = new P3D_Painter();
        }

        painter.SetCanvas(gameObject, TextureName, MaterialIndex);
    }

    public void Awake(GameObject gameObject) {
        if (DuplicateOnAwake) {
            Material material = P3D_Helper.CloneMaterial(gameObject, MaterialIndex);

            if (material != null) {
                Texture texture = material.GetTexture(TextureName);

                if (texture != null) {
                    texture = P3D_Helper.Clone(texture);
                    material.SetTexture(TextureName, texture);
                }
            }
        }

        if (CreateOnAwake && CreateWidth > 0 && CreateHeight > 0) {
            Material material2 = P3D_Helper.GetMaterial(gameObject, MaterialIndex);

            if (material2 != null) {
                Texture texture2 = material2.GetTexture(TextureName);
                TextureFormat textureFormat = P3D_Helper.GetTextureFormat(CreateFormat);
                Texture2D texture2D = P3D_Helper.CreateTexture(CreateWidth, CreateHeight, textureFormat, CreateMipMaps);

                if (texture2 != null) {
                    Debug.LogWarning("There is already a texture in this texture slot, maybe set it to null to save memory?", gameObject);
                }

                texture2 = texture2D;
                P3D_Helper.ClearTexture(texture2D, CreateColor);
                material2.SetTexture(TextureName, texture2);

                if (!string.IsNullOrEmpty(CreateKeyword)) {
                    material2.EnableKeyword(CreateKeyword);
                }
            }
        }

        UpdateTexture(gameObject);
    }
}