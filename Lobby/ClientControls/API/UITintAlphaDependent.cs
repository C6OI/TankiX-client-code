using UnityEngine;

namespace Lobby.ClientControls.API {
    public class UITintAlphaDependent : UITint {
        public override void SetTint(Color tint) {
            Color tint2 = tint;
            tint2.a = 1f;
            tint2.r = Mathf.Lerp(tint2.r, 1f, 1f - tint.a);
            tint2.g = Mathf.Lerp(tint2.g, 1f, 1f - tint.a);
            tint2.b = Mathf.Lerp(tint2.b, 1f, 1f - tint.a);
            base.SetTint(tint2);
        }
    }
}