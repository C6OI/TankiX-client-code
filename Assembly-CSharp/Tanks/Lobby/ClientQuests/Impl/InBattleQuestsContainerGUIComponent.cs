using System;
using System.Collections;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientQuests.Impl {
    public class InBattleQuestsContainerGUIComponent : BehaviourComponent {
        [SerializeField] GameObject questPrefab;

        [SerializeField] GameObject questsContainer;

        public GameObject CreateQuestItem() {
            GameObject gameObject = Instantiate(questPrefab);
            gameObject.transform.SetParent(questsContainer.transform, false);
            SendMessage("RefreshCurve", SendMessageOptions.DontRequireReceiver);
            return gameObject;
        }

        public void DeleteAllQuests() {
            IEnumerator enumerator = questsContainer.transform.GetEnumerator();

            try {
                while (enumerator.MoveNext()) {
                    Transform transform = (Transform)enumerator.Current;
                    Destroy(transform.gameObject);
                }
            } finally {
                IDisposable disposable;

                if ((disposable = enumerator as IDisposable) != null) {
                    disposable.Dispose();
                }
            }
        }

        public void CreateQuest() {
            CreateQuestItem();
        }

        public void RemoveQuest() {
            Destroy(questsContainer.transform.GetChild(questsContainer.transform.childCount - 1).gameObject);
        }
    }
}