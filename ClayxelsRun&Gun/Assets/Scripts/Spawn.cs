using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject[] spawns;
    public Transform[] locations;
  
    // Start is called before the first frame update
    void Start()
    {
        
       

        foreach(Transform location in locations)
        {
            Instantiate(spawns[Random.Range(0,spawns.Length)], new Vector3(location.position.x,location.position.y,location.position.z), Quaternion.identity);
        }
    }
    private void Update()
    {
    }


}
