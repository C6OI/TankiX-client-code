using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public abstract class BaseRicochetSoundEffectComponent : BehaviourComponent {
        [SerializeField] AudioSource assetSource;

        [SerializeField] float lifetime = 2f;

        public virtual void PlayEffect(Vector3 position) {
            GetInstanceFromPoolEvent getInstanceFromPoolEvent = new();
            getInstanceFromPoolEvent.Prefab = assetSource.gameObject;
            getInstanceFromPoolEvent.AutoRecycleTime = lifetime;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent2 = getInstanceFromPoolEvent;
            ScheduleEvent(getInstanceFromPoolEvent2, new EntityStub());
            Transform instance = getInstanceFromPoolEvent2.Instance;
            instance.position = position;
            instance.rotation = Quaternion.identity;
            AudioSource component = instance.GetComponent<AudioSource>();
            component.gameObject.SetActive(true);
            Play(component);
        }

        public abstract void Play(AudioSource sourceInstance);
    }
}