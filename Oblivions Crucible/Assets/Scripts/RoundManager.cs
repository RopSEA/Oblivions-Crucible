using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundManager : MonoBehaviour
{

    public List<roundKind> round = new List<roundKind>();
    public List<int> req = new List<int>();
    public List<Transform> spawns = new List<Transform>();
    public List<GameObject> enemys = new List<GameObject>();
    public GameObject enemy;
    public TMP_Text roundText;
    public TMP_Text waitText;
    public TMP_Text endText;
    private int currRound;
    private int currDef;
    private bool isShop;
    public ShopDisplay shop;

    public enum roundKind
    {
        Normal,
        Shop,
        Boss
    }

    public void spawnEnemies()
    {
        GameObject temp = null;
        // Fix LATER:  better optimize
        if (enemys.Count > 0)
        {
            for (int i = 0; i < spawns.Count; i++)
            {
                if (enemys[i] != null)
                {
                    return;
                }
            }

            currDef += spawns.Count;
            enemys.Clear();

            if (currDef >= req[currRound])
            {
                StartCoroutine(intermission(5));
                return;
                //updateRound();
            }

            return;
        }
        
        if (currDef >= req[currRound])
        {
            
            return;
        }

        for (int i = 0; i < spawns.Count; i++)
        {
            temp = Instantiate(enemy, spawns[i].position, spawns[i].rotation);
            enemys.Add(temp);
        }
    }
    void determineRound(int curr)
    {
        

        if (round[curr] == roundKind.Normal)
        {
            spawnEnemies();
        }
        if (round[curr] == roundKind.Shop)
        {
            //Debug.Log("ShopNow");
            roundText.text = "Buy Round " + (currRound + 1);

            if (isShop == false)
            {
                isShop = true;
                StartCoroutine(shopRound());
            }
            

        }
        if (round[curr] == roundKind.Boss)
        {
           // Debug.Log("BOSS");
        }
    }

    IEnumerator shopRound()
    {
        shop.ShowShop();

        while (shop.Shop == true)
        {
            yield return null;
        }

        updateRound();
    }

    IEnumerator intermission(int Wait)
    {


        for (int i = Wait; i >= 0; i--)
        {
            waitText.text = "Time untill next round: " + i;
            yield return new WaitForSeconds(1f);
        }

        waitText.text = "";
        updateRound();
        yield return null;
    }

    void Victory()
    {
        endText.text = "VICTORY!!";
        endText.color = Color.green;
        Time.timeScale = 0;
    }

    public void Lose()
    {
        endText.text = "GAME OVER";
        endText.color = Color.red;
        Time.timeScale = 0;
    }


    void updateRound()
    {

        if (currRound + 1 >= round.Count)
        {
            Victory();
            return;
        }
        Debug.Log("FINISHED ROUND");

        currRound++;
        roundText.text = "Round " + (currRound + 1);
        currDef = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        roundText.text = "Round " + (currRound + 1);
        waitText.text = "";
        currRound = 0;

        determineRound(currRound);
    }

    // Update is called once per frame
    void Update()
    {
        determineRound(currRound);
    }
}
