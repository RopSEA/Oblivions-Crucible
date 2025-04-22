using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class DummyTarget : MonoBehaviour
{
    public int health = 50;
    public GameObject coinPrefab;

    private Animator animator;
    private bool isHit = false;
    private TutorialManager tutorialManager;
    public GameObject floatingText;
    private SpriteRenderer renderer; 

    [Header("Tutorial Settings")]
    public static int requiredKillsToAdvance = 4;

    [Tooltip("Tracks how many dummies have been destroyed.")]
    public static int totalDummiesKilled = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        tutorialManager = FindObjectOfType<TutorialManager>();
        renderer = GetComponent<SpriteRenderer>();
        int hitEffectAmount = Shader.PropertyToID("_HitEffectAmount");
        renderer.material.SetFloat(hitEffectAmount, 1);

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

    public void damage(int amount)
    {
        if (tutorialManager != null && !tutorialManager.HasMoved())
            return;

        if (isHit) return;

        isHit = true;
        health -= amount;
        StartCoroutine(redDamage());
        AudioManager.instance.PlaySfx("hitE", false);
        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }

        if (floatingText)
        {
            ShowNumber(amount);
        }

        if (health <= 0)
        {
            Die();
        }
        else
        {
            Invoke(nameof(ResetHit), 0.2f);
        }
    }

    public void ShowNumber(int dam)
    {
        var go = Instantiate(floatingText, transform.position, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = dam.ToString();
    }

    void ResetHit()
    {
        isHit = false;
    }

    void Die()
    {
        if (coinPrefab != null)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }

        totalDummiesKilled++;

        if (totalDummiesKilled % requiredKillsToAdvance == 0 && tutorialManager != null)
        {
            tutorialManager.OnDummyDestroyed();
        }

        Destroy(gameObject);
    }
}
