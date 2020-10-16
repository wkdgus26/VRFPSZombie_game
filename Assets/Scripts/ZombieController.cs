using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    //[SerializeField]
    private Transform goal;
    private NavMeshAgent agent;
    [SerializeField] int hp = 100;

    int count = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = new Vector3(goal.position.x, goal.position.y, goal.position.z);
        GetComponent<Animation>().Play("walk");
    }

    public void Update()
    {
        if(count==0)
        { 
            goal = Camera.main.transform;
            agent.destination = new Vector3(goal.position.x, goal.position.y, goal.position.z);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "bullet")
        {
            count++;
            GetComponent<CapsuleCollider>().enabled = false;
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            Destroy(rigidbody);
            Destroy(col.gameObject);
            agent.destination = gameObject.transform.position;
            GetComponent<Animation>().Stop("walk");
            GetComponent<Animation>().Play("back_fall");
            Destroy(gameObject, 1.5f);
        }
    }

 
}
