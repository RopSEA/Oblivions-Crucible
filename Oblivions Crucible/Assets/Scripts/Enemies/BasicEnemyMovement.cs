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
    public GameObject floatingText;
    public GameObject hitEffectPrefab;
    public GameObject coinPrefab;
    public SPUM_MatchingList sprite;
    public bool isTest;
    public Material hit;
    private GameObject enemy;

    public IEnumerator cor;

    public virtual void damage(int dam)
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
        AudioManager.instance.PlaySfx("hitE", true);
        // Show Number
        if (floatingText)
        {
            ShowNumber(dam);
        }
        

        if (hp == 0)
        {
            Destroy(gameObject);
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            DataPersistenceManager.instance.GameData.enemiesDefeated++;
        }
    }

    public virtual void ShowNumber(int dam)
    {
        var go = Instantiate(floatingText, transform.position, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = dam.ToString();
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




        List<MatchingElement> sprites = sprite.matchingTables;
        List<Material> materials = new List<Material>();
        int hitEffectAmount = Shader.PropertyToID("_HitEffectAmount");
        foreach (MatchingElement i in sprites)
        {
            i.renderer.material = hit;
            i.renderer.material.SetFloat(hitEffectAmount, 1);
        }

    }


    void Update()
    {

        //Debug.Log(player);
        
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
