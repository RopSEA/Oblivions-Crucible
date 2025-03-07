using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public HealthBar hb;
    public coinManager coin;

    [SerializeField]
    private int hp = 100;

    public bool invulnerable = false;

    void Start()
    {
        hb.SetMax(hp);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void damage(int dam)
    {
        if (invulnerable)
        {
            return;
        }
        
        if (hp - dam > 0)
        {
            hp = hp - dam;
        }
        else
        {
            hp = 0;
            Debug.Log("Game Over");
        }

        hb.SetHealth(hp);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            damage(25);
        }
        if (collision.collider.tag == "Coin")
        {
                coin.collectCoin();
                Destroy( collision.gameObject);
        }
        
    }

}
