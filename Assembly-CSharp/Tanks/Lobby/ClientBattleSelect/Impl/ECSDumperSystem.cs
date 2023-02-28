using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.System.Data.Exchange.ClientNetwork.API;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class ECSDumperSystem : ECSSystem {
        static readonly KeyCode DUMP_KEY = KeyCode.F12;

        [OnEventFire]
        public void Dump(UpdateEvent e, SingleNode<ClientSessionComponent> session) {
            if (Input.GetKeyUp(DUMP_KEY)) {
                Console.WriteLine("---------------Begin ECSDump--------------");
                Console.WriteLine(ECSStringDumper.Build(true));
                Console.WriteLine("----------------End ECSDump---------------");
            }
        }
    }
}