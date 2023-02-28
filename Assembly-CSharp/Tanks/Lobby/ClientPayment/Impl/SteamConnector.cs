using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Steamworks;
using Tanks.Lobby.ClientEntrance.Impl;
using Tanks.Lobby.ClientNavigation.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientPayment.Impl {
    public class SteamConnector : MonoBehaviour {
        static bool initialized;

        static SteamConnector instance;

        static SteamComponent steamComponent;

        protected static Callback<GetAuthSessionTicketResponse_t> GetAuthSessionTicketResponse;

        protected static Callback<MicroTxnAuthorizationResponse_t> MicroTxnAuthorizationResponse;

        protected static Callback<DlcInstalled_t> DlcInstalled;

        [SerializeField] SteamComponent steamEntityBehaviourPrefab;

        [Inject] public static EngineServiceInternal EngineService { get; set; }

        public void Start() {
            if (instance == null) {
                instance = this;
            } else {
                Destroy(gameObject);
            }

            instance.Initialize();
        }

        static void OnMicroTxnAuthorizationResponse(MicroTxnAuthorizationResponse_t pCallback) {
            if (steamComponent != null) {
                MicroTxnAuthorizationResponseEvent eventInstance = new(pCallback);
                EngineService.Engine.ScheduleEvent(eventInstance, steamComponent.Entity);
            }
        }

        static void OnGetAuthSessionTicketResponse(GetAuthSessionTicketResponse_t pCallback) {
            if (steamComponent != null) {
                steamComponent.OnGetAuthSessionTicketResponse(pCallback);
            }
        }

        static void OnDlcInstalled(DlcInstalled_t pCallback) {
            if (steamComponent != null && !string.IsNullOrEmpty(SteamComponent.Ticket)) {
                RequestCheckSteamDlcInstalledEvent eventInstance = new();
                EngineService.Engine.ScheduleEvent(eventInstance, steamComponent.Entity);
            }
        }

        void Initialize() {
            if (SteamManager.Initialized) {
                SteamManager steamManager = FindObjectOfType<SteamManager>();

                if (steamManager != null && steamManager.GetComponent<SkipRemoveOnSceneSwitch>() == null) {
                    steamManager.gameObject.AddComponent<SkipRemoveOnSceneSwitch>();
                }

                if (!initialized) {
                    initialized = true;
                    GetAuthSessionTicketResponse = Callback<GetAuthSessionTicketResponse_t>.Create(OnGetAuthSessionTicketResponse);
                    MicroTxnAuthorizationResponse = Callback<MicroTxnAuthorizationResponse_t>.Create(OnMicroTxnAuthorizationResponse);
                    DlcInstalled = Callback<DlcInstalled_t>.Create(OnDlcInstalled);
                }

                if (steamComponent == null) {
                    steamComponent = Instantiate(steamEntityBehaviourPrefab);
                    steamComponent.transform.SetParent(transform);
                    steamComponent.GetTicket();
                }
            } else {
                Destroy(this);
            }
        }
    }
}