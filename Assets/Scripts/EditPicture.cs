using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EditPicture : MonoBehaviour
{
    [SerializeField] SelectImageMode selectImage;
    public FramedPhoto currentPicture;
    Camera camera;

    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        
    }

    void OnEnable()
    {
        UIController.ShowUI("EditPicture");
        
        if (currentPicture)
        {
            currentPicture.BeginEdited(true);
        }
    }

    void OnDisable()
    {
        if (currentPicture)
        {
            currentPicture.BeginEdited(false);
        }    
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
        if (Physics.Raycast(ray, out hit, 50f, layerMask))
        {
            if (hit.collider.gameObject != currentPicture.gameObject)
            {
                currentPicture.BeginEdited(false);
                FramedPhoto picture = hit.collider.GetComponentInParent<FramedPhoto>();
                currentPicture = picture;
                picture.BeginEdited(true);
            }
        }
    }

    public void DeletePicture()
    {
        GameObject.Destroy(currentPicture.gameObject);
        InteractionController.EnableMode("Main");
    }

    public void SelectImageToReplace()
    {
        selectImage.isReplacing = true;
        InteractionController.EnableMode("SelectImage");
    }
}
