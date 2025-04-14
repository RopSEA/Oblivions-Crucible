using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public bool start = false;
    public AnimationCurve curve;
    public float dur = 1f;


    // Update is called once per frame
    void Update()
    {
        if (start)
        {
             start = false;
            StartCoroutine(shake());
        }
    }

    IEnumerator shake()
    {
        Vector3 StartPos = transform.position;
        float time = 0f;

        while (time < dur)
        {
            time += Time.deltaTime;
            float stren = curve.Evaluate(time / dur);
            transform.position = StartPos + Random.insideUnitSphere;
            yield return null;
        }

        transform.position = StartPos;
    }
}
