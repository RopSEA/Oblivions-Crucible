using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class coinManager : MonoBehaviour
{
    public TMP_Text text;
    private int coinCnt = 0;


    public void collectCoin()
    {
        coinCnt++;
        text.text = "$ " + coinCnt;

    }
    // Start is called before the first frame update
    void Start()
    {
        int coinCnt = 0;
    }
}
