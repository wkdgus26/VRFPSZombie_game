using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour {

    [SerializeField]
    public Animator animator;

    private float gunAccuracy; // 크로스헤어의 상태에 따른 총의 정확도
    [SerializeField]
    private GunController theGunController;
	// Update is called once per frame
	void Update () {
        //RunningAnimation();
	}

    public void AimAnimation(bool _flag)
    {
        animator.SetBool("Aim", _flag);
    }
    public void FireAnimation(bool _flag)
    {
        animator.SetBool("RunFire", _flag);
    }
    
    public float GetAccuracy()
    {
        if (theGunController.GetAim())
        {
            gunAccuracy = 0.01f;
        }
        else
        {
            gunAccuracy = 0.04f;
        }
        return gunAccuracy;
    }
    
}
