using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ImageButtons : MonoBehaviour
{
    [SerializeField] GameObject imageButtonPrefab;
    [SerializeField] ImagesData imagesData;
    [SerializeField] AddPictureMode addPicture;
    [SerializeField] SelectImageMode selectImage; 

    void Start()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        foreach (ImageInfo image in imagesData.images)
        {
            GameObject obj = Instantiate(imageButtonPrefab, transform);
            RawImage rawimage = obj.GetComponent<RawImage>();
            rawimage.texture = image.texture;
            Button button = obj.GetComponent<Button>();
            button.onClick.AddListener(() => OnClick(image));
        }
    }

    void Update()
    {
        
    }

    void OnClick(ImageInfo image)
    {
        //addPicture.imageInfo = image;
        //InteractionController.EnableMode("AddPicture");
        selectImage.ImageSelected(image);
    }
}
