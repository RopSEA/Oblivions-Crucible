using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public List<string> relicsAcquired;
    public List<string> classesUnlocked;
    public int roundsBeat;
    public int bossesBeat;
    public int enemiesDefeated;
    public int deathCount;

    public bool tutorialDone;

    // Default constructor initializes default values when a new game starts
    public GameData() 
    {
        this.relicsAcquired = new List<string>();
        this.roundsBeat = 0;
        this.bossesBeat = 0;
        this.enemiesDefeated = 0;
        this.deathCount = 0;
        this.tutorialDone = false;
    }
}
