using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Unity;
using Firebase;
using Firebase.Database;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.Analytics;
using static UnityEngine.XR.ARSubsystems.XRCpuImage;

public class DB_Manager : MonoBehaviour
{
    public static DB_Manager instance;
    public string databaseURL = "https://ar-termproject-c2798-default-rtdb.asia-southeast1.firebasedatabase.app/";
    public FirebaseApp app;

    Vector2 currentPos;
    bool isSearch = false;
    string objectName = "";
    string currentKey = "";

    private void Awake()
    {
        
        databaseURL = "https://ar-termproject-c2798-default-rtdb.asia-southeast1.firebasedatabase.app/";
        Debug.Log("---------------------------DB_Manager Start---------------------------");

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                Debug.Log("0-----------------------------------------------------------------------");
                app = Firebase.FirebaseApp.DefaultInstance;
                Debug.Log("1-----------------------------------------------------------------------");
                FirebaseApp.Create();
                Debug.Log("2-----------------------------------------------------------------------");
                FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri(databaseURL);
                Debug.Log("3-----------------------------------------------------------------------");
                SaveData();
                Debug.Log("4-----------------------------------------------------------------------");
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                Debug.Log("5-----------------------------------------------------------------------");
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
        Debug.Log("---------------------------DB_Manager End---------------------------");
        
    }

    void Start()
    {
        // DB�� URL�� �����Ѵ�.
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri(databaseURL);

        // �����͸� DB�� �����Ѵ�.
        SaveData();
    }


    void Update()
    {

    }

    void SaveData()
    {
        Debug.Log("----------------------------Save Data----------------------------");
        // ����� Ŭ���� ������ �����Ѵ�.
        ImageGPSData data1 = new ImageGPSData("BombMonster", 37.88622f, 127.7352f, false);
        ImageGPSData data2 = new ImageGPSData("Bat", 37.88622f, 127.7352f, false);
        ImageGPSData data3 = new ImageGPSData("Muumy", 37.88622f, 127.7352f, false);
        ImageGPSData data4 = new ImageGPSData("Anger", 37.88622f, 127.7352f, false);
        ImageGPSData data5 = new ImageGPSData("Chick", 37.88622f, 127.7352f, false);
        ImageGPSData data6 = new ImageGPSData("Chicken", 37.88622f, 127.7352f, false);
        ImageGPSData data7 = new ImageGPSData("Mole", 37.88622f, 127.7352f, false);
        ImageGPSData data8 = new ImageGPSData("Hornet", 37.88622f, 127.7352f, false);

        // Ŭ���� �����͸� json ���� �������� ��ȯ�Ѵ�.
        string jsonBombMonster = JsonUtility.ToJson(data1);
        string jsonBat = JsonUtility.ToJson(data2);
        string jsonMummy = JsonUtility.ToJson(data3);
        string jsonAnger = JsonUtility.ToJson(data4);
        string jsonChick = JsonUtility.ToJson(data5);
        string jsonChicken = JsonUtility.ToJson(data6);
        string jsonMole = JsonUtility.ToJson(data7);
        string jsonHornet = JsonUtility.ToJson(data8);

        // DB�� �ֻ��(Root) ���丮�� ã�´�.
        DatabaseReference refData = FirebaseDatabase.GetInstance(Firebase.FirebaseApp.DefaultInstance, databaseURL).RootReference;

        // �ֻ�� ���丮�� �������� ������ Marker��� ���丮�� �����
        // Marker�� ���� ���丮�� json ���� �����͸� �ִ´�.
        refData.Child("Markers").Child("Data1").SetRawJsonValueAsync(jsonBombMonster);
        refData.Child("Markers").Child("Data2").SetRawJsonValueAsync(jsonBat);
        refData.Child("Markers").Child("Data3").SetRawJsonValueAsync(jsonMummy);
        refData.Child("Markers").Child("Data4").SetRawJsonValueAsync(jsonAnger);
        refData.Child("Markers").Child("Data5").SetRawJsonValueAsync(jsonChick);
        refData.Child("Markers").Child("Data6").SetRawJsonValueAsync(jsonChicken);
        refData.Child("Markers").Child("Data7").SetRawJsonValueAsync(jsonMole);
        refData.Child("Markers").Child("Data8").SetRawJsonValueAsync(jsonHornet);

        Debug.Log("[MarkerAR] Data Saved!");
    }

