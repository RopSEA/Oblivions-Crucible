using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerArrow : MonoBehaviour
{
    public Transform pivotPoint;
    public float rotmod;
    public float rotSpeed;

    private Transform ot;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies == null)
        {
            return null;
        }
        Transform nearestEnemy = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(currentPosition, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        return nearestEnemy;
    }
    // Update is called once per frame
    void Update()
    {
        ot = FindNearestEnemy();
        if (ot != null) 
        {
            Vector3 vectorToTarget = ot.position - pivotPoint.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotmod;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            pivotPoint.rotation = Quaternion.Slerp(pivotPoint.rotation, q, Time.deltaTime * rotSpeed);
        }
        
    }
}
