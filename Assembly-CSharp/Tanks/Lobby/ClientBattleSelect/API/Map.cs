using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientLoading.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class Map {
        readonly Entity mapEntity;

        Sprite loadPreview;

        public Map(Entity mapEntity) => this.mapEntity = mapEntity;

        public Sprite LoadPreview {
            get {
                if (loadPreview != null) {
                    return loadPreview;
                }

                if (mapEntity.HasComponent<MapLoadPreviewDataComponent>()) {
                    MapLoadPreviewDataComponent component = mapEntity.GetComponent<MapLoadPreviewDataComponent>();
                    Texture2D texture2D = (Texture2D)component.Data;
                    loadPreview = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
                    return loadPreview;
                }

                return null;
            }
        }

        public string Name {
            get {
                if (!mapEntity.HasComponent<DescriptionItemComponent>()) {
                    return string.Empty;
                }

                return mapEntity.GetComponent<DescriptionItemComponent>().Name;
            }
        }

        public List<string> FlavorTextList {
            get {
                if (!mapEntity.HasComponent<FlavorListComponent>()) {
                    List<string> list = new();
                    list.Add(string.Empty);
                    return list;
                }

                return mapEntity.GetComponent<FlavorListComponent>().Collection;
            }
        }
    }
}