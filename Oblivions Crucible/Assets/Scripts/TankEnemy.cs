using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemy : BasicEnemyMovement
{
    private GameObject enemy;
    private bool isShake;
    private GameObject cam;


    public float dist;
    public GameObject shake;
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
        cam = GameObject.FindWithTag("MainCamera");
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
        
        dist = Vector3.Distance(transform.position, player.transform.position);

        if (dist >= 3.5)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            Debug.Log("were not far");
            if (isShake == false)
            {
                isShake = true;
                StartCoroutine(tankMove());
            }
        }

    }

    IEnumerator tankMove()
    {
        player.gameObject.GetComponent<PlayerMovement>().ChangeState(PlayerMovement.State.Disable);
        shake.SetActive(true);
        cam.GetComponent<ScreenShake>().start = true;
        AudioManager.instance.PlaySfx("lazerBig", false);
        yield return new WaitForSeconds(1.5f);
        player.gameObject.GetComponent<PlayerMovement>().ChangeState(PlayerMovement.State.Normal);
        
        shake.SetActive(false);

        yield return new WaitForSeconds(3f);
        isShake = false;
    }

    private void OnDestroy()
    {
        if (enemyLeft.instance != null)
        {
            enemyLeft.instance.addDef();
        }
        
        if (shake.activeSelf == true)
        {
            player.gameObject.GetComponent<PlayerMovement>().ChangeState(PlayerMovement.State.Normal);
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
}
