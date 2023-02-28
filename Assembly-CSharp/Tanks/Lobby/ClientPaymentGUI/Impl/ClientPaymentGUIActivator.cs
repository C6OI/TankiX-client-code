using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Lobby.ClientPaymentGUI.Impl.Payguru;
using Tanks.Lobby.ClientPaymentGUI.Impl.TankRent;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class ClientPaymentGUIActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            EngineService.RegisterSystem(new PaymentSectionSystem());
            EngineService.RegisterSystem(new GoodsSelectionScreenSystem());
            EngineService.RegisterSystem(new SelectCountryScreenSystem());
            EngineService.RegisterSystem(new MethodSelectionScreenSystem());
            EngineService.RegisterSystem(new PaymentProcessingScreenSystem());
            EngineService.RegisterSystem(new BankCardPaymentScreenSystem());
            EngineService.RegisterSystem(new MobilePaymentScreenSystem());
            EngineService.RegisterSystem(new MobilePaymentCheckoutScreenSystem());
            EngineService.RegisterSystem(new QiwiWalletScreenSystem());
            EngineService.RegisterSystem(new XCrystalsSaleSystem());
            EngineService.RegisterSystem(new ShopXCrystalsSystem());
            EngineService.RegisterSystem(new DealsUISystem());
            EngineService.RegisterSystem(new StarterPackSystem());
            EngineService.RegisterSystem(new EnergyBonusUISystem());
            EngineService.RegisterSystem(new PackPurchaseSystem());
            EngineService.RegisterSystem(new TankRentOfferSystem());
            EngineService.RegisterSystem(new NewLeagueRewardSystem());
            EngineService.RegisterSystem(new PayguruUISystem());
        }
    }
}