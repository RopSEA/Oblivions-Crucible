using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float DestroyTime = 3f;
    public Vector3 Offset = new Vector3(0, 0.5f, 0);
    public Vector3 intens = new Vector3(0.5f, 0, 0);
    void Start()
    {
        Destroy(gameObject, DestroyTime);

        transform.localPosition += Offset;
        transform.localPosition += new Vector3(Random.Range(-intens.x, intens.x), Random.Range(-intens.y, intens.y), Random.Range(-intens.z, intens.z));
    }

}
