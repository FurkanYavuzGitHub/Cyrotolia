using UnityEngine.Audio;
using System;
using UnityEngine;

public class SesKontrol : MonoBehaviour
{
    public Ses[] Sesler; //Ses class'� ile ba�lant� kurduk
    void Awake()
    {
        foreach (Ses N in Sesler) //Seskontrol objesine t�m se�enekleri ekliyoruz.
        {
            N.Asource = gameObject.AddComponent<AudioSource>();
            N.Asource.clip = N.Aclip;
            N.Asource.volume = N.sesduzeyi;
            N.Asource.pitch = N.pitch;
            N.Asource.loop = N.Loop;
            N.Asource.playOnAwake = N.Playonawake;
        }
    }

    void Start() //Ba�lang��ta sesi oynat
    {
        Oynat("idle");
    }

    public void Oynat(string isim) //Se�enekteki sesi oynat
    {
        Ses N = Array.Find(Sesler, ses => ses.isim == isim);
        N.Asource.Play();

    }
    
    public void Durdur(string isim) //Se�enekteki sesi kapat
    {
        Ses N = Array.Find(Sesler, ses => ses.isim == isim);
        N.Asource.Stop();
    }
    
    public void idle()
    {
        //Sava� d��� oynat�lacak m�zik
    }
    public void Savas()
    {
        //Sava��rken oynat�lacak m�zik
    }
}
