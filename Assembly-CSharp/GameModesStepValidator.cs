using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

public class GameModesStepValidator : MonoBehaviour, ITutorialShowStepValidator {
    [Inject] public static EngineServiceInternal EngineService { get; set; }

    public bool ShowAllowed(long stepId) => true;
}