using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CNB
{
    /// <summary>
    /// These 3 materials define most of the look of the generated map. Saving this looks in scriptable objects makes it easy to create and backUp new map skins.
    /// Estos 3 materiales definen la mayor parte del aspecto del mapa generado. Guardar este aspecto en objetos programables facilita la creación y copia de seguridad de nuevas máscaras de mapas.
    /// </summary>
    [CreateAssetMenu(fileName = "NewMapMat", menuName = "MapMatSO")]
    public class MapMatSO : ScriptableObject
    {
        public Material _meshMaterial;
        public Material _backgroundMaterial;
        public Material _lineRendererMaterial;
    }
}