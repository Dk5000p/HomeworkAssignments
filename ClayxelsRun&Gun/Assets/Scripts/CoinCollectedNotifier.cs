using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CoinCollectedNotifier : MonoBehaviour
{
    public static event Action<CoinCollectedNotifier> OnCoinCollected;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (OnCoinCollected != null)
        {
            OnCoinCollected(this);
        }
    }
}
