using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public float activeTime = 2f;
    public Color activeColor = Color.red;

    private SpriteRenderer sr;
    private BoxCollider2D col;
    private bool isActive = false;

    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();

        if (sr == null)
            Debug.LogError("SpriteRenderer missing from laser prefab!");
        if (col == null)
            Debug.LogError("BoxCollider2D missing from laser prefab!");

        StartCoroutine(FireLaser());
    }

    IEnumerator FireLaser()
    {
        // Activate
        sr.color = activeColor;
        col.enabled = true;
        isActive = true;
        yield return new WaitForSeconds(activeTime);

        Destroy(gameObject); 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive && other.CompareTag("Player"))
        {
            other.GetComponent<HealthSystem>().preCalcTakeDamage(25);
        }
    }
}
