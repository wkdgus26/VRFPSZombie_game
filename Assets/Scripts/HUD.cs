using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    [SerializeField]
    private GunController theGunController;
    private Gun currentGun;

    [SerializeField]
    private GameObject go_BulletHUD; // 필요하면 HUD호출, 필요없으면 비활성화

    [SerializeField]
    private Text[] text_Bullet; // 총알 개수 텍스트에 반영

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        CheckBullet();
	}

    private void CheckBullet()
    {
        currentGun = theGunController.GetGun();
        text_Bullet[0].text = currentGun.stayBullet.ToString();
        text_Bullet[1].text = currentGun.haveBullet.ToString();
    }
}
