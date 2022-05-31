using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SesKontrol : MonoBehaviour
{

    public AudioSource S_idle;
    public AudioSource S_battle;

    // Update is called once per frame
    void Update()
    {
        if(!DusmanKontrol.Ses)
        {
            if (S_idle.isPlaying)
            {
                if (S_battle.isPlaying)
                {
                    S_battle.Stop();
                }
                return;
            }
            
            S_idle.Play();
        }
        else if(DusmanKontrol.Ses && !OyuncuKontrol.Oyundurdu)
        {
            if(S_battle.isPlaying)
            {
                return;
            }
            S_battle.Play();
        }
    }
}
