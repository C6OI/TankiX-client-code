using System.ComponentModel;

namespace Tanks.Battle.ClientCore.Impl {
    public enum UnityInputAxes {
        [Description("Horizontal")] Horizontal = 0,
        [Description("Vertical")] Vertical = 1,
        [Description("Mouse X")] MouseX = 2,
        [Description("Mouse Y")] MouseY = 3,
        [Description("Mouse ScrollWheel")] MouseScrollWheel = 4,
        [Description("Joy1 Axis 2")] Joy1Axis2 = 5,
        [Description("Joy1 Axis 1")] Joy1Axis1 = 6,
        [Description("Joy1 Axis 4")] Joy1Axis4 = 7,
        [Description("Joy1 Axis 5")] Joy1Axis5 = 8,
        [Description("Joy1 Axis 6")] Joy1Axis6 = 9,
        [Description("Joy1 Axis 7")] Joy1Axis7 = 10,
        [Description("Joy1 Axis 8")] Joy1Axis8 = 11,
        [Description("Joy1 Axis 9")] Joy1Axis9 = 12,
        [Description("Joy1 Axis 10")] Joy1Axis10 = 13,
        [Description("Joy1 Axis 11")] Joy1Axis11 = 14
    }
}