using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject shootingPosition;
    public GameObject monster;
    public GameObject bullet;
    
    public int playerHealth = 100;
    public int playerDamage = 20;

    public bool shootingFlag = false;
    public bool monsterDieFlag = false;

    void Start()
    {
        playerHealth = 100;
    }

    void Update()
    {
        if (shootingFlag)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began && !monsterDieFlag)
                {
                    Shoot();
                }
            }
        }
    }

    public void GetDamaged(int damage) 
    { 
        playerHealth -= damage;

        if (playerHealth <= 0)
        {
            Debug.Log("--------------------Player Died--------------------");
        }
    }

    void Shoot()
    {
        GameObject temp = Instantiate(bullet, shootingPosition.transform.position, this.transform.rotation);
        Destroy(temp, 3.0f);

        Vector3 throwAngle = Camera.main.transform.forward.normalized;
        temp.GetComponent<Rigidbody>().AddForce(throwAngle * 800);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("MonsterBullet"))
        {
            Debug.Log("--------------------Player Hit--------------------");
            GetDamaged(GameObject.FindWithTag("Monster").GetComponent<Monster>().damage);
        }    
    }
}
