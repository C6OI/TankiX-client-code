using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Steamworks;
using UnityEngine;

namespace Tanks.Lobby.ClientEntrance.Impl {
    public class SteamAuthenticationSystem : ECSSystem {
        [OnEventFire]
        public void AuthenticateSteamUser(RequestSteamAuthenticationEvent e, SingleNode<ClientSessionComponent> clientSession, [JoinAll] SingleNode<SteamComponent> steam,
            [JoinAll] SingleNode<EntranceScreenComponent> entranceScreen) {
            if (!string.IsNullOrEmpty(SteamComponent.Ticket)) {
                entranceScreen.component.LockScreen(true);
                PlayerPrefs.SetInt("SteamAuthentication", 0);
                AuthenticateSteamUserEvent authenticateSteamUserEvent = new();
                authenticateSteamUserEvent.HardwareFingerpring = HardwareFingerprintUtils.HardwareFingerprint;
                authenticateSteamUserEvent.SteamId = steam.component.SteamID ?? string.Empty;
                authenticateSteamUserEvent.SteamNickname = SteamFriends.GetPersonaName();
                authenticateSteamUserEvent.Ticket = SteamComponent.Ticket ?? string.Empty;
                AuthenticateSteamUserEvent eventInstance = authenticateSteamUserEvent;
                ScheduleEvent(eventInstance, clientSession);
            }
        }

        [OnEventFire]
        public void SteamAuthenticationButtonClick(ButtonClickEvent e, SingleNode<SteamLoginButtonComponent> button, [JoinAll] SingleNode<ClientSessionComponent> session,
            [JoinAll] SingleNode<EntranceScreenComponent> entranceScreen) {
            ScheduleEvent<RequestSteamAuthenticationEvent>(session);
            entranceScreen.component.LockScreen(true);
        }

        [OnEventFire]
        public void DisableSteamLoginButton(NodeAddedEvent e, SingleNode<SteamLoginButtonComponent> button) {
            if (!SteamManager.Initialized || string.IsNullOrEmpty(SteamComponent.Ticket)) {
                button.component.gameObject.SetActive(false);
            }
        }
    }
}