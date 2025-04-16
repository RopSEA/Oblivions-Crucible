using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    public GameObject spawnPoint;
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
    public GameObject hpBar;
    public GameObject BossIntro;
    private int temp;
    [SerializeField] private int enemiesPerWave = 7;

    public void spawnEnemies()
    {
        GameObject temp = null;
        int rand = Random.Range(0,8);
        int randy = Random.Range(0, 8);
        int freq;
        int enem;

        // Fix LATER:  better optimize
        if (enemys.Count > 0)
        {
            for (int i = 0; i < enemiesPerWave; i++) // 7 
            {
                if (enemys[i] != null)
                {
                    return;
                }
            }

            currDef += enemiesPerWave; // 7 
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
        for (int i = 0; i < enemiesPerWave; i++) // 7
        {
            freq = Random.Range(0, 99);
            enem = chooseEnemy(freq);
            rand = Random.Range(-12, 12);
            randy = Random.Range(-7, 7);

            //temp = Instantiate(rs[currRound].enemyType[enem], spawns[rand].position, transform.rotation);
            temp = Instantiate(spawnPoint,new Vector3(rand,randy,0), transform.rotation);
            temp = temp.GetComponent<betterSpawn>().spawnEnmy(rs[currRound].enemyType[enem]);
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
            SpawnBoss();
        }
    }

    public void SpawnBoss()
    {
        GameObject temp = null;


        if (enemys.Count > 0)
        {
            if (enemys[0] != null)
            {
                return;
            }

            currDef = 1;
            enemys.Clear();
            hpBar.SetActive(false);
            StartCoroutine(intermission(5));
        }

        if (currDef >= rs[currRound].req)
        {
            //Debug.Log("huh");
            return;
        }

        temp = Instantiate(rs[currRound].enemyType[0], spawns[2].position, spawns[2].rotation);
        temp.GetComponent<BasicEnemyMovement>().enabled = true;
        enemys.Add(temp);
        
        hpBar.SetActive(true);
        temp.SetActive(true);

    }

    IEnumerator shopRound()
    {
        shop.ShowShop();

        while (shop.Shop == true)
        {
            yield return null;
        }

        updateRound();
        isShop = false;
    }

    IEnumerator intermission(int Wait)
    {


        for (int i = Wait; i >= 0; i--)
        {
            waitText.text = "Time untill next round: " + i;
            yield return new WaitForSeconds(1f);
        }

        waitText.text = "";

        if (currRound + 1 <= rs.Length && rs[currRound + 1].r == Round.roundKind.Boss)
        {
            BossIntro.SetActive(true);
            yield return new WaitForSeconds(1.7f);
            BossIntro.SetActive(false);
        }
        updateRound();
        yield return null;
    }

    void Victory()
    {
        endText.text = "VICTORY!!";
        endText.color = Color.green;
        Time.timeScale = 0;
        SceneManager.LoadScene("TitleScreen");

    }

    public void Lose()
    {
        endText.text = "GAME OVER";
        endText.color = Color.red;
        Time.timeScale = 0;
        SceneManager.LoadScene("TitleScreen");
    }

    public void QuitRun()
    {
        SceneManager.LoadScene("TitleScreen");
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
