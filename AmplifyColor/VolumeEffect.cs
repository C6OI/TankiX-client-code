using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AmplifyColor {
    [Serializable]
    public class VolumeEffect {
        public AmplifyColorBase gameObject;

        public List<VolumeEffectComponent> components;

        public VolumeEffect(AmplifyColorBase effect) {
            gameObject = effect;
            components = new List<VolumeEffectComponent>();
        }

        public static VolumeEffect BlendValuesToVolumeEffect(VolumeEffectFlags flags, VolumeEffect volume1,
            VolumeEffect volume2, float blend) {
            VolumeEffect volumeEffect = new(volume1.gameObject);
            VolumeEffectComponentFlags compFlags;

            foreach (VolumeEffectComponentFlags component in flags.components) {
                compFlags = component;

                if (!compFlags.blendFlag) {
                    continue;
                }

                VolumeEffectComponent volumeEffectComponent =
                    volume1.components.Find(s => s.componentName == compFlags.componentName);

                VolumeEffectComponent volumeEffectComponent2 =
                    volume2.components.Find(s => s.componentName == compFlags.componentName);

                if (volumeEffectComponent == null || volumeEffectComponent2 == null) {
                    continue;
                }

                VolumeEffectComponent volumeEffectComponent3 = new(volumeEffectComponent.componentName);
                VolumeEffectFieldFlags fieldFlags;

                foreach (VolumeEffectFieldFlags componentField in compFlags.componentFields) {
                    fieldFlags = componentField;

                    if (!fieldFlags.blendFlag) {
                        continue;
                    }

                    VolumeEffectField volumeEffectField =
                        volumeEffectComponent.fields.Find(s => s.fieldName == fieldFlags.fieldName);

                    VolumeEffectField volumeEffectField2 =
                        volumeEffectComponent2.fields.Find(s => s.fieldName == fieldFlags.fieldName);

                    if (volumeEffectField != null && volumeEffectField2 != null) {
                        VolumeEffectField volumeEffectField3 = new(volumeEffectField.fieldName, volumeEffectField.fieldType);

                        switch (volumeEffectField3.fieldType) {
                            case "System.Single":
                                volumeEffectField3.valueSingle = Mathf.Lerp(volumeEffectField.valueSingle,
                                    volumeEffectField2.valueSingle,
                                    blend);

                                break;

                            case "System.Boolean":
                                volumeEffectField3.valueBoolean = volumeEffectField2.valueBoolean;
                                break;

                            case "UnityEngine.Vector2":
                                volumeEffectField3.valueVector2 = Vector2.Lerp(volumeEffectField.valueVector2,
                                    volumeEffectField2.valueVector2,
                                    blend);

                                break;

                            case "UnityEngine.Vector3":
                                volumeEffectField3.valueVector3 = Vector3.Lerp(volumeEffectField.valueVector3,
                                    volumeEffectField2.valueVector3,
                                    blend);

                                break;

                            case "UnityEngine.Vector4":
                                volumeEffectField3.valueVector4 = Vector4.Lerp(volumeEffectField.valueVector4,
                                    volumeEffectField2.valueVector4,
                                    blend);

                                break;

                            case "UnityEngine.Color":
                                volumeEffectField3.valueColor = Color.Lerp(volumeEffectField.valueColor,
                                    volumeEffectField2.valueColor,
                                    blend);

                                break;
                        }

                        volumeEffectComponent3.fields.Add(volumeEffectField3);
                    }
                }

                volumeEffect.components.Add(volumeEffectComponent3);
            }

            return volumeEffect;
        }

        public VolumeEffectComponent AddComponent(Component c, VolumeEffectComponentFlags compFlags) {
            if (compFlags == null) {
                VolumeEffectComponent volumeEffectComponent = new(string.Concat(c.GetType(), string.Empty));
                components.Add(volumeEffectComponent);
                return volumeEffectComponent;
            }

            VolumeEffectComponent volumeEffectComponent2;

            if ((volumeEffectComponent2 =
                     components.Find(s => s.componentName == string.Concat(c.GetType(), string.Empty))) !=
                null) {
                volumeEffectComponent2.UpdateComponent(c, compFlags);
                return volumeEffectComponent2;
            }

            VolumeEffectComponent volumeEffectComponent3 = new(c, compFlags);
            components.Add(volumeEffectComponent3);
            return volumeEffectComponent3;
        }

        public void RemoveEffectComponent(VolumeEffectComponent comp) => components.Remove(comp);

        public void UpdateVolume() {
            if (gameObject == null) {
                return;
            }

            VolumeEffectFlags effectFlags = gameObject.EffectFlags;

            foreach (VolumeEffectComponentFlags component2 in effectFlags.components) {
                if (component2.blendFlag) {
                    Component component = gameObject.GetComponent(component2.componentName);

                    if (component != null) {
                        AddComponent(component, component2);
                    }
                }
            }
        }

        public void SetValues(AmplifyColorBase targetColor) {
            VolumeEffectFlags effectFlags = targetColor.EffectFlags;
            GameObject gameObject = targetColor.gameObject;
            VolumeEffectComponentFlags compFlags;

            foreach (VolumeEffectComponentFlags component2 in effectFlags.components) {
                compFlags = component2;

                if (!compFlags.blendFlag) {
                    continue;
                }

                Component component = gameObject.GetComponent(compFlags.componentName);

                VolumeEffectComponent volumeEffectComponent =
                    components.Find(s => s.componentName == compFlags.componentName);

                if (component == null || volumeEffectComponent == null) {
                    continue;
                }

                VolumeEffectFieldFlags fieldFlags;

                foreach (VolumeEffectFieldFlags componentField in compFlags.componentFields) {
                    fieldFlags = componentField;

                    if (!fieldFlags.blendFlag) {
                        continue;
                    }

                    FieldInfo field = component.GetType().GetField(fieldFlags.fieldName);

                    VolumeEffectField volumeEffectField =
                        volumeEffectComponent.fields.Find(s => s.fieldName == fieldFlags.fieldName);

                    if (field != null && volumeEffectField != null) {
                        switch (field.FieldType.FullName) {
                            case "System.Single":
                                field.SetValue(component, volumeEffectField.valueSingle);
                                break;

                            case "System.Boolean":
                                field.SetValue(component, volumeEffectField.valueBoolean);
                                break;

                            case "UnityEngine.Vector2":
                                field.SetValue(component, volumeEffectField.valueVector2);
                                break;

                            case "UnityEngine.Vector3":
                                field.SetValue(component, volumeEffectField.valueVector3);
                                break;

                            case "UnityEngine.Vector4":
                                field.SetValue(component, volumeEffectField.valueVector4);
                                break;

                            case "UnityEngine.Color":
                                field.SetValue(component, volumeEffectField.valueColor);
                                break;
                        }
                    }
                }
            }
        }

        public void BlendValues(AmplifyColorBase targetColor, VolumeEffect other, float blendAmount) {
            VolumeEffectFlags effectFlags = targetColor.EffectFlags;
            GameObject gameObject = targetColor.gameObject;
            VolumeEffectComponentFlags compFlags;

            foreach (VolumeEffectComponentFlags component2 in effectFlags.components) {
                compFlags = component2;

                if (!compFlags.blendFlag) {
                    continue;
                }

                Component component = gameObject.GetComponent(compFlags.componentName);

                VolumeEffectComponent volumeEffectComponent =
                    components.Find(s => s.componentName == compFlags.componentName);

                VolumeEffectComponent volumeEffectComponent2 =
                    other.components.Find(s => s.componentName == compFlags.componentName);

                if (component == null || volumeEffectComponent == null || volumeEffectComponent2 == null) {
                    continue;
                }

                VolumeEffectFieldFlags fieldFlags;

                foreach (VolumeEffectFieldFlags componentField in compFlags.componentFields) {
                    fieldFlags = componentField;

                    if (!fieldFlags.blendFlag) {
                        continue;
                    }

                    FieldInfo field = component.GetType().GetField(fieldFlags.fieldName);

                    VolumeEffectField volumeEffectField =
                        volumeEffectComponent.fields.Find(s => s.fieldName == fieldFlags.fieldName);

                    VolumeEffectField volumeEffectField2 =
                        volumeEffectComponent2.fields.Find(s => s.fieldName == fieldFlags.fieldName);

                    if (field != null && volumeEffectField != null && volumeEffectField2 != null) {
                        switch (field.FieldType.FullName) {
                            case "System.Single":
                                field.SetValue(component,
                                    Mathf.Lerp(volumeEffectField.valueSingle, volumeEffectField2.valueSingle, blendAmount));

                                break;

                            case "System.Boolean":
                                field.SetValue(component, volumeEffectField2.valueBoolean);
                                break;

                            case "UnityEngine.Vector2":
                                field.SetValue(component,
                                    Vector2.Lerp(volumeEffectField.valueVector2,
                                        volumeEffectField2.valueVector2,
                                        blendAmount));

                                break;

                            case "UnityEngine.Vector3":
                                field.SetValue(component,
                                    Vector3.Lerp(volumeEffectField.valueVector3,
                                        volumeEffectField2.valueVector3,
                                        blendAmount));

                                break;

                            case "UnityEngine.Vector4":
                                field.SetValue(component,
                                    Vector4.Lerp(volumeEffectField.valueVector4,
                                        volumeEffectField2.valueVector4,
                                        blendAmount));

                                break;

                            case "UnityEngine.Color":
                                field.SetValue(component,
                                    Color.Lerp(volumeEffectField.valueColor, volumeEffectField2.valueColor, blendAmount));

                                break;
                        }
                    }
                }
            }
        }

        public VolumeEffectComponent GetEffectComponent(string compName) =>
            components.Find(s => s.componentName == compName);

        public static Component[] ListAcceptableComponents(AmplifyColorBase go) {
            if (go == null) {
                return new Component[0];
            }

            Component[] source = go.GetComponents(typeof(Component));

            return source.Where(comp =>
                comp != null &&
                !string.Concat(comp.GetType(), string.Empty).StartsWith("UnityEngine.") &&
                comp.GetType() != typeof(AmplifyColorBase)).ToArray();
        }

        public string[] GetComponentNames() => components.Select(r => r.componentName).ToArray();
    }
}