using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineerClass : Classes
{
    public override void priSkill()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("PRIMARY Engineer");
        }
    }

    public override void secSkill()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("SECONDARY Engineer");
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        priSkill();
        secSkill();
    }
}
