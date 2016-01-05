using UnityEngine;
using UnityEditor;

public class EquipmentBlueprintAsset
{
    [MenuItem("Assets/Create/EquipmentBlueprint")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<EquipmentBlueprint>();
    }
}