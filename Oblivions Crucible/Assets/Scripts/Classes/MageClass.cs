using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MageClass : Classes
{
    public float priCooldown;
    public float secCooldown;
    public Image Img;
    public Image Img2;

    private bool pri = false;
    private bool sec = false;


    // Stronger Mage Blast
    public override void priSkill()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pri == false)
            {
                Debug.Log("PRI SKILL");
                StartCoroutine(priCor());
            }
        }
    }

    // Magiacal Rotating Sheild
    public override void secSkill()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (sec  == false)
            {
                Debug.Log("SEC SKILL");
                StartCoroutine(secCor());
            }
          
        }

    }

    IEnumerator priCor()
    {
        float cool = priCooldown;
        Img.fillAmount = 1;

        while (cool > 0)
        {
            Img.fillAmount -= 0.5f / priCooldown;
            yield return new WaitForSeconds(0.5f);
            cool -= 0.5f;
        }
        Debug.Log("READY PRI");
        pri = false;
    }

    IEnumerator secCor()
    {
        float cool = secCooldown;
        Img2.fillAmount = 1;

        while (cool > 0)
        {
            Img2.fillAmount -= 0.5f / secCooldown;
            yield return new WaitForSeconds(0.5f);
            cool -= 0.5f;
        }

        Debug.Log("READY SEC");
        sec = false;
    }

    void Start()
    {
        
    }

    void Update()
    {
        priSkill();
        secSkill();
    }
}
