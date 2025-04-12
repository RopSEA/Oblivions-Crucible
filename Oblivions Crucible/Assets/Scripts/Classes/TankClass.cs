using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankClass : Classes
{
    public float priCooldown;
    public float secCooldown;
    public Image Img;
    public Image Img2;

    private bool pri = false;
    private bool sec = false;
    public GameObject slashPrefab;
    public GameObject sheildPrefab;



    // Slash
    public override void priSkill()
    {
        GameObject temp = null;
        PlayerMovement movement = gameObject.GetComponent<PlayerMovement>();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pri == false)
            {
                pri = true;
                Debug.Log("PRI SKILL tank");
                // instantiate sword
                temp = Instantiate(slashPrefab, transform.position, transform.rotation, gameObject.transform);
                temp.transform.rotation = Quaternion.LookRotation(Vector3.forward, movement.dirs);
                Destroy(temp, 0.5f);
                StartCoroutine(priCor());
            }
        }
    }

    // Sheild bash
    public override void secSkill()
    {
        GameObject temp = null;
        PlayerMovement movement = gameObject.GetComponent<PlayerMovement>();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (sec == false)
            {
                sec = true;
                Debug.Log("SEC SKILL tank");
                // instantiate sheild
                temp = Instantiate(sheildPrefab, transform.position, transform.rotation);
                temp.transform.rotation = Quaternion.LookRotation(Vector3.forward, movement.dirs);
                Destroy(temp, 0.35f);
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
