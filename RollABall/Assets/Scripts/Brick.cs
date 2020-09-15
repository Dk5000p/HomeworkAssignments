using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : Hero
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Attack()
    {
        Debug.Log("Attacking with fists.");
    }
    public override void Defend()
    {
        Debug.Log("Defending with body.");
    }
}
