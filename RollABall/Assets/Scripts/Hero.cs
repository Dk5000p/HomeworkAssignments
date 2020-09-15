using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hero:MonoBehaviour 
{
    public string heroName;
    private string secretIdentity;
    public string primaryPower;
    public abstract void Attack();
    public abstract void Defend();
    
}
