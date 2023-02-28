using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientBattleSelect.Impl;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    [Shared]
    [SerialVersionUID(1513167696434L)]
    public class PersonalXCrystalBonusComponent : Component, ComponentServerChangeListener {
        bool finished = true;

        [Inject] public static EngineService EngineService { get; set; }

        public long StartTime { get; set; }

        public long DurationInSec { get; set; }

        public Date EndDate { get; set; }

        public double Multiplier { get; set; }

        public bool Used { get; set; }

        public bool Finished { get; set; }

        public void ChangedOnServer(Entity entity) {
            if (Finished != finished) {
                finished = Finished;

                if (finished) {
                    EngineService.Engine.ScheduleEvent<FinishPersonalXCrystalBonusEvent>(entity);
                } else {
                    EngineService.Engine.ScheduleEvent<StartPersonalXCrystalBonusEvent>(entity);
                }
            }
        }

        public long timeToEndSec() {
            float num = EndDate - Date.Now;
            Debug.Log("PersonalXCrystalBonusComponent.timeToEndSec delta=" + num);
            return (long)(num / 1000f);
        }
    }
}