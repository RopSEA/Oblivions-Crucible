using UnityEngine;
using TMPro;


public class coinManager : MonoBehaviour
{
    public static coinManager instance; 
    public TMP_Text text;
    private int coinCnt = 0;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateCoinUI();
    }

    public void CollectCoin()
    {
        coinCnt++;
        UpdateCoinUI();
    }

    public bool SpendCoins(int amount)
    {
        if (coinCnt >= amount)
        {
            coinCnt -= amount;
            UpdateCoinUI();
            return true;
        }
        else
        {
            Debug.Log("Not enough coins!");
            return false;
        }
    }

    void UpdateCoinUI()
    {
        if (text != null)
            text.text = coinCnt.ToString();
    }

    public int GetCoinCount()
    {
        return coinCnt;
    }
}
