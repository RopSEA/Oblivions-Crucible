using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageClass : Classes
{
    public override void priSkill()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("PRI SKILL");

        }
    }

    public override void secSkill()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("SEC SKILL");
        }

    }

    void Start()
    {
        
    }

    void Update()
    {
        priSkill();
        secSkill();
    }
}
