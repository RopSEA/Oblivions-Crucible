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

    public Image iconHold;
    public Image iconHold2;

    public Image icon1;
    public Image icon2;

    private bool pri = false;
    private bool sec = false;
    private bool currSheild;
    public float rotRadius;
    public Transform RotatePoint;
    public GameObject MageBlast;
    public GameObject RotSheild;
    public int RotSpeed;


    // Stronger Mage Blast
    public override void priSkill()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pri == false)
            {
                pri = true;
                Instantiate(MageBlast, transform.position, transform.rotation);
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
            if (sec == false)
            {
                sec = true;
                currSheild = true;

                StopCoroutine("RotateSh");

                foreach (Transform child in RotatePoint)
                {
                    Destroy(child.gameObject);
                }


                Debug.Log("SEC SKILL");
                Instantiate(RotSheild, new Vector3(-rotRadius, 0, 0) + transform.position, RotatePoint.rotation, RotatePoint);
                Instantiate(RotSheild, new Vector3(rotRadius, 0, 0) + transform.position, RotatePoint.rotation, RotatePoint);
                Instantiate(RotSheild, new Vector3(0,-rotRadius, 0) + transform.position, RotatePoint.rotation, RotatePoint);
                Instantiate(RotSheild, new Vector3(0, rotRadius, 0) + transform.position, RotatePoint.rotation, RotatePoint);
                StartCoroutine(RotateSh());
                StartCoroutine(secCor());
            }

        }

    }

    IEnumerator RotateSh()
    {
        float time = 0;
        while (time <= 100)
        {
            RotatePoint.RotateAround(RotatePoint.position, Vector3.forward, RotSpeed);
            time += 0.1f;
            yield return new WaitForSeconds(0.001f);
        }

        foreach (Transform child in RotatePoint)
        {
            Destroy(child.gameObject);
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
        iconHold.sprite = icon1.sprite;
        iconHold2.sprite = icon2.sprite;
    }

    void Update()
    {
        priSkill();
        secSkill();
    }
}
