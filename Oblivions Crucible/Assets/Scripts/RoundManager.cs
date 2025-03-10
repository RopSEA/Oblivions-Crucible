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
    private int currRound;
    private int currDef;

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
            //Debug.Log("Basic Round");
            spawnEnemies();
        }
        if (round[curr] == roundKind.Shop)
        {
            //Debug.Log("ShopNow");
            roundText.text = "Buy Round " + (currRound + 1);
        }
        if (round[curr] == roundKind.Boss)
        {
           // Debug.Log("BOSS");
        }
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


    void updateRound()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        determineRound(currRound);
    }
}
