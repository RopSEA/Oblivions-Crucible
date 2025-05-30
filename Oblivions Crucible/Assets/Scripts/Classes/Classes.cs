using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Classes : MonoBehaviour
{
    public int attack;
    public int defense;
    public int intelligence;
    public int vit;
    public int movementSpeed;
    public float fireRate;


    public float DamageMultiplier;
    public float Lifesteal;
    public float ReviveOnce;


    public abstract void priSkill();
    public abstract void secSkill();

    public void addStren(int stren)
    {
        attack += stren;
    }
    public void addVit(int vits)
    {
        vit += vits;
    }
    public void addIntell(int intell)
    {
        intelligence += intell;
    }
    public void addStam(int end)
    {
        movementSpeed += end;
        gameObject.GetComponent<PlayerMovement>().upgradeStam(end);
    }
    public void addDef(int deff)
    {
        defense += deff;
    }

    public void addDmgMult(float DamageMult)
    {
        DamageMultiplier = DamageMult;
        attack = attack * (int)DamageMultiplier;
    }

    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
