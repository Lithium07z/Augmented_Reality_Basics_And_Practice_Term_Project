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
        // ����, ��ġ ���� ���ſ� ���� ������� �㰡�� ���� ���ߴٸ�..
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            // �㰡�� ��û�ϴ� �˾��� ����.
            Permission.RequestUserPermission(Permission.FineLocation);

            // ���� �޾Ҵ��� Ȯ�ε� ������ ��� ����Ѵ�.
            while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                yield return null;
            }
        }

        // ����, ������� GPS ��ġ�� �������� �ʴٸ�, �Լ��� �����Ѵ�.
        if (!Input.location.isEnabledByUser)
        {
            latitude_text.text = "GPS off";
            longitude_text.text = "GPS off";
            yield break;
        }

        // ��ġ �����͸� ��û�Ѵ� -> ���Ŵ��
        Input.location.Start();

        // GPS ���� ���°� �ʱ� ���¿��� ���� �ð� ���� ����Ѵ�.
        while (Input.location.status == LocationServiceStatus.Initializing && waitTime < maxWaitTime)
        {
            yield return new WaitForSeconds(1.0f);
            waitTime++;
        }

        // ��ġ ���� ���ſ� ���������� ����Ѵ�.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            latitude_text.text = "��ġ ���� ���� ����!";
            longitude_text.text = "��ġ ���� ���� ����!";
        }

        // ������ �ð��� �ʰ��Ǿ����� ����Ѵ�.
        if (waitTime >= maxWaitTime)
        {
            latitude_text.text = "���� ��� �ð� �ʰ�";
            longitude_text.text = "���� ��� �ð� �ʰ�";
        }

        // ��ġ ���� ���� ���� üũ
        receiveGPS = true;

        // ��ġ ������ ���� ���� ���� resendTime ������� ��ġ ������ �����ϰ� ����Ѵ�.
        while (receiveGPS)
        {
            yield return new WaitForSeconds(resendTime);

            // ���ŵ� ��ġ ���� �����͸� UI�� ����Ѵ�.
            LocationInfo li = Input.location.lastData;
            latitude = li.latitude;
            longitude = li.longitude;

            latitude_text.text = "Latitude: " + latitude.ToString();
            longitude_text.text = "Longitude: " + longitude.ToString();
        }
    }
}
