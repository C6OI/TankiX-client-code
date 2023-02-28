namespace Tanks.Battle.ClientGraphics.API {
    public class AmbientSoundFilter : SingleFadeSoundFilter {
        volatile float filterVolume;

        protected override float FilterVolume {
            get => filterVolume;
            set => filterVolume = value;
        }

        protected override void Awake() {
            base.Awake();
            FilterVolume = 0f;
        }
    }
}