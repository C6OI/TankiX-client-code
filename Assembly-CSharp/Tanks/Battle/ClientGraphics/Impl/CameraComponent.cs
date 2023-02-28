using Tanks.Battle.ClientGraphics.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

//TODO
namespace Tanks.Battle.ClientGraphics.Impl {
    public class CameraComponent : Component {
        public CameraComponent(Camera unityCamera) {
            UnityCamera = unityCamera;
            //PostProcessingBehaviour = unityCamera.GetComponent<PostProcessingBehaviour>();
            PostEffectsSets = unityCamera.GetComponents<PostEffectsSet>();
        }

        public Camera UnityCamera { get; set; }

        public bool Enabled {
            get => UnityCamera.enabled;
            set => UnityCamera.enabled = value;
        }

        public float FOV {
            get => UnityCamera.fieldOfView;
            set => UnityCamera.fieldOfView = value;
        }

        public DepthTextureMode DepthTextureMode {
            get => UnityCamera.depthTextureMode;
            set => UnityCamera.depthTextureMode = value;
        }

        public Matrix4x4 ProjectionMatrix => UnityCamera.projectionMatrix;

        public Matrix4x4 WorldToCameraMatrix => UnityCamera.worldToCameraMatrix;

        public PostEffectsSet[] PostEffectsSets { get; }

        public SetPostProcessing SetPostProcessing { get; set; }

        //public PostProcessingBehaviour PostProcessingBehaviour { get; set; }
    }
}