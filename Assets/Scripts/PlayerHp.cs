using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour {
    [SerializeField]
    private GameObject HitPanel;
    public Image Hp_Back, Hpbar;

    public int hpSingleBar = 20;
    public int MaxHp = 100;
    public int currentHp = 100;

    public bool isHit = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Refresh();
	}

    float GetHpRationInSingleBar(int targetHp)
    {
        float resultRatio = 0;

        float divided = (float)targetHp / hpSingleBar;

        if (targetHp > 0)
        {
            if (divided == (int)divided)
            {
                resultRatio = 1;
            }
            else
            {
                float moduled = targetHp % hpSingleBar;

                resultRatio = moduled / hpSingleBar;
            }
        }
        
        else
        {
            resultRatio = 0;
        }
        return resultRatio;
    }

    private void Refresh()
    {
        Hpbar.rectTransform.sizeDelta = new Vector2(Hp_Back.rectTransform
            .sizeDelta.x * GetHpRationInSingleBar(currentHp), Hp_Back.rectTransform.sizeDelta.y);
    }

    
    private void OnCollisionEnter(Collision col)
    {
        if (isHit == false && col.gameObject.tag == "zombie")
        {
            StartCoroutine(HitCoroutine());
        }
    }

    IEnumerator HitCoroutine()
    {
        HitPanel.SetActive(true);
        StartCoroutine(HpCoroutine());
        yield return new WaitForSeconds(0.6f);
        HitPanel.SetActive(false);
    }

    IEnumerator HpCoroutine()
    {
        isHit = !isHit;
        currentHp -= 7;
        yield return new WaitForSeconds(2f);
        isHit = !isHit;
    }
    




}
