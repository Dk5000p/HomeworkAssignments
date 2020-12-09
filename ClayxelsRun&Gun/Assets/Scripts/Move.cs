using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour, IPooledObject
{
    public float moveInput = 1;
    public float speed = 100;
    public Rigidbody rb;
    
    // Start is called before the first frame update
    public void OnObjectSpawn()
    {
        rb.velocity = new Vector3(moveInput * speed,rb.velocity.y, rb.velocity.z);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
       
        
    }
}
