using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Pathfinding;

public class LazerBoss : BasicEnemyMovement
{

    public GameObject hpBar;
    public GameObject LazerV;
    public GameObject LazerH;
    public GameObject miniLazer;
    private bool isAttack1 = false;
    private bool isAttack2 = false;
    private int hpMax;
    private GameObject cam;
    public float wait;


    // for Path Finding
    public float nextWaypoinDist = 3f;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Path path;
    private int currWay = 0;
    private bool ReachedEND = false;



    private Vector3 h = new Vector3(-8.537104f, -2.552133f, -0.1259285f);
    private Vector3 v = new Vector3(-0.24f, -0.03f, -0.1259285f);

    public override void damage(int dam)
    {
        //

        if (hp - dam > 0)
        {
            hp = hp - dam;
            hpBar.GetComponent<HealthBar>().SetHealth(hp);
        }
        else
        {
            hp = 0;
            hpBar.GetComponent<HealthBar>().SetHealth(hp);
        }

        StartCoroutine(redDamage());
        ShowHitEffect();
        AudioManager.instance.PlaySfx("hitE");

        if (floatingText)
        {
            ShowNumber(dam);
        }

        if (hp == 0)
        {
            hpBar.SetActive(false);
            Destroy(gameObject);
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            //DataPersistenceManager.instance.GameData.enemiesDefeated++;
        }
    }


    IEnumerator redDamage()
    {
        List<MatchingElement> sprites = sprite.matchingTables;
        float dur = 0.25f;
        float elapsedTime = 0f;
        int hitEffectAmount = Shader.PropertyToID("_HitEffectAmount");

        while (elapsedTime < dur)
        {
            elapsedTime += Time.deltaTime;

            float lerpedAmt = Mathf.Lerp(1f, 0f, (elapsedTime / dur));
            foreach (MatchingElement i in sprites)
            {
                i.renderer.material.SetFloat(hitEffectAmount, lerpedAmt);
            }
            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < dur)
        {
            elapsedTime += Time.deltaTime;

            float lerpedAmt = Mathf.Lerp(0f, 1f, (elapsedTime / dur));
            foreach (MatchingElement i in sprites)
            {
                i.renderer.material.SetFloat(hitEffectAmount, lerpedAmt);
            }
            yield return null;

        }

    }

    void ShowHitEffect()
    {
        if (hitEffectPrefab != null)
        {

            GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 1f);
        }
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        cam = GameObject.FindWithTag("MainCamera");
        if (playerObj != null)
        {
            player = playerObj.transform;
            Debug.Log("Enemy found player: " + player.name);
        }
        else
        {
            Debug.LogWarning("No player found!");
        }
    }


    // first attack (LAZERZZZ)
    void attack1()
    {
        if (isAttack2 == true || isAttack1 == true)
        {
            return;
        }
        isAttack1 = true;
        StartCoroutine(firstMove());
    }

    IEnumerator firstMove()
    {
        GameObject temp;
        GameObject temp2;
        int rand;

        rand = Random.Range(1, 100);

        if (hp < (hpMax / 2))
        {
            temp = Instantiate(LazerH, h, transform.rotation);
            temp2 = Instantiate(LazerV, v, transform.rotation);
            yield return new WaitForSeconds(wait);
             AudioManager.instance.PlaySfx("lazerBig");
            cam.GetComponent<ScreenShake>().start = true;

            Destroy(temp, 1f);
            Destroy(temp2, 1f);
        }
        else if (rand % 2 == 0)
        {
            temp = Instantiate(LazerV, v, transform.rotation);
            yield return new WaitForSeconds(wait);
            AudioManager.instance.PlaySfx("lazerBig");
            cam.GetComponent<ScreenShake>().start = true;
            Destroy(temp, 1f);
        }
        else if (rand % 2 != 0)
        {
            temp = Instantiate(LazerH , h, transform.rotation);
            yield return new WaitForSeconds(wait);
            AudioManager.instance.PlaySfx("lazerBig");
            cam.GetComponent<ScreenShake>().start = true;
            Destroy(temp, 1f);
        }


        yield return new WaitForSeconds(4f);
        isAttack1 = false;
    }


    // second attack
    void attack2()
    {
        GameObject temp;
        if (isAttack2 == true || isAttack1 == true)
        {
            return;
        }
        isAttack2 = true;
        temp = Instantiate(miniLazer, transform.position, transform.rotation);
        AudioManager.instance.PlaySfx("lazerSmall");
        StartCoroutine(secCool());
    }

    IEnumerator secCool()
    {
        yield return new WaitForSeconds(3f);
        isAttack2 = false;
    }


    void chooseAttack()
    {
        int d = Random.Range(0, 2);
        Debug.Log(d);

        if (d == 0)
        {
            attack1();
        }
        else if (d == 1)
        {
            attack2();
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currWay = 0;
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, player.transform.position, OnPathComplete);
        }
    }


    void Start()
    {
        if (hpBar == null)
        {

            hpBar = GameObject.FindWithTag("BossBar");
            hpBar.SetActive(true);
            hpBar.GetComponent<HealthBar>().SetMax(hp);
            hpMax = hp;
        }

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        FindPlayer();


        List<MatchingElement> sprites = sprite.matchingTables;
        List<Material> materials = new List<Material>();
        int hitEffectAmount = Shader.PropertyToID("_HitEffectAmount");
        foreach (MatchingElement i in sprites)
        {
            i.renderer.material = hit;
            i.renderer.material.SetFloat(hitEffectAmount, 1);
        }

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            Debug.Log("hi no player found");
            FindPlayer();
            return;
        }


        //transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        if (isAttack2 == true || isAttack1 == true)
        {
            return;
        }
        chooseAttack();
    }

    private void FixedUpdate()
    {
        if (path == null)
            return;

        if (currWay >= path.vectorPath.Count)
        {
            ReachedEND = true;
            return;
        }
        else
        {
            ReachedEND = false;
        }

        Vector2 dir = ((Vector2)path.vectorPath[currWay] - rb.position).normalized;
        Vector2 force = dir * speed * Time.deltaTime;

        rb.AddForce(force);

        float dist = Vector2.Distance(rb.position, path.vectorPath[currWay]);

        if (dist < nextWaypoinDist)
        {
            currWay++;
        }
    }
}
