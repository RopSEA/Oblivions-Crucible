using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorSc : MonoBehaviour
{
    private Vector3 hotspot = Vector3.zero;
    public Sprite sprite;
    public bool debug = false;
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;

        if (debug == true) 
        {
            Cursor.visible = false;
        }
        
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition + hotspot;
    }

    public void SwapCursor(Sprite newSprite)
    {
        if (newSprite != null)
        {
            image.sprite = newSprite; 
        }
    }

}
