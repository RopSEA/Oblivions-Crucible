using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerPylon : MonoBehaviour
{
    public GameObject floatingText;
    private SpriteRenderer renderer;
    public int hp = 50;



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
        AudioManager.instance.PlaySfx("hitE", true);

        // Show Number
        if (floatingText)
        {
            ShowNumber(dam);
        }


        if (hp == 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator redDamage()
    {
        float dur = 0.25f;
        float elapsedTime = 0f;
        int hitEffectAmount = Shader.PropertyToID("_HitEffectAmount");

        while (elapsedTime < dur)
        {
            elapsedTime += Time.deltaTime;

            float lerpedAmt = Mathf.Lerp(1f, 0f, (elapsedTime / dur));

            renderer.material.SetFloat(hitEffectAmount, lerpedAmt);
            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < dur)
        {
            elapsedTime += Time.deltaTime;

            float lerpedAmt = Mathf.Lerp(0f, 1f, (elapsedTime / dur));
            renderer.material.SetFloat(hitEffectAmount, lerpedAmt);
            yield return null;
        }

    }


    public void ShowNumber(int dam)
    {
        var go = Instantiate(floatingText, transform.position, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = dam.ToString();
    }


    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        int hitEffectAmount = Shader.PropertyToID("_HitEffectAmount");
        renderer.material.SetFloat(hitEffectAmount, 1);
        LaserManager.instance.addPylon();
    }

    private void OnDestroy()
    {
        LaserManager.instance.breakPylon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
