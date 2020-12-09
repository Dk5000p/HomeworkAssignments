using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementSystemUsingEvents : MonoBehaviour
{
    public AudioSource sound;
    // Start is called before the first frame update
    void Start()
    {
        CoinCollectedNotifier.OnCoinCollected += playSound;
     
    }
    private void playSound(CoinCollectedNotifier c)
    {
        sound.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
