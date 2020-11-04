using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : GAction
{
    public AudioSource sound;
    public override bool PrePerform()
    {

        return true;
    }

    public override bool PostPerform()
    {
      
            sound.Play();
            
        
        
        return true;
    }
}
