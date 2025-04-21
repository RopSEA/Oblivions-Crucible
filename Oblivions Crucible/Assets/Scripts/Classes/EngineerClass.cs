using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EngineerClass : Classes
{
    public float priCooldown;
    public float secCooldown;
    public Image Img;
    public Image Img2;

    public Image iconHold;
    public Image iconHold2;

    public Image icon1;
    public Image icon2;


    private bool pri = false;
    private bool sec = false;
    public GameObject turretPrefab;
    private GameObject currTurr;
    public Transform turretLoc;
    public GameObject bulletPrefab;

    public override void priSkill()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pri == false)
            {
                Debug.Log("PRIMARY Engineer");
                pri = true;
                if (currTurr != null)
                {
                    Destroy(currTurr);
                    currTurr = null;
                }
                currTurr = Instantiate(turretPrefab, turretLoc.position, turretLoc.rotation);
                StartCoroutine(priCor());
            }
            
        }
    }

    public override void secSkill()
    {
        int rot = 0;
        GameObject temp;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (sec == false)
            {
                Debug.Log("SECONDARY Engineer");
                sec = true;

                for (int i = 0; i < 8; i++)
                {
                    temp = Instantiate(bulletPrefab, transform.position, transform.rotation);
                    temp.GetComponent<ForwardBullet>().updateDir(rot);
                    rot += 45;
                }

                // Cooldown
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
        if (RelicRunData.instance != null)
        {
            RelicRunData.instance.AddStats(this);
        }
        iconHold.sprite = icon1.sprite;
        iconHold2.sprite = icon2.sprite;
    }
    void Update()
    {
        priSkill();
        secSkill();
    }
}
