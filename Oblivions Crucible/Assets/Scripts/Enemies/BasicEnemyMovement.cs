using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;

public class BasicEnemyMovement : MonoBehaviour
{
    public float speed;
    public int hp;
    public Transform player;
    public GameObject hitEffectPrefab;
    public GameObject coinPrefab;
    public SPUM_MatchingList sprite;
    public bool isTest;
    private GameObject enemy;

    public void damage(int dam)
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

    private void Awake()
    {
        gameObject.GetComponent<BasicEnemyMovement>().enabled = true;

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

        Debug.Log(player);
        
        if (player == null)
        {
            Debug.Log("hi no player found");
            FindPlayer();
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        
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
