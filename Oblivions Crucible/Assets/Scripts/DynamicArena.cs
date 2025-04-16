using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DynamicArena : MonoBehaviour
{
    public GameObject tileGroup;
    public GameObject[][] pretileset;
    public GameObject[][] tileset;

    public GameObject[] tileHazards;

    public string[] patts;

    // Start is called before the first frame update
    void Start()
    {
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

        dynmArena();

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


    void Pattern()
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

    public int chooseHaz()
    {
        int temp = Random.Range(0, tileHazards.Length);
        return temp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
