using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    [SerializeField]
    private Gun currentGun;

    private CrossHair theCrossHair;

    private float currentFireRate;
    
    //private AudioSource reloadAudio;
    private AudioSource akmAudio;
    [SerializeField]
    private AudioClip[] clip;

    private bool isReload = false;
    private bool isRun = false;
    private RaycastHit hitInfo; // 레이저 충돌정보 받아옴
    public ParticleSystem hitGround;

    
    [SerializeField]
    private Camera theCamera;

    private Vector3 originPos;
    private Vector3 crosshairOriginPos;
    private Quaternion crosshairOriginRot;
    private Vector3 playerAimPos;
    [SerializeField]
    private Vector3 aimPos;
    [SerializeField]
    private Vector3 gunOriginPos;
    [SerializeField]
    private GameObject crosshair;
    private bool isShooting = false;

    private GameObject gun;
    private GameObject spawnPoint;
    private Vector3 aimOriginPos;

    [SerializeField] int speed;
    private bool isFindSightMode = false;

    public static bool isTalk = false;

    void Start()
    {
        akmAudio = GetComponent<AudioSource>();
        gun = gameObject.transform.GetChild(2).gameObject;
        spawnPoint = gun.transform.GetChild(1).gameObject;
        originPos = theCamera.transform.localPosition;
        crosshairOriginPos = this.transform.localPosition;
        crosshairOriginRot = this.transform.localRotation;
        playerAimPos = originPos;

        aimOriginPos = this.transform.localPosition;
        theCrossHair = FindObjectOfType<CrossHair>();
    }
    void Update()
    {
        GunFireRateCalc();
        TryFire();
        TryStopFire();
        TryReload();
        TryAim();
    }

    private void TryAim()
    {
        //Input.GetButton("Fire2")
        if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad) && !isReload)
        {
            Aim();
        }
        if (OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad)) 
        {
            StopAim();
        }
    }

    private void Aim()
    {
        isFindSightMode = true;
        playerAimPos = aimPos;
        currentGun.ani.Rebind();
        theCrossHair.AimAnimation(true);
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 45f, Time.deltaTime * 15f);
        StartCoroutine(AimCoroutine());
    }

    private void CanelAim()
    {
        if (isFindSightMode)
            StopAim();
    }

    private void StopAim()
    {
        isFindSightMode = false;
        playerAimPos = originPos;
        theCrossHair.AimAnimation(false);
        Camera.main.fieldOfView = 90f;
        StartCoroutine(AimCoroutine());
    }


    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime;
        }
    }

    private void TryFire()
    {
        //(Input.GetButton("Fire1")
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && currentFireRate <= 0 && !isReload)
            Fire();
    }

    private void TryStopFire()
    {
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
            StopFire();
    }

    private void Fire()     // 발사전 준비
    {
        if (!isReload)
        {
            if (currentGun.stayBullet != 0)
                Shoot();

            else
            {
                CanelAim();
                StartCoroutine(ReloadCoroutine());
            }
        }
    }

    private void Shoot()        // 발사후
    {
        currentGun.stayBullet--;
        currentFireRate = currentGun.fireRate;  // 연산속도 재계산
                                                //currentGun.ani.SetBool("isShoot", true);
                                                // akmAudio.PlayOneShot(audioClip);
        akmAudio.clip = clip[0];
        akmAudio.Play();
        currentGun.muzzleFlash.Play();
        theCrossHair.FireAnimation(true);
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
        Hit();
    }

    private void Hit()
    {
        float positionX = Random.Range(-theCrossHair.GetAccuracy() - currentGun.accuracy, theCrossHair.GetAccuracy() + currentGun.accuracy);
        float positionY = Random.Range(0, theCrossHair.GetAccuracy() + currentGun.accuracy);
        if (Physics.Raycast(theCamera.transform.position, theCamera.transform.forward +
           new Vector3(positionX, positionY, 0), out hitInfo, currentGun.range))
        {
            Instantiate(hitGround, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            if (!isShooting)
            {
                StartCoroutine(HitCoroutine());
            }
        }
        //StartCoroutine(HitCoroutine());

    }

    private void StopFire()
    {
        //currentGun.ani.SetBool("isShoot", false);
        currentGun.muzzleFlash.Stop();
        theCrossHair.FireAnimation(false);
    }

    private void TryReload()
    {
        //if(Input.GetKeyDown("r"))

        if (OVRInput.GetDown(OVRInput.Button.Back) && currentGun.stayBullet != currentGun.reloadBullet && !isReload)
        {
            CanelAim();
            StartCoroutine(ReloadCoroutine());
        }
    }

    public bool GetAim()
    {
        return isFindSightMode;
    }
   
    IEnumerator HitCoroutine()
    {
        isShooting = true;

        GameObject bullet = Instantiate(Resources.Load("bullet1", typeof(GameObject))) as GameObject;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        bullet.transform.rotation = spawnPoint.transform.rotation;
        bullet.transform.position = spawnPoint.transform.position;
        //Instantiate(bullet, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
        rb.AddForce(theCamera.transform.forward * speed);
        Destroy(bullet, 3f);
        yield return null;
        isShooting = false;
    }

    IEnumerator AimCoroutine()
    {
        int count = 0;
        Vector3 pos = this.transform.localPosition;
        while (pos != playerAimPos)
        {
            count++;
            pos = Vector3.Lerp(pos, playerAimPos, 0.3f);
            this.transform.localPosition = pos;
            if (count > 10)
            {
                break;
            }
            yield return null;
        }
        this.transform.localPosition = playerAimPos;
    }

    IEnumerator RetroActionCoroutine()
    {
        Vector3 recoilBack = new Vector3(gunOriginPos.x, gunOriginPos.y, currentGun.retroActionForce);
        Vector3 AimRecoilBack = new Vector3(currentGun.focusSightOriginPos.x, currentGun.focusSightOriginPos.y, currentGun.retroActionFocus);

        if (!isFindSightMode)
        {
            currentGun.transform.localPosition = gunOriginPos;

            // 반동시작
            while (currentGun.transform.localPosition.z > currentGun.retroActionForce + 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(recoilBack, currentGun.transform.localPosition, 0.4f);
                yield return null;
            }

            // 원위치

            while (currentGun.transform.localPosition != gunOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, gunOriginPos, 0.1f);
                yield return null;
            }

        }
        else
        {
            currentGun.transform.localPosition = currentGun.focusSightOriginPos;

            // 반동시작
            while (currentGun.transform.localPosition.z > currentGun.retroActionFocus + 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(AimRecoilBack, currentGun.transform.localPosition, 0.4f);
                yield return null;
            }

            // 원위치

            while (currentGun.transform.localPosition != currentGun.focusSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.focusSightOriginPos, 0.1f);
                yield return null;
            }
        }

    }

    IEnumerator ReloadCoroutine()
    {
        int numOfBullet;
        isReload = true;
        currentGun.ani.SetBool("isShoot", false);
        currentGun.muzzleFlash.Stop();

        if (currentGun.haveBullet > 0)
        {
            akmAudio.clip = clip[1];
            akmAudio.Play();
            currentGun.ani.SetTrigger("Reload");

            yield return new WaitForSeconds(currentGun.reloadTime);
            if (currentGun.stayBullet == 0) // 자동장전
            {
                if (currentGun.haveBullet >= currentGun.reloadBullet)
                {
                    currentGun.stayBullet = currentGun.reloadBullet;
                    currentGun.haveBullet -= currentGun.reloadBullet;
                }
                else
                {
                    currentGun.stayBullet = currentGun.haveBullet;
                    currentGun.haveBullet = 0;
                }
            }


            if (currentGun.haveBullet + currentGun.stayBullet >= currentGun.reloadBullet)
            {
                numOfBullet = currentGun.reloadBullet - currentGun.stayBullet;
                currentGun.stayBullet = currentGun.reloadBullet;
                currentGun.haveBullet -= numOfBullet;
            }
            else
            {
                currentGun.stayBullet += currentGun.haveBullet;
                currentGun.haveBullet = 0;
            }



            if (OVRInput.GetUp(OVRInput.Button.Back))
            {
                currentGun.ani.SetBool("isReload", false);
            }
        }
        else
        {
            Debug.Log("총알 없음");
        }


        isReload = false;

    }
    public Gun GetGun()
    {
        return currentGun;
    }
}
