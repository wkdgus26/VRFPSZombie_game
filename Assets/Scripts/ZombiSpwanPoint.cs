using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiSpwanPoint : MonoBehaviour {
    public Transform[] points;
    public GameObject zombie;
    public float createTime = 1.7f;
    private float delay = 0;
    void Start() {

        //GameObject zombie = Instantiate(Resources.Load("zombie", typeof(GameObject))) as GameObject;
        //points = GameObject.Find("zombieSpawnPoint").GetComponentsInChildren<Transform>();
        //StartCoroutine(this.CreateZombie());
    }

    IEnumerator CreateZombie()
    {
        points = GameObject.Find("zombieSpawnPoint").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        Instantiate(zombie, points[idx].position, Quaternion.identity);
        //yield return new WaitForSeconds(createTime);
        yield return null;
    }
	
	// Update is called once per frame
	void Update () {
        delay += Time.deltaTime;
        if (delay >= createTime)
        {
            StartCoroutine(this.CreateZombie());
            delay = 0;
        }
        
    }
}
