using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    public GameObject backgroundObj; //Assign in Inspector

    private float objectWidth;
    private float objectHeight;
    private float minX, minY, maxX, maxY;

    // Start is called before the first frame update
    void Start()
    {
        if (backgroundObj == null)
        {
            Debug.LogError("Background GameObject is not assigned in the Inspector!");
            return;
        }

         // Get the SpriteRenderer from the assigned background
        SpriteRenderer backgroundRenderer;
        if (!backgroundObj.TryGetComponent(out backgroundRenderer))
        {
            Debug.LogError("Background GameObject found, but no SpriteRenderer attached!");
            return;
        }

        // Grab background size
        float backgroundWidth = backgroundRenderer.bounds.extents.x;
        float backgroundHeight = backgroundRenderer.bounds.extents.y;

        // Grab player sprite size
        objectWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
        objectHeight = GetComponent<SpriteRenderer>().bounds.extents.y;

        float buffer = 0.05f; //Prevent Jitter
        // Define bounds based on background size
        minX = -backgroundWidth + objectWidth + buffer;
        minY = -backgroundHeight + objectHeight + buffer;
        maxX = backgroundWidth - objectWidth - buffer;
        maxY = backgroundHeight - objectHeight - buffer;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 viewPosition = transform.position;

        //Clamp player
        viewPosition.x = Mathf.Clamp(viewPosition.x, minX, maxX);
        viewPosition.y = Mathf.Clamp(viewPosition.y, minY, maxY);

        transform.position = viewPosition;
    }
}
