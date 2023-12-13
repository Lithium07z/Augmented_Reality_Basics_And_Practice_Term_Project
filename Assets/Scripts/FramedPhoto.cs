using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramedPhoto : MonoBehaviour
{
    [SerializeField] Transform scalarObject;
    [SerializeField] GameObject imageObject;
    [SerializeField] GameObject highlightObject;
    [SerializeField] Collider boundingCollider;
    
    int layer;
    bool isEditing;
    ImageInfo imageInfo;
    MovePicture movePicture;
    ReSizePicture resizePicture;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void Awake()
    {
        movePicture = GetComponent<MovePicture>();
        resizePicture = GetComponent<ReSizePicture>();
        movePicture.enabled = false;
        resizePicture.enabled = false;
        layer = LayerMask.NameToLayer("PlacedObjects");
        Highlight(false);    
    }

    void OnTriggerStay(Collider other)
    {
        const float spacing = 0.1f;
        
        if (isEditing && other.gameObject.layer == layer)
        {
            Bounds bounds = boundingCollider.bounds;
            if (other.bounds.Intersects(bounds))
            {
                Vector3 centerDistane = bounds.center - other.bounds.center;
                Vector3 distOnPlane = Vector3.ProjectOnPlane(centerDistane, transform.forward);
                Vector3 direction = distOnPlane.normalized;
                float distanceToMoveThisFrame = bounds.size.x * spacing;
                transform.Translate(direction * distanceToMoveThisFrame);
            }
        }
    }

    public void SetImage(ImageInfo image)
    {
        imageInfo = image;

        Renderer renderer = imageObject.GetComponent<Renderer>();
        Material material = renderer.material;
        material.SetTexture("_BaseMap", imageInfo.texture);
        AdjustScale();
    }

    public void AdjustScale()
    {
        Vector2 scale = ImagesData.AspectRatio(imageInfo.width, imageInfo.height);
        scalarObject.localScale = new Vector3(scale.x, scale.y, 1f);
    }

    public void Highlight(bool show)
    {
        if (highlightObject)
        {
            highlightObject.SetActive(show);
        }
    }

    public void BeginEdited(bool editing)
    {
        Highlight(editing);
        isEditing = editing;
        movePicture.enabled = editing;
        resizePicture.enabled = editing;
    }
}
