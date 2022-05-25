using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OyuncuAnimasyonKontrol : MonoBehaviour
{
    private Animator Animasyon;
    void Start()
    {
        Animasyon = GetComponentInChildren<Animator>();
    }
    public void Hareket(float Hareketkod)
    {
        Animasyon.SetFloat("Hareket",Mathf.Abs(Hareketkod));
    }
    public void Zipla(bool ZiplaKod)
    {
        Animasyon.SetBool("Zipla", ZiplaKod);
    }
    public void Saldir()
    {
        Animasyon.SetTrigger("Saldir");
    }
    public void HasarAl()
    {
        Animasyon.SetTrigger("Hasaral");
    }
    public void Savunma(bool Savunkod)
    {
        Animasyon.SetBool("Savun",Savunkod);
    }
    public void Ol()
    {
        Animasyon.SetBool("Oldumu", true);
        Animasyon.SetTrigger("Ol");
    }
    public void YenidenDog()
    {
        Animasyon.SetBool("Oldumu", false);
        Animasyon.SetTrigger("YenidenDog");
    }
}
