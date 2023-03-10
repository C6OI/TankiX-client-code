using Lobby.ClientPayment.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;

namespace Lobby.ClientPayment.Impl {
    public class ClientPaymentActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            RegisterTemplates();
            RegisterSystems();
        }

        void RegisterTemplates() {
            TemplateRegistry.Register<GameCurrencyPackTemplate>();
            TemplateRegistry.Register<PaymentMethodTemplate>();
            TemplateRegistry.Register<SectionTemplate>();
            TemplateRegistry.Register<CountriesTemplate>();
        }

        static void RegisterSystems() {
            EngineService.RegisterSystem(new GoToUrlSystem());
        }

        protected override void Activate() {
            CreateEntity<CountriesTemplate>("payment/country");
        }
    }
}