using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Ses
{
    

    public AudioClip Aclip; //oynatýlacak ses

    public string isim; //oynatýlacak sesin ismi

    [Range(0f,1f)]
    public float sesduzeyi; //oynatýlacak sesin ses düzeyi

    [Range(0.1f, 3f)]
    public float pitch; //oynatýlacak sesin pitch seviyesi

    public bool Loop; //loop seçeneði

    public bool Playonawake;

    [HideInInspector]
    public AudioSource Asource; //source ile baðlantý
}
