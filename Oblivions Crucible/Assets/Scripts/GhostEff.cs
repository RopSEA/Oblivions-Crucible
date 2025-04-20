using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEff : MonoBehaviour
{
    public float GhostDelay;
    private float ghostDelaySec;
    public GameObject ghost;
    public bool makeGhost = false;


    // Start is called before the first frame update
    void Start()
    {
        ghostDelaySec = GhostDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (makeGhost == false)
        {
            return;
        }

        if (ghostDelaySec > 0)
        {
            ghostDelaySec -= Time.deltaTime;
        }
        else
        {
            GameObject curr = Instantiate(ghost, transform.position, transform.rotation);
            Destroy(curr, 1f);
            ghostDelaySec = GhostDelay;
        }
    }
}
