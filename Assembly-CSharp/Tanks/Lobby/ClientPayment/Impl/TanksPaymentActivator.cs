using Assets.tanks.modules.lobby.ClientPayment.Scripts.API.Template;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Lobby.ClientPayment.API;
using tanks.modules.lobby.ClientPayment.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    public class TanksPaymentActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            TemplateRegistry.Register<PaymentNotificationTemplate>();
            TemplateRegistry.Register<SaleEndNotificationTemplate>();
            TemplateRegistry.Register<XCrystalsPackTemplate>();
            TemplateRegistry.Register<BaseStarterPackSpecialOfferTemplate>();
            TemplateRegistry.Register<LegendaryTankSpecialOfferTemplate>();
            TemplateRegistry.Register<StarterPackSpecialOfferTemplate>();
            TemplateRegistry.Register<FullGarageSpecialOfferTemplate>();
            TemplateRegistry.Register<LeagueFirstEntranceSpecialOfferTemplate>();
            TemplateRegistry.Register<PaymentSectionTemplate>();
            TemplateRegistry.Register<PersonalSpecialOfferPropertiesTemplate>();
            TemplateRegistry.Register<PremiumOfferTemplate>();
            TemplateRegistry.Register<GoldBonusOfferTemplate>();
            EngineService.RegisterSystem(new PaymentNotificationSystem());
            EngineService.RegisterSystem(new GoodsPriceSystem());
            EngineService.RegisterSystem(new SteamSystem());
        }

        protected override void Activate() {
            CreateEntity<PaymentSectionTemplate>("payment");
        }
    }
}