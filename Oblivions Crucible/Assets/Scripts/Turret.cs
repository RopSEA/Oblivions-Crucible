using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform pivotPoint;
    public float fireRate = 0.5f;
    public float rotmod;

    private float nextFireTime = 0f;

    public float rotSpeed;
    private Vector3 dir;
    private Quaternion rot;
    private GameObject ot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ot != null)
        {
            Vector3 vectorToTarget = ot.transform.position - pivotPoint.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotmod;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            pivotPoint.rotation = Quaternion.Slerp(pivotPoint.rotation, q, Time.deltaTime * rotSpeed);
        }
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (Time.time >= nextFireTime)
            {
                ot = other.gameObject;

                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other = null;
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
