using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Classes : MonoBehaviour
{
    public int attack;
    public int defense;
    public int intelligence;
    public int movementSpeed;
    public float fireRate;

    public abstract void priSkill();
    public abstract void secSkill();



    public void addStren(int stren)
    {
        attack += stren;
    }
    public void addIntell(int intell)
    {
        intelligence += intell;
    }
    public void addStam(int end)
    {
       // movementSpeed += end;
    }
    public void addDef(int deff)
    {
        defense += deff;
    }

    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
