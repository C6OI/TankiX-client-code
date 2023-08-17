using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    [SerialVersionUID(635824351907885226L)]
    public class ShaftAimingControllerSoundEffectComponent : AbstractShaftSoundEffectComponent<SoundController> {
        public override void Play() => soundComponent.FadeIn();

        public override void Stop() => soundComponent.FadeOut();
    }
}