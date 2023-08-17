using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class ClientPaymentGUIActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static EngineService EngineService { get; set; }

        public void RegisterSystemsAndTemplates() {
            EngineService.RegisterSystem(new PaymentSectionSystem());
            EngineService.RegisterSystem(new GoodsSelectionScreenSystem());
            EngineService.RegisterSystem(new SelectCountryScreenSystem());
            EngineService.RegisterSystem(new MethodSelectionScreenSystem());
            EngineService.RegisterSystem(new PaymentProcessingScreenSystem());
            EngineService.RegisterSystem(new BankCardPaymentScreenSystem());
            EngineService.RegisterSystem(new MobilePaymentScreenSystem());
            EngineService.RegisterSystem(new MobilePaymentCheckoutScreenSystem());
            EngineService.RegisterSystem(new ForceEnterEmailScreenSystem());
            EngineService.RegisterSystem(new ExchangeCrystalsScreenSystem());
        }
    }
}