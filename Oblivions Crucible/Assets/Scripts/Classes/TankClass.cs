using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.ParticleSystem;

public class TankClass : Classes
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
    public GameObject slashPrefab;
    public GameObject sheildPrefab;


    Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform nearestEnemy = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(currentPosition, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        return nearestEnemy;
    }

    Vector2 findDir()
    {

        Transform temp2 = FindNearestEnemy();

        Vector2 dir;

        if (temp2 != null)
        {
            // calculate direction toward the enemy
            Vector2 direction = (temp2.position - transform.position).normalized;

            // rotate toward enemy
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            dir = direction;
        }
        else
        {
            // no enemy found, shoot up
            dir = transform.up;
            //moveDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        return dir;
    }



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
                temp.transform.rotation = Quaternion.LookRotation(Vector3.forward, findDir());
                Destroy(temp, 0.5f);
                StartCoroutine(priCor());
            }
        }
    }

    // Sheild bash
    public override void secSkill()
    {
        GameObject temp = null;
        GameObject temp2 = null;
        PlayerMovement movement = gameObject.GetComponent<PlayerMovement>();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (sec == false)
            {
                sec = true;
                Debug.Log("SEC SKILL tank");
                // instantiate sheild
                temp = Instantiate(sheildPrefab, transform.position, transform.rotation);
                
                temp.transform.rotation = Quaternion.LookRotation(Vector3.forward, findDir());
                Destroy(temp, 0.35f);
                StartCoroutine(secCor());
            }
        }
    }

    IEnumerator priCor()
    {
        float cool = priCooldown;
        bool first = true;
        Img.fillAmount = 1;

        while (cool > 0)
        {
            Img.fillAmount -= 0.5f / priCooldown;
            yield return new WaitForSeconds(0.5f);
            if (first == true)
            {
                transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector2(0, 0));
                first = false;
            }
            
            cool -= 0.5f;
        }
        Debug.Log("READY PRI");
        pri = false;
    }

    IEnumerator secCor()
    {
        float cool = secCooldown;
        bool first = true;
        Img2.fillAmount = 1;

        while (cool > 0)
        {
            Img2.fillAmount -= 0.5f / secCooldown;
            yield return new WaitForSeconds(0.5f);
            if (first == true)
            {
                transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector2(0, 0));
                first = false;
            }
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
