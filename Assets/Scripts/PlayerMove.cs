using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour {

    [SerializeField]
    private GameObject navObject;
    
    void Start ()
    {   
       
    }
	
	// Update is called once per frame
	void Update () {
        Arrive();
	}

    private void Arrive()
    {
        this.transform.position = new Vector3 (navObject.transform.position.x, this.transform.position.y, navObject.transform.position.z);
    }

   
}
