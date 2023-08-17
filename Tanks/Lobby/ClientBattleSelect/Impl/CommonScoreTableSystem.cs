using System;
using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class CommonScoreTableSystem : ECSSystem {
        [OnEventFire]
        public void InitScoreTable(NodeAddedEvent e, ScoreTableWithHeaderNode node) {
            node.Entity.AddComponent(new ScoreTableGroupComponent(node.Entity));

            foreach (ScoreTableRowIndicator header in node.scoreTableHeader.headers) {
                node.scoreTable.AddHeader(header);
            }

            LayoutRebuilder.MarkLayoutForRebuild(node.scoreTableHeader.GetComponent<RectTransform>());
        }

        [OnEventFire]
        public void SetIndicators(NodeAddedEvent e, RowNode row, [JoinByScoreTable] ScoreTableNode scoreTable) {
            foreach (ScoreTableRowIndicator indicator in scoreTable.scoreTableUserRowIndicators.indicators) {
                row.scoreTableRow.AddIndicator(indicator);
            }
        }

        [OnEventFire]
        public void ClearScoreTable(NodeRemoveEvent e, ScoreTableNode scoreTable) => scoreTable.scoreTable.Clear();

        [OnEventFire]
        public void RemoveRow(NodeRemoveEvent e, UserNode user, [JoinByUser] [Combine] UserRowNode row,
            [JoinByScoreTable] [Mandatory] ScoreTableNode scoreTable) => scoreTable.scoreTable.RemoveRow(row.scoreTableRow);

        [OnEventFire]
        public void ColorizeRemoteUserRow(NodeAddedEvent e, UserRowNode userRow,
            [JoinByScoreTable] ScoreTableColorNode scoreTableColor, [Context] UserRowNode userRow1,
            [JoinByUser] RemoteUserNode remoteUser) =>
            userRow.scoreTableRow.Color = scoreTableColor.scoreTableRowColor.rowColor;

        [OnEventFire]
        public void ColorizeSelfUserRow(NodeAddedEvent e, UserRowNode userRow,
            [JoinByScoreTable] ScoreTableColorNode scoreTableColor, UserRowNode userRow1,
            [JoinByUser] SelfUserNode selfUser) =>
            userRow.scoreTableRow.Color = scoreTableColor.scoreTableRowColor.selfRowColor;

        [OnEventFire]
        public void SetPosition(SetScoreTablePositionEvent e, Node roundUser,
            [JoinByUser] SingleNode<ScoreTableRowComponent> row) {
            Log.DebugFormat("SetPosition roundUser={0} position={1}", roundUser.Entity.Id, e.Position);
            row.component.Position = e.Position;
        }

        [OnEventFire]
        public void InitPosition(NodeAddedEvent e, RoundUserNode roundUser, [JoinByUser] [Context] UserRowNode userRow) {
            Log.DebugFormat("InitPosition roundUser={0} position={1}",
                roundUser.Entity.Id,
                roundUser.roundUserStatistics.Place);

            userRow.scoreTableRow.HidePosition();
            userRow.scoreTableRow.Position = roundUser.roundUserStatistics.Place;
            userRow.scoreTableRow.SetLayoutDirty();
        }

        public class ScoreTableWithHeaderNode : Node {
            public ScoreTableComponent scoreTable;

            public ScoreTableHeaderComponent scoreTableHeader;
        }

        public class ScoreTableNode : Node {
            public ScoreTableComponent scoreTable;

            public ScoreTableGroupComponent scoreTableGroup;

            public ScoreTableUserRowIndicatorsComponent scoreTableUserRowIndicators;
        }

        public class ScoreTableColorNode : Node {
            public ScoreTableGroupComponent scoreTableGroup;

            public ScoreTableRowColorComponent scoreTableRowColor;
        }

        public class RowNode : Node {
            public ScoreTableGroupComponent scoreTableGroup;
            public ScoreTableRowComponent scoreTableRow;
        }

        public class UserNode : Node {
            public BattleGroupComponent battleGroup;
            public RoundUserComponent roundUser;

            public UserGroupComponent userGroup;
        }

        [Not(typeof(SelfUserComponent))]
        public class RemoteUserNode : Node {
            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;
            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        public class UserRowNode : Node {
            public ScoreTableGroupComponent scoreTableGroup;
            public ScoreTableRowComponent scoreTableRow;

            public UserGroupComponent userGroup;
        }

        public class RoundUserNode : Node, IComparable<RoundUserNode> {
            public BattleGroupComponent battleGroup;
            public RoundUserStatisticsComponent roundUserStatistics;

            public UserGroupComponent userGroup;

            public int CompareTo(RoundUserNode other) => roundUserStatistics.CompareTo(other.roundUserStatistics);
        }
    }
}