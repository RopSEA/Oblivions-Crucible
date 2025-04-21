using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class enemyLeft : MonoBehaviour
{
    public static enemyLeft instance;
    public TextMeshProUGUI text;
    public int currEnemies = 8;
    public int currDef = 0;

    public void addDef()
    {
        currDef++;
        setNewText();
    }

    public void setNewText()
    {
        text.text = (currEnemies - currDef).ToString();
    }

    public void setText(int num)
    {
        currEnemies = num;
        text.text = num.ToString();
        currDef = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        text.text = "0";
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
