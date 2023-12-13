using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageScanMode : MonoBehaviour
{
    [SerializeField] ARTrackedImageManager imageManager;

    void Start()
    {
        
    }

    void Update()
    {
        if (imageManager.trackables.count > 0)
        {
            InteractionController.EnableMode("Main");
        }
    }

    private void OnEnable()
    {
        UIController.ShowUI("Scan");
    }
}
