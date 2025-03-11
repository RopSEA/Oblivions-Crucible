using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{ 
    public coinType coinT;
    public enum coinType
    {
        Normal,
        Red,
        Blue
    }
}
