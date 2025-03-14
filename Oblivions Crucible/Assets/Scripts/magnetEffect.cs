using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magnetEffect : MonoBehaviour
{
    public int speed;
    public Transform pref;
    public Transform coin;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            pref = collision.gameObject.transform;
            coin.position = Vector3.MoveTowards(coin.position, pref.transform.position, speed * Time.deltaTime);
        }
        
    }
}
