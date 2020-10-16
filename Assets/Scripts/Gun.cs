using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    public string gunName; // 총의 이름

    public float range; // 사정거리
    public float accuracy; // 정확도
    public float fireRate; // 연사속도
    public float reloadTime;  // 재장전속도
    public float retroActionForce;  // 총의 반동
    public float retroActionFocus;  // 정조준시 반동

    public int damage;  // 데미지
    public int reloadBullet; // 총알 재장전 갯수
    public int stayBullet; // 현재 탄알집에 있는 총알갯수
    public int maxBullet; // 총알최대 보유 개수
    public int haveBullet; // 현재가지고 있는 총알갯수

    public Vector3 focusSightOriginPos;
    public ParticleSystem muzzleFlash;
    public Animator ani;
    //public AudioClip akmSound;


    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
