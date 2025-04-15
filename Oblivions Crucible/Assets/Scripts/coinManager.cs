using UnityEngine;
using TMPro;
using static Coin;


public class coinManager : MonoBehaviour
{
    public static coinManager instance;
    public GameObject coinEffect;
    public TMP_Text text;
    public int coinCnt = 0;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateCoinUI();
    }

    public void CollectCoin(GameObject coin)
    {
        coinType temp = coin.GetComponent<Coin>().coinT;
        AudioManager.instance.PlaySfx("Coin");
        if (temp == coinType.Normal)
        {
            CollectYellowCoin(coin);
        }
        if (temp == coinType.Red)
        {
            CollectRedCoin(coin);
        }
        if (temp == coinType.Blue)
        {
            CollectBlueCoin(coin);
        }

    }

    public void CollectYellowCoin(GameObject coin)
    {
        showCollectEffect(coin);
        coinCnt++;
        UpdateCoinUI();
        
    }

    public void CollectRedCoin(GameObject coin)
    {
        showCollectEffect(coin);
        coinCnt += 5;
        UpdateCoinUI();

    }

    public void CollectBlueCoin(GameObject coin)
    {
        showCollectEffect(coin);
        coinCnt += 10;
        UpdateCoinUI();

    }

    public void showCollectEffect(GameObject coin)
    {
        if (coinEffect != null)
        {
            GameObject effect = Instantiate(coinEffect, coin.transform.position, Quaternion.identity);
            Destroy(effect, 1f);
        }
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
