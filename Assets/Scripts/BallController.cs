using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.ParticleSystem;

public class BallController : MonoBehaviour
{
    public float resetTime = 3.0f;
    public float captureRate = 0.3f;
    public TMP_Text result;

    public GameObject mainMode;
    public GameObject effect;
    public GameObject player;

    Rigidbody rb;
    bool isReady = true;
    bool isPressed = false;
    Vector2 startPos, currentPos;

    void Start()
    {
        result = GameObject.Find("Result").GetComponent<TMP_Text>();
        result.text = "";

        mainMode = GameObject.Find("Main Mode");
        player = GameObject.Find("Player");

        rb = this.transform.GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void Update()
    {
        if (!isReady)   // ����ڰ� ���� �߻��� ���� ���ư��� �ִ� ���߿��� ���� ��ġ�� ī�޶� �տ� ������Ű�� �ȵ�
        {
            return;
        }

        SetBallPosition(Camera.main.transform); // ���� ī�޶� ���� �ϴܿ� ��ġ�Ѵ�.
    }

    void SetBallPosition(Transform anchor)
    {
        Vector3 offset = anchor.forward * 0.5f + anchor.up * -0.2f;
        transform.position = anchor.position + offset;
    }

    void ResetBall()
    {
        rb.isKinematic = true;      // ���� ��� ��Ȱ��ȭ
        rb.velocity = Vector3.zero; // �ӵ� �ʱ�ȭ
        isReady = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (isReady)
        {
            return;
        }

        if (other.transform.CompareTag("Monster"))
        {
            Debug.Log("---------------------------PokeBall Detected---------------------------");
            float draw = Random.Range(0, 1.0f);

            if (draw <= captureRate)
            {
                result.text = other.name.Substring(0, other.name.Length - 7) + "��(��) ��Ҵ�!";

                DB_Manager.instance.UpdateCaptured();
            }
            else
            {
                result.text = "��! ������ ���͹��ȴ�! ���ݸ� �� �ϸ� ���� �� �־��µ�!";
            }

            //Instantiate(effect, other.transform.position, Camera.main.transform.rotation);

            Destroy(other.gameObject);
            StartCoroutine(Loading());
        }
    }

    public void TouchBall(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            isPressed = false;

            float dragDistance = currentPos.y - startPos.y;
            Vector3 throwAngle = (Camera.main.transform.forward + Camera.main.transform.up).normalized;

            isReady = false;
            rb.isKinematic = false;
            rb.AddForce(throwAngle * dragDistance * 0.005f, ForceMode.VelocityChange);

            Invoke("ResetBall", resetTime);
        }
    }

    public void DragBall(InputAction.CallbackContext context)
    {
        Vector2 touchPosition = context.ReadValue<Vector2>();

        if (touchPosition != null)
        {
            if (isPressed == false)
            {
                isPressed = true;
                startPos = touchPosition;
            }
            else
            {
                currentPos = touchPosition;
            }
        }
    }

    IEnumerator Loading()
    {
        yield return new WaitForSeconds(1.5f);
        result.text = "";
        player.GetComponent<Player>().shootingFlag = false;
        player.GetComponent<Player>().monsterDieFlag = false;
        Destroy(this.gameObject);
    }
}
