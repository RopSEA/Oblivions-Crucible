using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

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



    private Vector3 h = new Vector3(-8.537104f, -2.552133f, -0.1259285f);
    private Vector3 v = new Vector3(-9.435257f, -3.450285f, -0.1259285f);

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
        List<Color> c = new List<Color>();
        SpriteRenderer curr;
        int j = 0;



        foreach (MatchingElement i in sprites)
        {

            curr = i.renderer;
            c.Add(new Color());
            c[j] = curr.color;
            curr.color = Color.red;
            j++;
        }

        j = 0;
        yield return new WaitForSeconds(0.2f);

        foreach (MatchingElement i in sprites)
        {
            curr = i.renderer;
            curr.color = c[j];
            j++;
        }
        sprites[j - 1].renderer.color = Color.white;

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
            cam.GetComponent<ScreenShake>().start = true;

            Destroy(temp, 1f);
            Destroy(temp2, 1f);
        }
        else if (rand % 2 == 0)
        {
            temp = Instantiate(LazerV, v, transform.rotation);
            yield return new WaitForSeconds(wait);
            cam.GetComponent<ScreenShake>().start = true;
            Destroy(temp, 1f);
        }
        else if (rand % 2 != 0)
        {
            temp = Instantiate(LazerH , h, transform.rotation);
            yield return new WaitForSeconds(wait);
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


    void Start()
    {
        if (hpBar == null)
        {

            hpBar = GameObject.FindWithTag("BossBar");
            hpBar.SetActive(true);
            hpBar.GetComponent<HealthBar>().SetMax(hp);
            hpMax = hp;
        }

        FindPlayer();
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


        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        if (isAttack2 == true || isAttack1 == true)
        {
            return;
        }
        chooseAttack();
    }
}
