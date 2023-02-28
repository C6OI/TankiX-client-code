using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class AlloyToStandardMaterialsMap : MonoBehaviour {
        [SerializeField] List<Material> alloyMaterials = new();

        [SerializeField] List<Material> standardMaterials = new();

        [SerializeField] List<Material> usedMaterials = new();

        public List<Material> UsedMaterials => usedMaterials;

        public List<Material> AlloyMaterials => alloyMaterials;

        public List<Material> StandardMaterials => standardMaterials;

        public bool HasStandardReplacement(Material alloy) => alloyMaterials.Contains(alloy);

        public Material GetStandardReplacement(Material alloy) {
            int index = alloyMaterials.IndexOf(alloy);
            return standardMaterials[index];
        }

        public void AddStandardReplacement(Material alloy, Material standard) {
            alloyMaterials.Add(alloy);
            standardMaterials.Add(standard);
        }
    }
}