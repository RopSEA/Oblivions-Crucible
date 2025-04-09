using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Classes : MonoBehaviour
{
    public int attack;
    public int defense;
    public int movementSpeed;
    public float fireRate;

    public abstract void priSkill();
    public abstract void secSkill();

    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
