using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class HangarConfigComponent : MonoBehaviour, Component {
        [SerializeField] float autoRotateSpeed = 15f;

        [SerializeField] float keyboardRotateSpeed = 30f;

        [SerializeField] float mouseRotateFactor = 0.7f;

        [SerializeField] float decelerationRotateFactor = 1.5f;

        [SerializeField] float autoRotateDelay = 30f;

        [SerializeField] float flightToLocationTime = 1f;

        [SerializeField] float flightToLocationHigh = 5f;

        [SerializeField] float flightToTankTime = 1f;

        [SerializeField] float flightToTankRadius = 2f;

        public float AutoRotateSpeed => autoRotateSpeed;

        public float KeyboardRotateSpeed => keyboardRotateSpeed;

        public float MouseRotateFactor => mouseRotateFactor;

        public float DecelerationRotateFactor => decelerationRotateFactor;

        public float AutoRotateDelay => autoRotateDelay;

        public float FlightToLocationTime => flightToLocationTime;

        public float FlightToTankTime => flightToTankTime;

        public float FlightToTankRadius => flightToTankRadius;

        public float FlightToLocationHigh => flightToLocationHigh;
    }
}