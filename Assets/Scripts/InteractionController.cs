using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using System;

[System.Serializable]
public class InteractionModeDictionary :
    SerializableDictionaryBase<string, GameObject> { }

public class InteractionController : Singleton<InteractionController>
{
    [SerializeField] InteractionModeDictionary interactionModes;

    GameObject currentMode;

    void Awake()
    {
        base.Awake();
        ResetAllModes();
    }

    void ResetAllModes()
    {
        foreach (GameObject mode in interactionModes.Values)
        {
            mode.SetActive(false);
        }
    }

    public static void EnableMode(string name)
    {
        Instance?._EnableMode(name);
    }

    void _EnableMode(string name)
    {
        GameObject modeObject;
        if (interactionModes.TryGetValue(name, out modeObject))
        {
            StartCoroutine(ChangeMode(modeObject));
        }
        else
        {
            Debug.LogError("undefined mode named " + name);
        }
    }

    IEnumerator ChangeMode(GameObject mode)
    {
        if (mode == currentMode)
        {
            yield break;
        }

        if (currentMode)
        {
            currentMode.SetActive(false);
            yield return null;
        }

        currentMode = mode;
        mode.SetActive(true);
    }

    void Start()
    {
        _EnableMode("Startup");
    }
}
