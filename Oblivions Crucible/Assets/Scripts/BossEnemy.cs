using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : BasicEnemyMovement
{
    public GameObject hpBar;

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


    void Start()
    {
        if (hpBar == null)
        {

            hpBar = GameObject.FindWithTag("BossBar");
            hpBar.SetActive(true);
            hpBar.GetComponent<HealthBar>().SetMax(hp);
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
    }
}
