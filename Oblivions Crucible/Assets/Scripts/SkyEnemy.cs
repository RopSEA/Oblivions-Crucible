using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyEnemy : BasicEnemyMovement
{

    public GameObject bulletPrefab;
    public Transform firePoint;

    private GameObject enemy;
    private float dist;
    private bool isShoot = false;

    public void OnDestroy()
    {
        enemyLeft.instance.addDef();
    }
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
        AudioManager.instance.PlaySfx("hitE", true);

        if (floatingText)
        {
            ShowNumber(dam);
        }

        if (hp == 0)
        {
            Destroy(gameObject.transform.parent.gameObject);
            Destroy(gameObject);
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            DataPersistenceManager.instance.GameData.enemiesDefeated++;
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
    IEnumerator rangerMove()
    {
        Vector3 move = -player.position.normalized;
        int dis = 120;

        shoot();
        yield return new WaitForSeconds(0.5f);
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
        GameObject temp = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        temp.GetComponent<EnemyHomingBullet>().updateOwner(gameObject);
    }

    // Start is called before the first frame update
    void Start()
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
            i.renderer.sortingLayerName = "Sky";
        }
    }

    // Update is called once per frame
    void Update()
    {
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
}
