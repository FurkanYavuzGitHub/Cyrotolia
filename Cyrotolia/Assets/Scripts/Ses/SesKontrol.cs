using UnityEngine.Audio;
using System;
using UnityEngine;

public class SesKontrol : MonoBehaviour
{
    public Ses[] Sesler; //Ses class'ý ile baðlantý kurduk
    void Awake()
    {
        foreach (Ses N in Sesler) //Seskontrol objesine tüm seçenekleri ekliyoruz.
        {
            N.Asource = gameObject.AddComponent<AudioSource>();
            N.Asource.clip = N.Aclip;
            N.Asource.volume = N.sesduzeyi;
            N.Asource.pitch = N.pitch;
            N.Asource.loop = N.Loop;
            N.Asource.playOnAwake = N.Playonawake;
        }
    }

    void Start() //Baþlangýçta sesi oynat
    {
        Oynat("idle");
    }

    public void Oynat(string isim) //Seçenekteki sesi oynat
    {
        Ses N = Array.Find(Sesler, ses => ses.isim == isim);
        N.Asource.Play();

    }
    
    public void Durdur(string isim) //Seçenekteki sesi kapat
    {
        Ses N = Array.Find(Sesler, ses => ses.isim == isim);
        N.Asource.Stop();
    }
    
    public void idle()
    {
        //Savaþ dýþý oynatýlacak müzik
    }
    public void Savas()
    {
        //Savaþýrken oynatýlacak müzik
    }
}
