using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Lobby.ClientPayment.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    public class TanksPaymentActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        [Inject] public static EngineService EngineService { get; set; }

        public void RegisterSystemsAndTemplates() {
            TemplateRegistry.Register<XCrystalsPackTemplate>();
            TemplateRegistry.Register<ExchangePackTemplate>();
        }
    }
}