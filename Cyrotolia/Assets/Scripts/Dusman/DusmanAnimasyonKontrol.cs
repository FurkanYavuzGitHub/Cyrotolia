using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DusmanAnimasyonKontrol : MonoBehaviour
{
    private Animator Animasyon;
    void Start()
    {
        Animasyon = GetComponentInChildren<Animator>();
    }

    public void Saldir()
    {
        Animasyon.SetTrigger("Saldir");
    }
    
    public void Hareket()
    {
        Animasyon.SetBool("Hareket", true);
    }

    public void Dur()
    {
        Animasyon.SetBool("Hareket", false);
    }

    public void HasarAl()
    {
        Animasyon.SetTrigger("Hasaral");
    }
    public void Ol()
    {
        Animasyon.SetBool("Hareket", false);
        Animasyon.SetTrigger("Ol");
    }

    //Golem
    public void SinirlenVeHareketet()
    {
        Animasyon.SetBool("Sinirlendimi", true);
        Animasyon.SetBool("Hareket", true);
    }

    public void SinirliDur()
    {
        Animasyon.SetBool("Sinirlendimi", true);
        Animasyon.SetBool("Hareket", false);
    }
    public void Sakinles()
    {
        Animasyon.SetBool("Sinirlendimi", false);
    }
}
