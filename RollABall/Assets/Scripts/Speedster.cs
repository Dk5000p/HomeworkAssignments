using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedster : Hero
{
    public override void Attack()
    {
        Debug.Log("Attacking with speed.");
    }
    public override void Defend()
    {
        Debug.Log("Defending with running away.");
    }
}
