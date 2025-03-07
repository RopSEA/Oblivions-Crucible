using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyMovement : MonoBehaviour
{
    public float speed;
    public int hp;
    public GameObject player;
    public GameObject hitEffectPrefab;
    public GameObject coinPrefab;
    public SPUM_MatchingList sprite;

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

    void Start()
    {

    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }
}
