using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesperationLazer : MonoBehaviour
{
 
    private int childs;
    private GameObject[] lazers;
    private LazerBoss temp;

    public void SetLazer(LazerBoss laz)
    {
        temp = laz;
    }

    void Awake()
    {
        childs = gameObject.transform.childCount;
        lazers = new GameObject[childs];
        int curr = 0;

        for (int i = 0; i < childs; i++)
        {
            lazers[i] = gameObject.transform.GetChild(i).gameObject;
            lazers[i].SetActive(false);
        }


        StartCoroutine(shootLazers());
    }


    IEnumerator shootLazers()
    {

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < childs; i++ )
        {
            lazers[i].transform.parent = null;
            lazers[i].SetActive(true);
            AudioManager.instance.PlaySfx("lazerBig", false);
            yield return new WaitForSeconds(.4f);
        }

        AudioManager.instance.PlaySfx("lazerBig", false);

        yield return new WaitForSeconds(1f);
        temp.setDone();
        for (int i = 0; i < childs; i++)
        {
            Destroy(lazers[i]);
       
        }
        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
