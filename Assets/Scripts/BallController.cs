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
        if (!isReady)   // 사용자가 공을 발사해 공이 날아가고 있는 도중에는 공의 위치를 카메라 앞에 고정시키면 안됨
        {
            return;
        }

        SetBallPosition(Camera.main.transform); // 공을 카메라 전방 하단에 배치한다.
    }

    void SetBallPosition(Transform anchor)
    {
        Vector3 offset = anchor.forward * 0.5f + anchor.up * -0.2f;
        transform.position = anchor.position + offset;
    }

    void ResetBall()
    {
        rb.isKinematic = true;      // 물리 기능 비활성화
        rb.velocity = Vector3.zero; // 속도 초기화
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
                result.text = other.name.Substring(0, other.name.Length - 7) + "을(를) 잡았다!";

                DB_Manager.instance.UpdateCaptured();
            }
            else
            {
                result.text = "앗! 볼에서 나와버렸다! 조금만 더 하면 잡을 수 있었는데!";
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
