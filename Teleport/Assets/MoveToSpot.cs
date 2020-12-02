using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToSpot : MonoBehaviour
{
    public Transform player;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay(Collider collision)
    {
        if (collision.tag == "Player")

        {
            Debug.Log("In Square");
            player.position=Vector3.MoveTowards(player.position, target.position, 10f * Time.deltaTime);
        }
    }
}
