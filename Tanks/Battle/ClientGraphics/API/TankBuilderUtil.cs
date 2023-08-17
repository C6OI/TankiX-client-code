using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public static class TankBuilderUtil {
        const string HULL_RENDERER_NAME = "body";

        const string WEAPON_RENDERER_NAME = "weapon";

        public static Renderer GetHullRenderer(GameObject hull) {
            TankVisualRootComponent componentInChildren = hull.GetComponentInChildren<TankVisualRootComponent>();
            GameObject gameObject = componentInChildren.transform.Find("body").gameObject;
            return GraphicsBuilderUtils.GetRenderer(gameObject);
        }

        public static Renderer GetWeaponRenderer(GameObject weapon) {
            WeaponVisualRootComponent componentInChildren = weapon.GetComponentInChildren<WeaponVisualRootComponent>();
            GameObject gameObject = componentInChildren.transform.Find("weapon").gameObject;
            return GraphicsBuilderUtils.GetRenderer(gameObject);
        }
    }
}