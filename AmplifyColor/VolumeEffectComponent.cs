using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AmplifyColor {
    [Serializable]
    public class VolumeEffectComponent {
        public string componentName;

        public List<VolumeEffectField> fields;

        public VolumeEffectComponent(string name) {
            componentName = name;
            fields = new List<VolumeEffectField>();
        }

        public VolumeEffectComponent(Component c, VolumeEffectComponentFlags compFlags)
            : this(compFlags.componentName) {
            foreach (VolumeEffectFieldFlags componentField in compFlags.componentFields) {
                if (componentField.blendFlag) {
                    FieldInfo field = c.GetType().GetField(componentField.fieldName);

                    VolumeEffectField volumeEffectField = !VolumeEffectField.IsValidType(field.FieldType.FullName) ? null
                                                              : new VolumeEffectField(field, c);

                    if (volumeEffectField != null) {
                        fields.Add(volumeEffectField);
                    }
                }
            }
        }

        public VolumeEffectField AddField(FieldInfo pi, Component c) => AddField(pi, c, -1);

        public VolumeEffectField AddField(FieldInfo pi, Component c, int position) {
            VolumeEffectField volumeEffectField =
                !VolumeEffectField.IsValidType(pi.FieldType.FullName) ? null : new VolumeEffectField(pi, c);

            if (volumeEffectField != null) {
                if (position < 0 || position >= fields.Count) {
                    fields.Add(volumeEffectField);
                } else {
                    fields.Insert(position, volumeEffectField);
                }
            }

            return volumeEffectField;
        }

        public void RemoveEffectField(VolumeEffectField field) => fields.Remove(field);

        public void UpdateComponent(Component c, VolumeEffectComponentFlags compFlags) {
            VolumeEffectFieldFlags fieldFlags;

            foreach (VolumeEffectFieldFlags componentField in compFlags.componentFields) {
                fieldFlags = componentField;

                if (fieldFlags.blendFlag && !fields.Exists(s => s.fieldName == fieldFlags.fieldName)) {
                    FieldInfo field = c.GetType().GetField(fieldFlags.fieldName);

                    VolumeEffectField volumeEffectField = !VolumeEffectField.IsValidType(field.FieldType.FullName) ? null
                                                              : new VolumeEffectField(field, c);

                    if (volumeEffectField != null) {
                        fields.Add(volumeEffectField);
                    }
                }
            }
        }

        public VolumeEffectField GetEffectField(string fieldName) => fields.Find(s => s.fieldName == fieldName);

        public static FieldInfo[] ListAcceptableFields(Component c) {
            if (c == null) {
                return new FieldInfo[0];
            }

            FieldInfo[] source = c.GetType().GetFields();
            return source.Where(f => VolumeEffectField.IsValidType(f.FieldType.FullName)).ToArray();
        }

        public string[] GetFieldNames() => fields.Select(r => r.fieldName).ToArray();
    }
}