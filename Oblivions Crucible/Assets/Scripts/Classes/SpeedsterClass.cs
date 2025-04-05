using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

public class SpeedsterClass : Classes
{

    public float priCooldown;
    public float secCooldown;
    public Transform orbit;
    public Image Img;
    public Image Img2;


    private float lastPri;
    private float lastSec;
    private bool pri = false;
    private bool sec = false;
    private bool p1;
    private bool s;

    private State state;
    private enum State
    {
        Normal,
        prii,
        secc
    }



    IEnumerator priCor()
    {
        HealthSystem health = gameObject.GetComponent<HealthSystem>();
        PlayerMovement movement = gameObject.GetComponent<PlayerMovement>();

        float cool = priCooldown;
        float dist = 150f;
        Img.fillAmount = 1;

        health.SetInvulnerable(true); 
        gameObject.GetComponent<TrailRenderer>().emitting = true;
        p1 = true;

        while (dist > 0)
        {
            gameObject.transform.position += movement.getSlideDir() * 200 * Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
            dist -= 10f;
        }

        health.SetInvulnerable(false); 
        gameObject.GetComponent<TrailRenderer>().emitting = false;
        p1 = false;
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
        HealthSystem health = gameObject.GetComponent<HealthSystem>();
        float cool = secCooldown;
        float rot = 270;
        Img2.fillAmount = 1;

        gameObject.GetComponent<TrailRenderer>().emitting = true;
        health.SetInvulnerable(true);
        s = true;
        while (rot > 0)
        {
            gameObject.transform.RotateAround(orbit.position ,Vector3.forward , 10);
            yield return new WaitForSeconds(0.0025f);
            rot -= 10f;
        }
        this.transform.Rotate(Vector3.forward, -270);
        health.SetInvulnerable(false);
        s = false;
        gameObject.GetComponent<TrailRenderer>().emitting = false;

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
        Player p = gameObject.GetComponent<Player>();
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pri == true)
            {
                return;
            }
            p1 = true;
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
            s = true;
            sec = true;
            Debug.Log("SECONDARY");

            StartCoroutine(secCor());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Player p = gameObject.GetComponent<Player>();
        if (collision.collider.tag == "Enemy" && (p1 == true || s == true))
        {
            collision.gameObject.GetComponent<BasicEnemyMovement>().damage(100);
        }
    }


    void Start()
    {
        state = State.Normal;
        gameObject.GetComponent<TrailRenderer>().emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        priSkill();
        secSkill();
    }
}
