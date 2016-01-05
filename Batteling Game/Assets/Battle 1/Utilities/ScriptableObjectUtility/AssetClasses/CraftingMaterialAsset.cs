using UnityEngine;
using UnityEditor;

public class CraftingMaterialAsset
{
    [MenuItem("Assets/Create/CraftingMaterial")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<CraftingMaterial>();
    }
}