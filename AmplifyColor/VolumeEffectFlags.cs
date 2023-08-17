using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AmplifyColor {
    [Serializable]
    public class VolumeEffectFlags {
        public List<VolumeEffectComponentFlags> components;

        public VolumeEffectFlags() => components = new List<VolumeEffectComponentFlags>();

        public void AddComponent(Component c) {
            VolumeEffectComponentFlags volumeEffectComponentFlags;

            if ((volumeEffectComponentFlags =
                     components.Find(s => s.componentName == string.Concat(c.GetType(), string.Empty))) !=
                null) {
                volumeEffectComponentFlags.UpdateComponentFlags(c);
            } else {
                components.Add(new VolumeEffectComponentFlags(c));
            }
        }

        public void UpdateFlags(VolumeEffect effectVol) {
            VolumeEffectComponent comp;

            foreach (VolumeEffectComponent component in effectVol.components) {
                comp = component;
                VolumeEffectComponentFlags volumeEffectComponentFlags = null;

                if ((volumeEffectComponentFlags = components.Find(s => s.componentName == comp.componentName)) == null) {
                    components.Add(new VolumeEffectComponentFlags(comp));
                } else {
                    volumeEffectComponentFlags.UpdateComponentFlags(comp);
                }
            }
        }

        public static void UpdateCamFlags(AmplifyColorBase[] effects, AmplifyColorVolumeBase[] volumes) {
            foreach (AmplifyColorBase amplifyColorBase in effects) {
                amplifyColorBase.EffectFlags = new VolumeEffectFlags();

                foreach (AmplifyColorVolumeBase amplifyColorVolumeBase in volumes) {
                    VolumeEffect volumeEffect = amplifyColorVolumeBase.EffectContainer.GetVolumeEffect(amplifyColorBase);

                    if (volumeEffect != null) {
                        amplifyColorBase.EffectFlags.UpdateFlags(volumeEffect);
                    }
                }
            }
        }

        public VolumeEffect GenerateEffectData(AmplifyColorBase go) {
            VolumeEffect volumeEffect = new(go);

            foreach (VolumeEffectComponentFlags component2 in components) {
                if (component2.blendFlag) {
                    Component component = go.GetComponent(component2.componentName);

                    if (component != null) {
                        volumeEffect.AddComponent(component, component2);
                    }
                }
            }

            return volumeEffect;
        }

        public VolumeEffectComponentFlags GetComponentFlags(string compName) =>
            components.Find(s => s.componentName == compName);

        public string[] GetComponentNames() => (from r in components
                                                where r.blendFlag
                                                select r.componentName).ToArray();
    }
}