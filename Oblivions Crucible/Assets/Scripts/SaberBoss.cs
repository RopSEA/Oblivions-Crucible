using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.U2D;
using static Unity.Burst.Intrinsics.X86.Avx;

public class SaberBoss : BasicEnemyMovement
{
    public GameObject hpBar;
    public GameObject bulletPre;
    public GameObject bulletPre2;
    public GameObject saber;
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
    public float RotSpeed = 3f;
    public Transform rotatePoint;

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
                if (i.renderer == null)
                {
                    continue;
                }
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
                if (i.renderer == null)
                {
                    continue;
                }
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
    IEnumerator Spinny()
    {
        float comp = 0;
        while (comp < 360)
        {
            rotatePoint.RotateAround(rotatePoint.position, Vector3.forward, 1);
            comp += 0.0033f;
            yield return new WaitForSeconds(0.001f);
        }

        yield break;
    }
    IEnumerator firstMove()
    {
        int rot = 0;
        int comp = 0;
        GameObject[] jemp = new GameObject[8];
        IEnumerator curr = Spinny();
        //StartCoroutine(curr);
        while (comp < 20)
        {
            for (int i = 0; i < 8; i++)
            {

                jemp[i] = Instantiate(bulletPre, transform.position, transform.rotation, rotatePoint);
                jemp[i].GetComponent<EnemyHomingBullet>().updateOwner(gameObject);
                jemp[i].GetComponent<EnemyHomingBullet>().updateDir(rot);
                rot += 45;
            }
            
            rot = 0;
            comp++;
            yield return new WaitForSeconds(0.3f);
            /*    
            for (int i = 0; i < 8; i++)
            {

               // jemp[i].transform.parent = null;
            }
            */
        }
        StopCoroutine(curr);

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
        saber.SetActive(true);

        StartCoroutine(secCool());
    }

    IEnumerator secCool()
    {
        int rot = 0;
        int comp = 0;

        float time = 0;
        while (time <= 100)
        {
            saber.transform.RotateAround(saber.transform.position, Vector3.forward, RotSpeed);
            time += 0.1f;
            yield return new WaitForSeconds(0.001f);
        }
        saber.transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector2(0, 0));
        saber.SetActive(false);

        yield return new WaitForSeconds(4f);
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
            if (i.renderer == null)
            {
                continue;
            }
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
