using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Round
{
    public roundKind r;
    public int req;
    public List<GameObject> enemyType = new List<GameObject>();
    public List<int> enemyTypeFreq = new List<int>();
    public enum roundKind
    {
        Normal,
        Shop,
        Boss
    }
}
