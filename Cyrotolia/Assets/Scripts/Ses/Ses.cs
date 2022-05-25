using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Ses
{
    

    public AudioClip Aclip; //oynatılacak ses

    public string isim; //oynatılacak sesin ismi

    [Range(0f,1f)]
    public float sesduzeyi; //oynatılacak sesin ses düzeyi

    [Range(0.1f, 3f)]
    public float pitch; //oynatılacak sesin pitch seviyesi

    public bool Loop; //loop seçeneği

    public bool Playonawake;

    [HideInInspector]
    public AudioSource Asource; //source ile bağlantı
}
