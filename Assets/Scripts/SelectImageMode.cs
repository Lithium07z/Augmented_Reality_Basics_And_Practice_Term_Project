using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectImageMode : MonoBehaviour
{
    [SerializeField] AddPictureMode addPicture;
    [SerializeField] EditPicture editPicture;

    public bool isReplacing = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnEnable()
    {
        UIController.ShowUI("SelectImage");    
    }

    public void ImageSelected(ImageInfo image)
    {
        if (isReplacing)
        {
            editPicture.currentPicture.SetImage(image);
            InteractionController.EnableMode("EditPicture");
        }
        else
        {
            addPicture.imageInfo = image;
            InteractionController.EnableMode("AddPicture");
        }
    }
}
