using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GalleryMainMode : MonoBehaviour
{
    [SerializeField] SelectImageMode selectImage;
    [SerializeField] EditPicture editMode;
    Camera camera;

    void Start()
    {
        camera = Camera.main;   
    }

    void OnEnable()
    {
        UIController.ShowUI("Main");
    }

    public void OnSelectObject(InputValue value)
    {
        Vector2 touchPosition = value.Get<Vector2>();
        FindObjectToEdit(touchPosition);
    }

    void FindObjectToEdit(Vector2 touchPosition)
    {
        Ray ray = camera.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        int layerMask = 1 << LayerMask.NameToLayer("PlacedObjects");
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            FramedPhoto picture = hit.collider.GetComponentInParent<FramedPhoto>();
            editMode.currentPicture = picture;
            InteractionController.EnableMode("EditPicture");
        }
    }

    public void SelectImageToAdd()
    {
        selectImage.isReplacing = false;
        InteractionController.EnableMode("AddPicture");
    }
}
