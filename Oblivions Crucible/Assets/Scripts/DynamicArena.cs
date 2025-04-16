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
            tileset[i] = new GameObject[6];
            pretileset[i] = new GameObject[6];
            for (int j = 0; j < 6; j++)
            {
                tileset[i][j] = tiles[curr];
                pretileset[i][j] = tiles[curr];
                curr++;
            }
        }

        pattern();

    }


    public void pattern()
    {
        GameObject temp;
        char[][] pattern = new char[4][];
        for (int i = 0; i < 4; i++)
        {
            pattern[i] = new char[6];

            for (int j = 0; j < 6; j++)
            {
                pattern[i][j] = patts[i][j];
            }
        }



        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (pattern[i][j] == 'S')
                {
                    temp = Instantiate(tileHazards[chooseHaz()], tileset[i][j].transform.position, tileset[i][j].transform.rotation);
                    Destroy(tileset[i][j]);
                    tileset[i][j] = temp;
                }
            }
        }
    }

    public int chooseHaz()
    {
        int temp = 0;

        return temp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
