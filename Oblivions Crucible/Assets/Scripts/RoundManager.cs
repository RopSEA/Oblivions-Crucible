using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

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
    public bool isDoneWaiting = false;
    public ShopDisplay shop;
    public GameObject hpBar;
    public GameObject BossIntro;
    public DynamicArena dynA;
    public GameObject LosePrompt;
    public GameObject WinPrompt;
    public GameObject realWinPrompt;
    public GameObject cursor;
    public GameObject spawnLazer;
    private GameObject player;

    private int roundsSurvivedThisRun = 0;
    private bool tookDamageThisRun = false;

    public int EveryOther = 10;
    private int temp;
    public bool isTransitionState;
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
            rand = Random.Range(-14, 23);
            randy = Random.Range(-9, 9);

            //temp = Instantiate(rs[currRound].enemyType[enem], spawns[rand].position, transform.rotation);
            temp = Instantiate(spawnPoint,new Vector3(rand,randy,0), transform.rotation);
            Debug.Log(enem);
            temp = temp.GetComponent<betterSpawn>().spawnEnmy(rs[currRound].enemyType[enem]);
            //temp.GetComponent<BasicEnemyMovement>().enabled = true;
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
        DynamicArena.instance.dynmArena();
        updateRound();
        isShop = false;
    }

    IEnumerator shopBoss()
    {
        shop.ShowShop();

        while (shop.Shop == true)
        {
            yield return null;
        }
        DynamicArena.instance.dynmArena();
        updateRound();
        isShop = false;
    }

    IEnumerator intermission(int Wait)
    {

        DynamicArena.instance.delHaz();

        for (int i = Wait; i >= 0; i--)
        {
            waitText.text = "Time untill next round: " + i;
            yield return new WaitForSeconds(1f);
        }

        waitText.text = "";

        if (currRound + 1 >= rs.Length)
        {
            updateRound();
            yield break;
        }

        if (rs[currRound + 1].r != Round.roundKind.Shop )
        {
            DynamicArena.instance.dynmArena();
        }

        if (currRound + 1 <= rs.Length && rs[currRound + 1].r == Round.roundKind.Boss)
        {
            shop.ShowShop();

            while (shop.Shop == true)
            {
                yield return null;
            }
            isShop = false;

            BossIntro.SetActive(true);
            yield return new WaitForSeconds(1.7f);
            BossIntro.SetActive(false);
        }
        updateRound();
        yield break;
    }

    void realVictory()
    {
        cursor.SetActive(true);
        isTransitionState = true;
        Time.timeScale = 0f;
        realWinPrompt.SetActive(true);
    }

    void Victory()
    {
        cursor.SetActive(true);
        isTransitionState = true;
        Time.timeScale = 0f;
        WinPrompt.SetActive(true);
    }

    public void Lose()
    {
        cursor.SetActive(true);
        isTransitionState = true;
        Time.timeScale = 0f;
        LosePrompt.SetActive(true);
    }

    public void QuitRun()
    {
        SceneManager.LoadScene("TitleScreen");
    }


    public void updateRound()
    {
        if ((currRound + 1) >= rs.Length)
        {
            realVictory();
            return;
        }
        else if ((currRound + 1) % EveryOther == 0 && isDoneWaiting == false)
        {
            Victory();
            return;
        }

        Debug.Log("FINISHED ROUND");

        currRound++;
        roundText.text = "Round " + (currRound + 1);
        currDef = 0;
        isDoneWaiting = false;
        gameObject.GetComponent<enemyLeft>().setText(rs[currRound].req);

        GameData data = DataPersistenceManager.instance.GameData;
        data.roundsBeat++;

        // Track rounds survived in this run
        roundsSurvivedThisRun++;

        // Check for relic unlocks
        CheckStatBasedRelicUnlocks(data);

        // Save game after any unlocks
        DataPersistenceManager.instance.SaveGame();
    }
    private void CheckStatBasedRelicUnlocks(GameData data)
    {
        List<string> relics = data.relicsAcquired;

        TryUnlock("Heartroot Amulet", roundsSurvivedThisRun >= 5, relics);
        TryUnlock("Moonfang Amulet", currRound + 1 >= 10 && !tookDamageThisRun, relics);
        TryUnlock("Amulet of Vital Bloom", currRound + 1 >= 7 && !tookDamageThisRun, relics);
    }

    private void TryUnlock(string relicName, bool condition, List<string> unlocked)
    {
        if (condition && !unlocked.Contains(relicName))
        {
            unlocked.Add(relicName);
            Debug.Log($"Unlocked relic: {relicName}!");
        }
    }

    public void FlagDamageTaken()
    {
        tookDamageThisRun = true;
    }



    public IEnumerator startZone()
    {
        //Beam of light entrance

        player.GetComponent<PlayerMovement>().ChangeState(PlayerMovement.State.Disable);
        spawnLazer.SetActive(true);
        yield return new WaitForSeconds(1f);
        spawnLazer.SetActive(false);
        player.GetComponent<PlayerMovement>().ChangeState(PlayerMovement.State.Normal);

        DynamicArena.instance.Pattern();
        DynamicArena.instance.Warning();
        for (int i = 5; i >= 0; i--)
        {
            waitText.text = "Round will start in: " + i;
            yield return new WaitForSeconds(1f);
        }
        waitText.text = "";

        DynamicArena.instance.dynmArena();
        isTransitionState = false;
        gameObject.GetComponent<enemyLeft>().setText(rs[0].req);
        yield break;
    }

    public IEnumerator startZoneNew()
    {
        //Beam of light entrance
        player.GetComponent<PlayerMovement>().ChangeState(PlayerMovement.State.Disable);
        yield return new WaitForSeconds(1f);
        spawnLazer.SetActive(false);
        player.GetComponent<PlayerMovement>().ChangeState(PlayerMovement.State.Normal);


        DynamicArena.instance.Pattern();
        DynamicArena.instance.Warning();
        for (int i = 5; i >= 0; i--)
        {
            waitText.text = "New zone Round Starts in: " + i;
            yield return new WaitForSeconds(1f);
        }
        waitText.text = "";

        DynamicArena.instance.dynmArena();
        isTransitionState = false;
        updateRound();
        yield break;
    }

    // Start is called before the first frame update
    void Awake()
    {

        // DontDestroyOnLoad(this.gameObject);
        player = GameObject.FindWithTag("Player");
        roundText.text = "Round " + (currRound + 1);
        waitText.text = "";
        currRound = 0;
        currDef = 0;
        StartCoroutine(startZone());

    }

    // Update is called once per frame
    void Update()
    {
        if (isTransitionState == true) return;

        determineRound(currRound);
    }
}
