using UnityEngine;
using UnityEditor;

public class FoodAsset
{
    [MenuItem("Assets/Create/Food")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<FoodItem>();
    }
}