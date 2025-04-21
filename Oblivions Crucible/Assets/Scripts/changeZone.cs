using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class changeZone : MonoBehaviour
{
    public GameObject[] zone;
    public GameObject[] zoneLight;

    public GameObject currZone;
    public  GameObject currLight;

    public GameObject winPrompt;
    public TextMeshProUGUI text;


    public GameObject transition;


    private GameObject Player;

    public int index = 0;

    public void changecurrZone()
    {
        Time.timeScale = 1f;
        // Screen Transiton
        StartCoroutine(transitions());


    }

    IEnumerator transitions()
    {
        transition.SetActive(true);
        yield return new WaitForSeconds(2f);
        text.text = "The Second crucible is Conqured!!!!\r\nWould you\r\nlike to continue?";
        winPrompt.SetActive(false);
        index++;
        Time.timeScale = 1f;
        zone[index].SetActive(true);
        zoneLight[index].SetActive(true);
        currZone.SetActive(false);
        currLight.SetActive(false);
        currZone = zone[index];
        currLight = zoneLight[index];
        Player.transform.position = new Vector3(0,0,0);

        transition.GetComponent<Animator>().SetBool("isDoneLoad", true);
        //yield return new WaitForSeconds(2f);

        gameObject.GetComponent<RoundManager>().spawnLazer.SetActive(true);
        gameObject.GetComponent<RoundManager>().cursor.SetActive(false);
        gameObject.GetComponent<RoundManager>().dynA = DynamicArena.instance;
        gameObject.GetComponent<RoundManager>().isDoneWaiting = true;
        gameObject.GetComponent<RoundManager>().isTransitionState = false;
        transition.SetActive(false);

        yield return new WaitForSeconds(1f);
        StartCoroutine(gameObject.GetComponent<RoundManager>().startZoneNew());

        yield break;
    }
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
