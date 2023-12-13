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
        // DB의 URL을 설정한다.
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri(databaseURL);

        // 데이터를 DB에 저장한다.
        SaveData();
    }


    void Update()
    {

    }

    void SaveData()
    {
        Debug.Log("----------------------------Save Data----------------------------");
        // 저장용 클래스 변수를 생성한다.
        ImageGPSData data1 = new ImageGPSData("BombMonster", 37.88622f, 127.7352f, false);
        ImageGPSData data2 = new ImageGPSData("Bat", 37.88622f, 127.7352f, false);
        ImageGPSData data3 = new ImageGPSData("Muumy", 37.88622f, 127.7352f, false);
        ImageGPSData data4 = new ImageGPSData("Anger", 37.88622f, 127.7352f, false);
        ImageGPSData data5 = new ImageGPSData("Chick", 37.88622f, 127.7352f, false);
        ImageGPSData data6 = new ImageGPSData("Chicken", 37.88622f, 127.7352f, false);
        ImageGPSData data7 = new ImageGPSData("Mole", 37.88622f, 127.7352f, false);
        ImageGPSData data8 = new ImageGPSData("Hornet", 37.88622f, 127.7352f, false);

        // 클래스 데이터를 json 문서 형식으로 변환한다.
        string jsonBombMonster = JsonUtility.ToJson(data1);
        string jsonBat = JsonUtility.ToJson(data2);
        string jsonMummy = JsonUtility.ToJson(data3);
        string jsonAnger = JsonUtility.ToJson(data4);
        string jsonChick = JsonUtility.ToJson(data5);
        string jsonChicken = JsonUtility.ToJson(data6);
        string jsonMole = JsonUtility.ToJson(data7);
        string jsonHornet = JsonUtility.ToJson(data8);

        // DB의 최상단(Root) 디렉토리를 찾는다.
        DatabaseReference refData = FirebaseDatabase.GetInstance(Firebase.FirebaseApp.DefaultInstance, databaseURL).RootReference;

        // 최상단 디렉토리를 기준으로 하위에 Marker라는 디렉토리를 만들고
        // Marker의 하위 디렉토리로 json 문서 데이터를 넣는다.
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

    // 데이터베이스로부터 데이터를 읽어오는 함수 
    public IEnumerator LoadData(Vector2 myPos, Transform trackedImage)
    {
        // 현재 나의 위치를 저장한다.
        currentPos = myPos;

        // 데이터를 읽기 위한 기준이 되는 노드를 찾는다.
        DatabaseReference refData = FirebaseDatabase.GetInstance(Firebase.FirebaseApp.DefaultInstance, databaseURL).GetReference("Markers");

        // DB에서 데이터를 받아오기
        isSearch = true;
        refData.GetValueAsync().ContinueWith(LoadFunc);

        // 데이터를 받아오는 동안에는 이 함수를 잠시 대기한다.
        while (isSearch)
        {
            yield return null;
        }

        // Resources 폴더에서 이미지 이름과 동일한 이름의 프리팹을 찾는다.
        GameObject imagePrefab = Resources.Load<GameObject>(objectName);

        if (imagePrefab != null)
        {
            // 만일 인식한 이미지에 등록된 자식 오브젝트가 없다면...
            if (trackedImage.transform.childCount < 1)
            {
                // 이미지의 위치에 검색된 프리팹을 생성한다.
                GameObject go = Instantiate(imagePrefab, trackedImage.transform.position, trackedImage.transform.rotation);

                // 생성된 오브젝트를 이미지의 자식 오브젝트로 등록한다.
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
            // DB 데이터를 가져온다.
            DataSnapshot snapshot = task.Result;

            // 가져온 DB 데이터를 순회한다.
            foreach (DataSnapshot data in snapshot.Children)
            {
                // DB 스냅샷 데이터를 Json 데이터로 변환한다.
                string myData = data.GetRawJsonValue();

                // Json 데이터를 다시 ImageGPSData로 변환한다.
                ImageGPSData myClassData = JsonUtility.FromJson<ImageGPSData>(myData);

                // 만일, 누군가 다른 사용자에게 포획되지 않았다면...
                if (!myClassData.isCaptured)
                {
                    // DB에 저장된 위치를 Vector2 형태로 저장한다.
                    Vector2 dataPos = new Vector2(myClassData.latitude, myClassData.longitude);

                    // DB의 위치 데이터와 나의 현재 위치 데이터를 비교하여 거리를 구한다.
                    float distance = Vector2.Distance(currentPos, dataPos);

                    if (distance < 0.001f)
                    {
                        // 생성할 프리팹 이름과 DB 키의 값을 저장한다.
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
        // 데이터를 변경할 노드의 위치를 결정한다.
        string dataPath = "Markers/" + currentKey + "/isCaptured";

        // 데이터 경로를 기반으로 DM의 노드 위치를 찾는다.
        DatabaseReference refData = FirebaseDatabase.GetInstance(Firebase.FirebaseApp.DefaultInstance, databaseURL).GetReference(dataPath);

        // 찾은 경로의 값을 true로 변경한다.
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