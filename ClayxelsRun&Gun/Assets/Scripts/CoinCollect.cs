using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CoinCollect : MonoBehaviour
{
    public static event Action coinPickup;
    private void OnDisable()
    {
        coinPickup?.Invoke();
    }
}
