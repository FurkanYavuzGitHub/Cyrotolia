using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoyluAnimasyonKontrol : MonoBehaviour
{
    private Animator Animasyon;

    private void Start()
    {
        Animasyon = GetComponentInChildren<Animator>();
    }
    public void Hareket()
    {
        Animasyon.SetBool("Hareket", true);
    }
    public void Dur()
    {
        Animasyon.SetBool("Hareket", false);
    }
}
