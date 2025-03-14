using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundManager : MonoBehaviour
{
    public Round[] rs;
    public List<Transform> spawns = new List<Transform>();
    public List<GameObject> enemys = new List<GameObject>();
    public TMP_Text roundText;
    public TMP_Text waitText;
    public TMP_Text endText;
    private int currRound;
    private int currDef;
    private bool isShop;
    public ShopDisplay shop;
    private int temp;

    public void spawnEnemies()
    {
        GameObject temp = null;
        int rand = Random.Range(0,8);
        int freq;
        int enem;
        // Fix LATER:  better optimize
        if (enemys.Count > 0)
        {
            for (int i = 0; i < 5; i++)
            {
                if (enemys[i] != null)
                {
                    return;
                }
            }

            currDef += 5;
            enemys.Clear();

            if (currDef >= rs[currRound].req)
            {
                StartCoroutine(intermission(5));
                return;
            }

            return;
        }
        
        if (currDef >= rs[currRound].req)
        {
            //Debug.Log("huh");
            return;
        }

        // Spawn Enemies
        for (int i = 0; i < 5; i++)
        {
            freq = Random.Range(0, 99);
            enem = chooseEnemy(freq);
            rand = Random.Range(0, 8);

            Debug.Log(enem + " nfiuwn " + rand);
            temp = Instantiate(rs[currRound].enemyType[enem], spawns[rand].position, spawns[rand].rotation);
            temp.GetComponent<BasicEnemyMovement>().enabled = true;
            enemys.Add(temp);
        }
    }

    int chooseEnemy(int freq)
    {
        int len = rs[currRound].enemyTypeFreq.Count;
        int curr;

        for (int i = len - 1 ; i >= 0; i--)
        {
            curr = 100 - rs[currRound].enemyTypeFreq[i];
            if (freq + 1 >= curr)
            {
                return i;
            }
        }

        return 0;
    }
    void determineRound(int curr)
    {
        

        if (rs[currRound].r == Round.roundKind.Normal)
        {
            spawnEnemies();
        }
        if (rs[currRound].r == Round.roundKind.Shop)
        {
            //Debug.Log("ShopNow");
            roundText.text = "Buy Round " + (currRound + 1);

            if (isShop == false)
            {
                isShop = true;
                StartCoroutine(shopRound());
            }
            

        }
        if (rs[currRound].r == Round.roundKind.Boss)
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

        if (currRound + 1 >= rs.Length)
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
    void Awake()
    {
       
       // DontDestroyOnLoad(this.gameObject);
        
        roundText.text = "Round " + (currRound + 1);
        waitText.text = "";
        currRound = 0;
        currDef = 0;

    }

    // Update is called once per frame
    void Update()
    {

        determineRound(currRound);
    }
}
