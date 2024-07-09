using MalbersAnimations.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntitiesManager : MonoBehaviour
{
    public static EntitiesManager Instance;
    void Awake()
    {
        Instance = this;
    }
    Dictionary<MSimpleTransformer, float> allSimpleTransformersOriginalDuration = new Dictionary<MSimpleTransformer, float>();
    private void Start()
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("EndGame");

        var allSimpleTransformer = Resources.FindObjectsOfTypeAll<MSimpleTransformer>();
        foreach (var item in allSimpleTransformer)
        {
            allSimpleTransformersOriginalDuration.Add(item, item.duration.Value);
        }
    }
    public void SlowDownAllTransformers(float multiplier)
    {
        foreach (var transformer in allSimpleTransformersOriginalDuration)
        {
            transformer.Key.duration.Value = transformer.Key.duration.Value * multiplier;
        }
    }
    public void UndoSlowDown()
    {
        foreach (var transformer in allSimpleTransformersOriginalDuration)
        {
            transformer.Key.duration.Value = transformer.Value;
        }
    }

    public bool IsEnemyFreeze = false;
    public void FreezeEnemyFuntions()
    {
        IsEnemyFreeze = true;
    }
    public void UnFreezeEnemyFunctions()
    {
        IsEnemyFreeze = false;
    }
}