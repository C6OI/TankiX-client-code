using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

public class GraffitiSoundComponent : MonoBehaviour, Component {
    [SerializeField] AudioSource sound;

    public AudioSource Sound => sound;
}