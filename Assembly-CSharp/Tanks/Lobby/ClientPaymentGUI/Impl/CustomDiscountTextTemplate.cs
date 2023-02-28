using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    [SerialVersionUID(636404710006892630L)]
    public interface CustomDiscountTextTemplate : Template {
        CustomDiscountTextComponent customDiscountText();
    }
}