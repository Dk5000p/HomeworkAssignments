using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public bool isMoving;
    public Transform target;
    public Transform currentSquare;
    public float currentSquareNumber;
    public float targetSquareNumber;
    public int dieRoll;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dieRoll = DiceNumberTextScript.diceNumber;
        currentSquareNumber = GetComponent<SquareNumber>().square;
        targetSquareNumber = currentSquareNumber + dieRoll;
        if (GetComponent<SquareNumber>().square == targetSquareNumber)
        {
            target = GetComponent<SquareNumber>().transform;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, 100 * Time.deltaTime);
    }
}
