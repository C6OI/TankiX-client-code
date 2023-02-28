using System.Collections.Generic;
using Tanks.Battle.ClientBattleSelect.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class MVPOtherStatComponent : MonoBehaviour {
        static int MAX_SHOWED_ITEM = 4;

        [SerializeField] MVPStatElementComponent flagsDelivered;

        [SerializeField] MVPStatElementComponent flagsReturned;

        [SerializeField] MVPStatElementComponent damage;

        [SerializeField] MVPStatElementComponent killStreak;

        [SerializeField] MVPStatElementComponent bonuseTaken;

        List<UserResult> allUsers;

        UserResult mvp;

        int showedItems;

        public void Set(UserResult mvp, BattleResultForClient battleResults) {
            this.mvp = mvp;
            allUsers = new List<UserResult>();
            allUsers.AddRange(battleResults.DmUsers);
            allUsers.AddRange(battleResults.RedUsers);
            allUsers.AddRange(battleResults.BlueUsers);
            showedItems = 0;
            SetStatItem(flagsDelivered, mvp, allUsers, (UserResult x) => x.Flags);
            SetStatItem(flagsReturned, mvp, allUsers, (UserResult x) => x.FlagReturns);
            SetStatItem(damage, mvp, allUsers, (UserResult x) => x.Damage);
            SetStatItem(killStreak, mvp, allUsers, (UserResult x) => x.KillStrike);
            SetStatItem(bonuseTaken, mvp, allUsers, (UserResult x) => x.BonusesTaken);
        }

        void SetStatItem(MVPStatElementComponent item, UserResult mvp, List<UserResult> allUsers, UserField field) {
            if (showedItems < MAX_SHOWED_ITEM) {
                item.Count = field(mvp);
                item.SetBest(isBest(mvp, allUsers, field));

                if (item.ShowIfCan()) {
                    showedItems++;
                }
            } else {
                item.Hide();
            }
        }

        bool isBest(UserResult mvp, List<UserResult> allUsers, UserField field) {
            allUsers.Sort((UserResult x, UserResult y) => field(y) - field(x));
            return field(allUsers[0]) == field(mvp) && field(mvp) > 0;
        }

        delegate int UserField(UserResult user);
    }
}