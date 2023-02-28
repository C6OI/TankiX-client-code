using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ModulesScreenTextComponent : Component {
        public string CraftableLabelText { get; set; }

        public string NotCraftableLabelText { get; set; }

        public string AssembleDialogConfirmButtonText { get; set; }

        public string AssembleDialogHeaderText { get; set; }

        public string SlotNameText { get; set; }

        public string MountLabelText { get; set; }

        public string ExceptionalLabelText { get; set; }

        public string EpicLabelText { get; set; }

        public string AdditionalEffectLabelText { get; set; }

        public string ResistLongLabelText { get; set; }

        public string DamageLongLabelText { get; set; }

        public string AdditionalEffectDefaultText { get; set; }

        public string SlotLockedText { get; set; }

        public string PleaseChooseModuleText { get; set; }

        public string ResistShortLabel { get; set; }

        public string DamageShortLabel { get; set; }

        public List<string> ModuleTypeDescriptions { get; set; }

        public string OkText { get; set; }

        public string PleaseChooseSlotText { get; set; }
    }
}