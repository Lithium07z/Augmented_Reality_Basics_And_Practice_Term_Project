using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;

public class GPS_Manager : MonoBehaviour
{
    public static GPS_Manager instance;
    public TMP_Text latitude_text;
    public TMP_Text longitude_text;

    public float latitude = 0;
    public float longitude = 0;
    public float maxWaitTime = 10.0f;

    public bool receiveGPS = false;
    public float resendTime = 1.0f;

    float waitTime = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        StartCoroutine(GPS_On());
    }

    void Update()
    {

    }

    IEnumerator GPS_On()
    {
        // 만일, 위치 정보 수신에 대해 사용자의 허가를 받지 못했다면..
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            // 허가를 요청하는 팝업을 띄운다.
            Permission.RequestUserPermission(Permission.FineLocation);

            // 동의 받았는지 확인될 때까지 잠시 대기한다.
            while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                yield return null;
            }
        }

        // 만일, 사용자의 GPS 장치가 켜져있지 않다면, 함수를 종료한다.
        if (!Input.location.isEnabledByUser)
        {
            latitude_text.text = "GPS off";
            longitude_text.text = "GPS off";
            yield break;
        }

        // 위치 데이터를 요청한다 -> 수신대기
        Input.location.Start();

        // GPS 수신 상태가 초기 상태에서 일정 시간 동안 대기한다.
        while (Input.location.status == LocationServiceStatus.Initializing && waitTime < maxWaitTime)
        {
            yield return new WaitForSeconds(1.0f);
            waitTime++;
        }

        // 위치 정보 수신에 실패했음을 출력한다.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            latitude_text.text = "위치 정보 수신 실패!";
            longitude_text.text = "위치 정보 수신 실패!";
        }

        // 응답대기 시간이 초과되었음을 출력한다.
        if (waitTime >= maxWaitTime)
        {
            latitude_text.text = "응답 대기 시간 초과";
            longitude_text.text = "응답 대기 시간 초과";
        }

        // 위치 정보 수신 시작 체크
        receiveGPS = true;

        // 위치 데이터 수신 시작 이후 resendTime 경과마다 위치 정보를 갱신하고 출력한다.
        while (receiveGPS)
        {
            yield return new WaitForSeconds(resendTime);

            // 수신된 위치 정보 데이터를 UI에 출력한다.
            LocationInfo li = Input.location.lastData;
            latitude = li.latitude;
            longitude = li.longitude;

            latitude_text.text = "Latitude: " + latitude.ToString();
            longitude_text.text = "Longitude: " + longitude.ToString();
        }
    }
}
