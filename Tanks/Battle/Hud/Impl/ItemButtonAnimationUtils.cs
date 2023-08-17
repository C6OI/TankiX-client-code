using UnityEngine;

namespace Tanks.Battle.Hud.Impl {
    public static class ItemButtonAnimationUtils {
        const string ENABLE = "Enable";

        const string DISABLE = "Disable";

        const string ACTIVATE = "Activate";

        const string COOLDOWN = "Cooldown";

        const string COOLDOWN_MULTIPLIER = "CooldownMultiplier";

        const string ACTIVE_MULTIPLIER = "ActiveMultiplier";

        const string MINE_ENABLE = "MineEnable";

        const string FROM_INVENTORY = "FromInventory";

        const string TANK_INACTIVE = "TankInactive";

        public static void ActivateMineButtonFromInventory(Animator animator) {
            animator.SetTrigger("FromInventory");
            animator.SetTrigger("MineEnable");
        }

        public static void ActivateItemButtonFromInventory(Animator animator) => animator.SetTrigger("FromInventory");

        public static void EnableItemButton(Animator animator) =>
            SetItemButtonAnimationTrigger("Enable", animator, "Disable", "Activate", "Cooldown");

        public static void DisableItemButton(Animator animator) =>
            SetItemButtonAnimationTrigger("Disable", animator, "Enable", "Activate", "Cooldown");

        public static void SwitchItemButtonToActiveState(Animator animator, float activeTime) {
            SetItemButtonAnimationMultiplier(animator, activeTime, "ActiveMultiplier");
            SetItemButtonAnimationTrigger("Activate", animator, "Enable", "Disable", "Cooldown");
        }

        public static void SwitchItemButtonToCooldownState(Animator animator, float cooldownTime) {
            SetItemButtonAnimationMultiplier(animator, cooldownTime, "CooldownMultiplier");
            SetItemButtonAnimationTrigger("Cooldown", animator, "Enable", "Disable", "Activate");
        }

        public static void ShowQuantityOnItemButton(Animator animator) =>
            SetItemButtonAnimationFlag(animator, "TankInactive", true);

        public static void HideQuantityOnItemButton(Animator animator) =>
            SetItemButtonAnimationFlag(animator, "TankInactive", false);

        static void SetItemButtonAnimationMultiplier(Animator animator, float animationTime, string multiplierName) {
            float value = 1f / Mathf.Max(Mathf.Epsilon, animationTime);
            animator.SetFloat(multiplierName, value);
        }

        static void SetItemButtonAnimationFlag(Animator animator, string flagName, bool value) =>
            animator.SetBool(flagName, value);

        static void SetItemButtonAnimationTrigger(string triggerToSet, Animator animator, params string[] triggersToReset) {
            int num = triggersToReset.Length;

            for (int i = 0; i < num; i++) {
                animator.ResetTrigger(triggersToReset[i]);
            }

            animator.SetTrigger(triggerToSet);
        }
    }
}