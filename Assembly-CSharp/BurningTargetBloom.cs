using System.Collections.Generic;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;
using UnityEngine.Rendering;
using UnityStandardAssets.ImageEffects;

[RequireComponent(typeof(Bloom))]
public class BurningTargetBloom : MonoBehaviour {
    [HideInInspector] public Shader bloomMaskShader;

    [HideInInspector] public Shader brightPassFilterShader;

    public List<Renderer> targets;

    Bloom bloom;

    Material bloomFilterMaterial;

    RenderTexture bloomMask;

    Material bloomMaskMaterial;

    CommandBuffer commandBuffer;

    void Reset() {
        bloomMaskShader = Shader.Find("Alternativa/PostEffects/TargetBloom/BloomMask");
        brightPassFilterShader = Shader.Find("Alternativa/PostEffects/TargetBloom/BrightPassFilter");
    }

    void Update() {
        commandBuffer.Clear();
        commandBuffer.SetRenderTarget(bloomMask);
        commandBuffer.ClearRenderTarget(true, true, Color.black, 1f);

        foreach (Renderer target in targets) {
            if (target == null) {
                continue;
            }

            Material[] materials = target.materials;

            for (int i = 0; i < materials.Length; i++) {
                if (materials[i].HasProperty(TankMaterialPropertyNames.COLORING_MAP_ALPHA_DEF_ID)) {
                    if (materials[i].GetFloat(TankMaterialPropertyNames.COLORING_MAP_ALPHA_DEF_ID).Equals(1f)) {
                        commandBuffer.DrawRenderer(target, bloomMaskMaterial, i);
                    } else if (materials[i].HasProperty(TankMaterialPropertyNames.TEMPERATURE_ID)) {
                        float @float = materials[i].GetFloat(TankMaterialPropertyNames.TEMPERATURE_ID);

                        if (@float > 0f) {
                            commandBuffer.DrawRenderer(target, bloomMaskMaterial, i);
                        }
                    }
                } else {
                    commandBuffer.DrawRenderer(target, bloomMaskMaterial, i);
                }
            }
        }
    }

    void OnEnable() {
        commandBuffer = CreateCommandBuffer();
        GetComponent<Camera>().AddCommandBuffer(CameraEvent.AfterDepthTexture, commandBuffer);
        bloomMask = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
        bloomMaskMaterial = new Material(bloomMaskShader);
        bloom = GetComponent<Bloom>();
        bloom.enabled = false;
        bloom.brightPassFilterShader = brightPassFilterShader;

        bloom.brightPassFilterMaterial = new Material(brightPassFilterShader) {
            hideFlags = HideFlags.DontSave
        };
    }

    void OnDisable() {
        if (commandBuffer != null) {
            GetComponent<Camera>().RemoveCommandBuffer(CameraEvent.AfterDepthTexture, commandBuffer);
            commandBuffer.Dispose();
            bloomMask.Release();
        }
    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination) {
        bloom.brightPassFilterMaterial.SetTexture("_TargetBloomMask", bloomMask);
        bloom.OnRenderImage(source, destination);
    }

    CommandBuffer CreateCommandBuffer() {
        CommandBuffer commandBuffer = new();
        commandBuffer.name = "HotBloomCommandBuffer";
        return commandBuffer;
    }
}