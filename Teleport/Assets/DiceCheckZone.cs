using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCheckZone : MonoBehaviour
{
    Vector3 diceVelocity;
    // Start is called before the first frame update
    private void FixedUpdate()
    {
        diceVelocity = DiceScript.diceVelocity;
    }
    private void OnTriggerStay(Collider other)
    {
        if (diceVelocity.x == 0f && diceVelocity.y == 0f && diceVelocity.z == 0f)
        {
            switch (other.gameObject.name)
            {
                case "Side1":
                    DiceNumberTextScript.diceNumber = 6;
                    break;
                case "Side2":
                    DiceNumberTextScript.diceNumber = 5;
                    break;
                case "Side3":
                    DiceNumberTextScript.diceNumber = 4;
                    break;
                case "Side4":
                    DiceNumberTextScript.diceNumber = 3;
                    break;
                case "Side5":
                    DiceNumberTextScript.diceNumber = 2;
                    break;
                case "Side6":
                    DiceNumberTextScript.diceNumber = 1;
                    break;

            }
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
