using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerStats
{
    public int Strength = 10;  

    public int Vitality = 10;
  
    public int Stamina = 10;  
    public int Intelligence = 10;
    public int Defense = 10;  

    public List<string> OwnedUpgrades = new List<string>(); 

    public PlayerStats() { }

   
}
