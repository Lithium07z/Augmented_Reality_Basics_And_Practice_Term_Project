using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Monster : MonoBehaviour
{
    public MonstersMainMode mainMode;
    
    private GameObject player;
    public GameObject shootingPosition;
    public GameObject detailPanel;
    public GameObject pokeBall;
    public GameObject bullet;

    public Animator animator;

    //private Coroutine attackCoroutine;

    public string monsterName;
    public string description;

    public int monsterHealth = 100;
    public int damage = 10;
    public float captureRate;
    public float attackDelay;

    private bool tracking = false;

    void Start()
    {
        mainMode = GameObject.Find("Main Mode").GetComponent<MonstersMainMode>(); ;
        player = GameObject.Find("Player");
        detailPanel = mainMode.detailsPanel;
        player.GetComponent<Player>().monster = this.gameObject;
    }

    void Update()
    {
        if (tracking)
        {
            this.transform.LookAt(player.transform.position);
        }
    }

    public void BattleStart()
    {
        detailPanel.SetActive(false);
        tracking = true;
        //attackCoroutine = StartCoroutine(attackPlayer());
        Debug.Log("----------------------Monster Battle Start----------------------");
        if (animator != null)
        {
            animator.SetBool("Start", true);
        }
        Debug.Log("----------------------Monster Battle End----------------------");
    }

    public void GetDamaged(int damage)
    {
        monsterHealth -= damage;
        if (animator != null)
        {
            animator.SetTrigger("Take Damage");
        }

        if (monsterHealth <= 0)
        {
            MonsterDied();
        }
    }

    public void MonsterDied()
    {
        Debug.Log("---------------------------Monster Died---------------------------");
        if (animator != null)
        {
            animator.SetBool("Die", true);
        }

        //StopCoroutine(attackCoroutine);

        GameObject pokeball = Instantiate(pokeBall, Vector3.zero, Quaternion.Euler(0, 90, 0));
        pokeBall.GetComponent<BallController>().captureRate = captureRate;

        player.GetComponent<Player>().monsterDieFlag = true;
        mainMode.infoButton.interactable = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Bullet"))
        {
            Debug.Log("---------------------------Bullet Detected---------------------------");
            GetDamaged(player.GetComponent<Player>().playerDamage);
        }
    }
    /*
    IEnumerator attackPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackDelay);
            //GameObject temp = Instantiate(bullet, shootingPosition.transform.position, this.transform.rotation);
            GameObject temp = Instantiate(bullet, this.transform.parent.position, this.transform.parent.rotation);
            Destroy(temp, 3.0f);
            
            Vector3 throwAngle = player.transform.position - this.transform.parent.position;
            temp.GetComponent<Rigidbody>().AddForce(throwAngle * 400);
        }
    }
    */
}
