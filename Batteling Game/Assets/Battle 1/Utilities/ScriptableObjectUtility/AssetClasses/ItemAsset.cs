using UnityEngine;
using UnityEditor;

public class ItemAsset
{
    [MenuItem("Assets/Create/Item")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<Item>();
    }
}