using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpeedsterClass : Classes
{

    public float priCooldown;
    public float secCooldown;
    private float lastPri;
    private float lastSec;
    private bool pri = false;
    private bool sec = false;
    public Image Img;
    public Image Img2;


    IEnumerator priCor()
    {
        float cool = priCooldown;
        Img.fillAmount = 1;
        while (cool >  0)
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

    public override void priSkill()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pri == true)
            {
                return;
            }
            pri = true;
            Debug.Log("PRIMARY");
            StartCoroutine(priCor());
        }
    }

    public override void secSkill()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (sec == true)
            {
                return;
            }
            sec = true;
            Debug.Log("SECONDARY");
            StartCoroutine(secCor());
        }
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        priSkill();
        secSkill();
    }
}
