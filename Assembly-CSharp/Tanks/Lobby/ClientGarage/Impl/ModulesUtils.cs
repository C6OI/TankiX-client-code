using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;
using Random = System.Random;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ModulesUtils {
        public static Color StringToColor(string s) {
            Random random = new(s.GetHashCode());
            return new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
        }

        public static bool EarlyIsUserItem(Entity item) => typeof(UserItemTemplate).IsAssignableFrom(((EntityImpl)item).TemplateAccessor.Get().TemplateDescription.TemplateClass);
    }
}