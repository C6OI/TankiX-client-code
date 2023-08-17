using System;
using System.Collections.Generic;
using System.Linq;

namespace AmplifyColor {
    [Serializable]
    public class VolumeEffectContainer {
        public List<VolumeEffect> volumes;

        public VolumeEffectContainer() => volumes = new List<VolumeEffect>();

        public void AddColorEffect(AmplifyColorBase colorEffect) {
            VolumeEffect volumeEffect;

            if ((volumeEffect = volumes.Find(s => s.gameObject == colorEffect)) != null) {
                volumeEffect.UpdateVolume();
                return;
            }

            volumeEffect = new VolumeEffect(colorEffect);
            volumes.Add(volumeEffect);
            volumeEffect.UpdateVolume();
        }

        public VolumeEffect AddJustColorEffect(AmplifyColorBase colorEffect) {
            VolumeEffect volumeEffect = new(colorEffect);
            volumes.Add(volumeEffect);
            return volumeEffect;
        }

        public VolumeEffect GetVolumeEffect(AmplifyColorBase colorEffect) {
            VolumeEffect volumeEffect = volumes.Find(s => s.gameObject == colorEffect);

            if (volumeEffect == null) {
                volumeEffect = volumes.Find(s =>
                    s.gameObject != null && s.gameObject.SharedInstanceID == colorEffect.SharedInstanceID);
            }

            return volumeEffect;
        }

        public void RemoveVolumeEffect(VolumeEffect volume) => volumes.Remove(volume);

        public AmplifyColorBase[] GetStoredEffects() => volumes.Select(r => r.gameObject).ToArray();
    }
}