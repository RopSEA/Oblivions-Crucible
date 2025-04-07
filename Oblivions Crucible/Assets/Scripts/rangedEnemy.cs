using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rangedEnemy : BasicEnemyMovement
{
    private GameObject enemy;
    public GameObject bulletPrefab;
    public Transform firePoint;

    [SerializeField]
    private float dist;
    private bool isShoot = false;

    public override void damage(int dam)
    {

        if (hp - dam > 0)
        {
            hp = hp - dam;
        }
        else
        {
            hp = 0;
        }

        StartCoroutine(redDamage());
        ShowHitEffect();

        if (hp == 0)
        {
            Destroy(gameObject);
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            DataPersistenceManager.instance.GameData.enemiesDefeated++;
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

    private void Awake()
    {
        gameObject.GetComponent<rangedEnemy>().enabled = true;

        if (isTest == true)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

    }

    private void Start()
    {
        enemy = this.gameObject;
        FindPlayer();
        enemy.SetActive(true);
    }

    void Update()
    {
        
        if (player == null)
        {
            Debug.Log("hi no player found");
            FindPlayer();
            return;
        }
        
        dist = Vector3.Distance(transform.position, player.transform.position);

        if (dist >= 3.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);   
        }
        else
        {
            if (isShoot == false)
            {
                isShoot = true;
                StartCoroutine(rangerMove());
            }
        }
    }

    IEnumerator rangerMove()
    {
        Vector3 move = -player.position.normalized;
        int dis = 120;

        shoot();
        yield return new WaitForSeconds(0.5f);
        shoot();
        yield return new WaitForSeconds(0.5f);

        //Dash

        while (dis > 0)
        {
            gameObject.transform.position += move * 100 * Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
            dis -= 10;
        }


        isShoot = false;
        yield return null;
    }

    void shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
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
}
