using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavController : MonoBehaviour
{
    [SerializeField]
    private PlayerHp playerHp;
    [SerializeField]
    private ZombiSpwanPoint zombiePoint;

    private Transform goal;
    private NavMeshAgent agent;
    private GameObject goalObject;
    private GameObject goalObject2;
    private GameObject goalObject3;
    [SerializeField]
    private GameObject startObject;

    private float delayTIme = 30f;
    private float delay;
    
    private AudioSource walkAudio;

    [SerializeField]
    private GameObject talkBox;
    [SerializeField]
    private GameObject talkBox1;
    [SerializeField]
    private GameObject talkBox2;
    [SerializeField]
    private GameObject talkBox3;
    [SerializeField]
    private GameObject talkBox_Dead;

    [SerializeField]
    private GameObject spwanAlways;
    [SerializeField]
    private GameObject spawnPoint1;
    [SerializeField]
    private GameObject spawnPoint2;

    private int count = 0;

    void Start()
    {
        goalObject = GameObject.Find("goalStation_1");
        goalObject2 = GameObject.Find("goalStation_2");
        goalObject3 = GameObject.Find("goalStation_3");
        goal = goalObject.transform;
        agent = GetComponent<NavMeshAgent>();
        agent.destination = new Vector3(goal.position.x, goal.position.y, goal.position.z);
        walkAudio = GetComponent<AudioSource>();
    }

    

    // Update is called once per frame
    void Update()
    {
        Arrive();
        ReStart();
        
    }

    private void ReStart()
    {
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && GunController.isTalk == true)
        {
            if (count == 0)
            {
                walkAudio.Play();
            }
            GunController.isTalk = false;
            talkBox.SetActive(false);
            talkBox1.SetActive(false);
            talkBox2.SetActive(false);
            Time.timeScale = 1f;
            count++;
        }
    }

    private void Arrive()
    {
        if (this.transform.position.x - 1f < startObject.transform.position.x && this.transform.position.x + 1f > startObject.transform.position.x && count == 0)
        {
            walkAudio.Stop();
            GunController.isTalk = true;
            Time.timeScale = 0f;
        }
        delay += Time.deltaTime;
        if (delay >= delayTIme)
        {
            StartCoroutine(ArriveCoroutine());
        }
    }
    private void StartTalkBox()
    {
        walkAudio.Stop();
        GunController.isTalk = true;
        talkBox.SetActive(true);
        Time.timeScale = 0f;
        
    }

    private void CallTalkBox()
    {
        walkAudio.Stop();
        GunController.isTalk = true;
        talkBox1.SetActive(true);
        Time.timeScale = 0f;
        spwanAlways.SetActive(false);
        spawnPoint1.SetActive(true);
        zombiePoint.createTime = 0.7f;
    }

    private void CallTalkBox2()
    {
        walkAudio.Stop();
        GunController.isTalk = true;
        talkBox2.SetActive(true);
        Time.timeScale = 0f;
        spwanAlways.SetActive(false);
        spawnPoint2.SetActive(true);
        zombiePoint.createTime = 0.7f;
    }

    private void CallTalkBox3()
    {
        walkAudio.Stop();
        talkBox3.SetActive(true);
        Time.timeScale = 0f;
    }

    IEnumerator ArriveCoroutine()
    {
        
        if (this.transform.position.x - 1f < goalObject.transform.position.x && this.transform.position.x + 1f > goalObject.transform.position.x && this.transform.position.x != goalObject3.transform.position.x && count == 1)
        {
            CallTalkBox();
            yield return new WaitForSeconds(10f);
            spwanAlways.SetActive(true);
            spawnPoint1.SetActive(false);
            zombiePoint.createTime = 1.7f;
            walkAudio.Play();
            goal = goalObject2.transform;
            agent = GetComponent<NavMeshAgent>();
            agent.destination = new Vector3(goal.position.x, goal.position.y, goal.position.z);

        }

        else if (this.transform.position.x - 1f < goalObject2.transform.position.x && this.transform.position.x + 1f > goalObject2.transform.position.x && count == 2)
        {
            CallTalkBox2();
            yield return new WaitForSeconds(10f);
            spwanAlways.SetActive(true);
            spawnPoint2.SetActive(false);
            zombiePoint.createTime = 1.7f;
            walkAudio.Play();
            goalObject = GameObject.Find("goalStation_3");
            goal = goalObject.transform;
            agent = GetComponent<NavMeshAgent>();
            agent.destination = new Vector3(goal.position.x, goal.position.y, goal.position.z);
        }

        if (this.transform.position.x - 1f < goalObject3.transform.position.x && this.transform.position.x + 1f > goalObject3.transform.position.x)
        {
            CallTalkBox3();
            walkAudio.Stop();
        }

        if (playerHp.currentHp <= 0)
        {
            walkAudio.Stop();
            talkBox_Dead.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}