    // �����ͺ��̽��κ��� �����͸� �о���� �Լ� 
    public IEnumerator LoadData(Vector2 myPos, Transform trackedImage)
    {
        // ���� ���� ��ġ�� �����Ѵ�.
        currentPos = myPos;

        // �����͸� �б� ���� ������ �Ǵ� ��带 ã�´�.
        DatabaseReference refData = FirebaseDatabase.GetInstance(Firebase.FirebaseApp.DefaultInstance, databaseURL).GetReference("Markers");

        // DB���� �����͸� �޾ƿ���
        isSearch = true;
        refData.GetValueAsync().ContinueWith(LoadFunc);

        // �����͸� �޾ƿ��� ���ȿ��� �� �Լ��� ��� ����Ѵ�.
        while (isSearch)
        {
            yield return null;
        }

        // Resources �������� �̹��� �̸��� ������ �̸��� �������� ã�´�.
        GameObject imagePrefab = Resources.Load<GameObject>(objectName);

        if (imagePrefab != null)
        {
            // ���� �ν��� �̹����� ��ϵ� �ڽ� ������Ʈ�� ���ٸ�...
            if (trackedImage.transform.childCount < 1)
            {
                // �̹����� ��ġ�� �˻��� �������� �����Ѵ�.
                GameObject go = Instantiate(imagePrefab, trackedImage.transform.position, trackedImage.transform.rotation);

                // ������ ������Ʈ�� �̹����� �ڽ� ������Ʈ�� ����Ѵ�.
                go.transform.SetParent(trackedImage.transform);
            }
        }
    }

    void LoadFunc(Task<DataSnapshot> task)
    {
        if (task.IsFaulted)
        {
            Debug.LogError("[MarkerAR] Failed to Load Data.");
        }
        else if (task.IsCanceled)
        {
            Debug.Log("[MarkerAR] Canceled to Load Data.");
        }
        else if (task.IsCompleted)
        {
            // DB �����͸� �����´�.
            DataSnapshot snapshot = task.Result;

            // ������ DB �����͸� ��ȸ�Ѵ�.
            foreach (DataSnapshot data in snapshot.Children)
            {
                // DB ������ �����͸� Json �����ͷ� ��ȯ�Ѵ�.
                string myData = data.GetRawJsonValue();

                // Json �����͸� �ٽ� ImageGPSData�� ��ȯ�Ѵ�.
                ImageGPSData myClassData = JsonUtility.FromJson<ImageGPSData>(myData);

                // ����, ������ �ٸ� ����ڿ��� ��ȹ���� �ʾҴٸ�...
                if (!myClassData.isCaptured)
                {
                    // DB�� ����� ��ġ�� Vector2 ���·� �����Ѵ�.
                    Vector2 dataPos = new Vector2(myClassData.latitude, myClassData.longitude);

                    // DB�� ��ġ �����Ϳ� ���� ���� ��ġ �����͸� ���Ͽ� �Ÿ��� ���Ѵ�.
                    float distance = Vector2.Distance(currentPos, dataPos);

                    if (distance < 0.001f)
                    {
                        // ������ ������ �̸��� DB Ű�� ���� �����Ѵ�.
                        objectName = myClassData.name;
                        currentKey = data.Key;
                    }
                }
            }
        }
        isSearch = false;
    }

    public void UpdateCaptured()
    {
        // �����͸� ������ ����� ��ġ�� �����Ѵ�.
        string dataPath = "Markers/" + currentKey + "/isCaptured";

        // ������ ��θ� ������� DM�� ��� ��ġ�� ã�´�.
        DatabaseReference refData = FirebaseDatabase.GetInstance(Firebase.FirebaseApp.DefaultInstance, databaseURL).GetReference(dataPath);

        // ã�� ����� ���� true�� �����Ѵ�.
        if (refData != null)
        {
            refData.SetValueAsync(true);
        }
    }
}

public class ImageGPSData
{
    public string name;
    public float latitude = 0;
    public float longitude = 0;
    public bool isCaptured = false;

    public ImageGPSData(string objName, float lat, float lon, bool captured)
    {
        name = objName;
        latitude = lat;
        longitude = lon;
        isCaptured = captured;
    }
}