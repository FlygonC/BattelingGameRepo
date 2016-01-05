using UnityEngine;
using UnityEditor;

public class WeaponBlueprintAsset
{
    [MenuItem("Assets/Create/WeaponBlueprint")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<WeaponBlueprint>();
    }
}