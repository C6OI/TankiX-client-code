using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class CriticalProbabilityPropertySystem : ECSSystem {
        const int MAX_PROBABILITY_CALCULATION_STEPS = 1000;

        [OnEventFire]
        public void UpdateValues(UpdateItemPropertiesEvent e, ItemNode item, [JoinByScreen] ItemUINode itemProperty) {
            StartCriticalProbabilityPropertyComponent startCriticalProbabilityProperty =
                item.startCriticalProbabilityProperty;

            MaxCriticalProbabilityPropertyComponent maxCriticalProbabilityProperty = item.maxCriticalProbabilityProperty;

            CriticalProbabilityDeltaPropertyComponent criticalProbabilityDeltaProperty =
                item.criticalProbabilityDeltaProperty;

            itemProperty.propertyUI.SetValue(CalculateProbability(e.GetValue(startCriticalProbabilityProperty),
                e.GetValue(maxCriticalProbabilityProperty),
                e.GetValue(criticalProbabilityDeltaProperty)));

            itemProperty.propertyUI.SetNextValue(CalculateProbability(e.GetNextValue(startCriticalProbabilityProperty),
                e.GetNextValue(maxCriticalProbabilityProperty),
                e.GetNextValue(criticalProbabilityDeltaProperty)));
        }

        static float CalculateProbability(float startProbability, float maxProbability, float probabilityDelta) {
            startProbability /= 100f;
            maxProbability /= 100f;
            probabilityDelta /= 100f;
            float num = startProbability;
            float num2 = 0f;
            float num3 = 1f;

            for (int i = 1; i < 1000; i++) {
                if (num > maxProbability) {
                    num2 += num3 * (i - 1 + 1f / maxProbability);
                    break;
                }

                if (num > 0f) {
                    num2 += num3 * num * i;
                    num3 *= 1f - num;
                }

                num += probabilityDelta;
            }

            return 100f / num2;
        }

        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ItemNode item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) =>
            screen.component.GetComponentsInChildren<CriticalProbabilityGarageItemPropertyComponent>(true)[0].gameObject
                .SetActive(true);

        public class ItemNode : Node {
            public CriticalProbabilityDeltaPropertyComponent criticalProbabilityDeltaProperty;

            public MaxCriticalProbabilityPropertyComponent maxCriticalProbabilityProperty;

            public StartCriticalProbabilityPropertyComponent startCriticalProbabilityProperty;
        }

        public class ItemUINode : Node {
            public CriticalProbabilityGarageItemPropertyComponent criticalProbabilityGarageItemProperty;

            public PropertyUIComponent propertyUI;
        }
    }
}