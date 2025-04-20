using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : BasicEnemyMovement
{
    public GameObject hpBar;
    public GameObject bulletPre;
    public GameObject bulletPre2;
    private bool isAttack1 = false;
    private bool isAttack2 = false;
    private bool isDed = false;


    // for Path Finding
    public float nextWaypoinDist = 3f;
    private Seeker seeker;
    private Rigidbody2D rb;
    private Path path;
    private int currWay = 0;
    private bool ReachedEND = false;

    public GameObject Expolosion;

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
        AudioManager.instance.PlaySfx("hitE", true);

        if (floatingText)
        {
            ShowNumber(dam);
        }

        if (hp == 0)
        {
            
            if (isDed == false)
            {
                hpBar.SetActive(false);
                isAttack1 = true;
                isDed = true;
                StartCoroutine(onDeath());
            }
            
            //DataPersistenceManager.instance.GameData.enemiesDefeated++;
        }
    }

    IEnumerator onDeath()
    {

        for (int i = 0; i < 8; i++)
        {
            showExpo();
            yield return new WaitForSeconds(0.5f);
        }

        Destroy(gameObject);
        GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
        DataPersistenceManager.instance.GameData.enemiesDefeated++;
    }


    public void showExpo()
    {
        var go = Instantiate(Expolosion, transform.position, Quaternion.identity, transform);
        StartCoroutine(redDamage());
        AudioManager.instance.PlaySfx("hitE", true);
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


    // first attack
    void attack1()
    {
        if (isAttack2 == true || isAttack1 == true || isDed == true)
        {
            return;
        }
        isAttack1 = true;
        StartCoroutine(firstMove());
    }

    IEnumerator firstMove()
    {
        int rot = 0;
        int comp = 0;
        GameObject temp;

        while (comp < 20)
        {
            for (int i = 0; i < 8; i++)
            {
                temp = Instantiate(bulletPre, transform.position, transform.rotation);
                temp.GetComponent<EnemyHomingBullet>().updateDir(rot);
                rot += 45;
            }
            rot = 0;
            comp++;
            yield return new WaitForSeconds(0.3f);
        }


        yield return new WaitForSeconds(4f);
        isAttack1 = false;
    }


    // second attack
    void attack2() 
    {
        GameObject temp;
        if (isAttack2 == true || isAttack1 == true || isDed == true)
        {
            return;
        }
        isAttack2 = true;
        temp = Instantiate(bulletPre2, transform.position, transform.rotation);

        StartCoroutine(secCool());
    }

    IEnumerator secCool()
    {
        yield return new WaitForSeconds(3f);
        isAttack2 = false;
    }


    void chooseAttack()
    {
        int d = Random.Range(0,2);
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
        if (seeker.IsDone() && isDed == true)
        {
            seeker.StartPath(rb.position, new Vector3(-0.28f, 7.85f, 0f), OnPathComplete);
        }
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
        }

        FindPlayer();

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

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


       // transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

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
