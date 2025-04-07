using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;  
    public Transform firePoint;
    public float fireRate = 0.5f; 

    private float nextFireTime = 0f;

    //Reference to RPG talk to stop firiing while dialog
    public RPGTalk rpgTalk;

    void Update()
    {
        if (rpgTalk != null && rpgTalk.isPlaying)
            return;
            
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)  
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}

