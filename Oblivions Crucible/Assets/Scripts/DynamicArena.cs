using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DynamicArena : MonoBehaviour
{
    public static DynamicArena instance;
    public GameObject tileGroup;
    public GameObject WarningT;
    public GameObject[][] pretileset;
    public GameObject[][] tileset;

    public List<GameObject> tileHazards = new List<GameObject>();

    public string[] patts;

    public bool debug = false;




    private void OnDestroy()
    {
        delHaz2();
    }

    // Start is called before the first frame update
    void Start()
    {

        if (debug == true)
        {
            return;
        }

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(instance);
            instance = this;
        }


        GameObject[] tiles = new GameObject[tileGroup.transform.childCount];
        int curr = 0;
        
        for (int i = 0; i < tileGroup.transform.childCount; i++)
        {
           tiles[i] = tileGroup.transform.GetChild(i).gameObject;
        }
        
        tileset = new GameObject[4][];
        pretileset = new GameObject[4][];

        for (int i = 0; i < 4; i++)
        {
            tileset[i] = new GameObject[7];
            pretileset[i] = new GameObject[7];
            for (int j = 0; j < 7; j++) 
            {
                tileset[i][j] = tiles[curr];
                pretileset[i][j] = tiles[curr];
                curr++;
            }
        }
    }


    public void delHaz() 
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (patts[i][j] == 'S')
                {
                    Destroy(tileset[i][j]);
                    tileset[i][j] = pretileset[i][j];
                }
            }
        }

        Pattern();
        Warning();
    }

    public void delHaz2()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (patts[i][j] == 'S')
                {
                    Destroy(tileset[i][j]);
                    tileset[i][j] = pretileset[i][j];
                }
            }
        }
    }


    public void dynmArena()
    {
        GameObject temp;
        char[][] pattern = new char[4][];
        for (int i = 0; i < 4; i++)
        {
            pattern[i] = new char[7];

            for (int j = 0; j < 7; j++)
            {
                pattern[i][j] = patts[i][j];
            }
        }



        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (pattern[i][j] == 'S')
                {
                    temp = Instantiate(tileHazards[chooseHaz()], tileset[i][j].transform.position, tileset[i][j].transform.rotation);
                    tileset[i][j] = temp;
                }
            }
        }

        AstarPath.active.Scan();
    }


    public void Pattern()
    {
        int rand = Random.Range(0, 2);

        if (rand == 0)
        {
            patts[0] = "###S###";
            patts[1] = "#S###S#";
            patts[2] = "SS###SS";
            patts[3] = "###S###";
        }
        if (rand == 1)
        {
            patts[0] = "S#####S";
            patts[1] = "#SS#SS#";
            patts[2] = "#####SS";
            patts[3] = "#SS#SS#";
        }
        if (rand == 2)
        {
            patts[0] = "#S###S#";
            patts[1] = "#S###S#";
            patts[2] = "#######";
            patts[3] = "SS###SS";
        }

        // ALERT CHANGE some sort of flash
    }

    public void Warning()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (patts[i][j] == 'S')
                {
                    Instantiate(WarningT, tileset[i][j].transform.position, tileset[i][j].transform.rotation);
                }
            }
        }
    }

    public int chooseHaz()
    {
        int temp = Random.Range(0, tileHazards.Count);
        return temp;
    }

    public void addHazard(GameObject hazard)
    {
        // check if is hazard first
        tileHazards.Add(hazard);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
