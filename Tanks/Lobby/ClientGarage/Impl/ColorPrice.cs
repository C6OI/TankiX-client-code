using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    [RequireComponent(typeof(AbstractPriceLabelComponent))]
    public class ColorPrice : ColorText {
        public override void SetColor(ColorData colorData) {
            AbstractPriceLabelComponent component = GetComponent<AbstractPriceLabelComponent>();
            component.Color = colorData.Color;

            if (!noApplyMaterial) {
                text.material = colorData.material;
            }
        }
    }
}