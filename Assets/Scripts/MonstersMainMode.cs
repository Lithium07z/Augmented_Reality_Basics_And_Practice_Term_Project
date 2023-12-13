using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using RotaryHeart.Lib.SerializableDictionary;
using TMPro;
using System.Threading;
using UnityEngine.XR.ARSubsystems;

[System.Serializable]
public class MonsterPrefabDictionary : SerializableDictionaryBase<string, GameObject> {}
public class CaughtMonsterDictionary : SerializableDictionaryBase<string, GameObject> { }

public class MonstersMainMode : MonoBehaviour
{
    [SerializeField] MonsterPrefabDictionary monsterPrefabs;
    [SerializeField] ARTrackedImageManager imageManager;
    [SerializeField] TMP_Text monsterName;
    [SerializeField] TMP_Text detailsText;
    public Toggle infoButton;
    public Toggle battleButton;

    public GameObject detailsPanel;
    private GameObject player;

    private Monster monster;

    Camera camera;
    int layerMask;

    void Start()
    {
        camera = Camera.main;
        player = GameObject.Find("Player");
        layerMask = 1 << LayerMask.NameToLayer("PlacedObjects");
        StartCoroutine(TurnOnImageTracker());
    }

    
    void Update()
    {
        if (imageManager.trackables.count == 0)
        {
            InteractionController.EnableMode("Scan");
        }
        else
        {
            Ray ray = new Ray(camera.transform.position, camera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                monster = hit.collider.GetComponentInParent<Monster>();
                monsterName.text = monster.monsterName;
                detailsText.text = monster.description;
                infoButton.interactable = true;
                battleButton.interactable = true;
            }
            else
            {
                monsterName.text = "";
                detailsText.text = "";
                infoButton.interactable = false;
                battleButton.interactable = false;
            }
        }
    }

    private void OnEnable()
    {
        UIController.ShowUI("Main");
        monsterName.text = "";
        infoButton.interactable = false;
        detailsPanel.SetActive(false);

        foreach (ARTrackedImage image in imageManager.trackables)
        {
            InstantiateMonster(image);
        }
        imageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void InstantiateMonster(ARTrackedImage image)
    {
        string name = image.referenceImage.name.Split('-')[0];
        if (image.transform.childCount == 0)
        {
            Debug.Log("------------------------------------Instantiate------------------------------------");
            GameObject monster = Instantiate(monsterPrefabs[name]);
            monster.transform.SetParent(image.transform, false);
        }
        else
        {
            Debug.Log("---------------------------------${name} already instantiated---------------------------------");
        }
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage newImage in eventArgs.added)
        {
            //InstantiateMonster(newImage);

            Vector2 myPos = new Vector2(GPS_Manager.instance.latitude, GPS_Manager.instance.longitude);

            StartCoroutine(DB_Manager.instance.LoadData(myPos, newImage.transform));
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            if (trackedImage.transform.childCount > 0)
            {
                trackedImage.transform.GetChild(0).position = trackedImage.transform.position;
                trackedImage.transform.GetChild(0).rotation = trackedImage.transform.rotation;
            }
        }
    }

    public void BattleStart()
    {
        Debug.Log("------------------------BattleStart------------------------");
        Debug.Log(monster.name.ToString());
        monster.BattleStart();
        Debug.Log("------------------------BattleEnd------------------------");
        player.GetComponent<Player>().shootingFlag = true;
        infoButton.interactable = false;
        battleButton.interactable = false;
    }

    IEnumerator TurnOnImageTracker()
    {
        imageManager.enabled = false;

        while (!GPS_Manager.instance.receiveGPS)
        {
            yield return null;
        }

        imageManager.enabled = true;

        imageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }
}