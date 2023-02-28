using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class HealingGraphicEffectComponent : BaseHealingGraphicEffectComponent<StopHealingGraphicsEffectEvent> {
        const string GLOBAL_EFFECT_ALPHA_KEY = "_RepairAlpha";

        const string BACK_BORDER_COEFF_KEY = "_RepairBackCoeff";

        const string LENGTH_EXTENSION_KEY = "_RepairAdditionalLengthExtension";

        const string FADE_ALPHA_RANGE_KEY = "_RepairFadeAlphaRange";

        const string MESH_SIZE_KEY = "_RepairVolume";

        const string CENTER_OFFSET_KEY = "_RepairCenter";

        const float MOVEMENT_DIRECTION_VALUE = 1f;

        const float FRONT_BORDER_COEFF_VALUE = 0f;

        const float BACK_BORDER_COEFF_VALUE = 0.69f;

        const float LENGTH_EXTENSION_VALUE = 0.125f;

        const float FADE_ALPHA_RANGE_VALUE = 1f;

        const float GLOBAL_EFFECT_ALPHA_VALUE = 1f;

        public override void InitRepairGraphicsEffect(HealingGraphicEffectInputs tankInputs, WeaponHealingGraphicEffectInputs weaponInputs, Transform soundRoot, Transform mountPoint) {
            base.InitRepairGraphicsEffect(tankInputs, weaponInputs, soundRoot, mountPoint);
            SkinnedMeshRenderer renderer = tankInputs.Renderer;
            SkinnedMeshRenderer renderer2 = weaponInputs.Renderer;
            Bounds localBounds = renderer.localBounds;
            Bounds localBounds2 = renderer2.localBounds;
            Vector3 extents = renderer.localBounds.extents;
            Vector3 extents2 = renderer2.localBounds.extents;
            Vector3 center = localBounds.center;
            Vector3 center2 = localBounds2.center;
            InitTankPartInputs(tankInputs, extents, center);
            InitTankPartInputs(weaponInputs, extents2, center2);
        }

        void InitTankPartInputs(HealingGraphicEffectInputs inputs, Vector3 extents, Vector3 effectCenter) {
            SkinnedMeshRenderer renderer = inputs.Renderer;
            Material[] materials = renderer.materials;
            int num = materials.Length;

            for (int i = 0; i < num; i++) {
                Material material = materials[i];
                SetInitialTankPartsParameters(material);
                SetConstantParameters(material);
                SetMeshSizeParams(extents, effectCenter, material);
            }
        }

        void SetMeshSizeParams(Vector3 extents, Vector3 effectCenter, Material mat) {
            mat.SetVector("_RepairVolume", new Vector4(extents.x, extents.y, extents.z, 0f));
            mat.SetVector("_RepairCenter", new Vector4(effectCenter.x, effectCenter.y, effectCenter.z, 0f));
        }

        void SetConstantParameters(Material mat) {
            mat.SetFloat("_RepairAlpha", 1f);
            mat.SetFloat("_RepairMovementDirection", 1f);
            mat.SetFloat("_RepairFrontCoeff", 0f);
            mat.SetFloat("_RepairBackCoeff", 0.69f);
            mat.SetFloat("_RepairAdditionalLengthExtension", 0.125f);
            mat.SetFloat("_RepairFadeAlphaRange", 1f);
        }
    }
}