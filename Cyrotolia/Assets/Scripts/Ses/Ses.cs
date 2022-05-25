using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Ses
{
    

    public AudioClip Aclip; //oynat�lacak ses

    public string isim; //oynat�lacak sesin ismi

    [Range(0f,1f)]
    public float sesduzeyi; //oynat�lacak sesin ses d�zeyi

    [Range(0.1f, 3f)]
    public float pitch; //oynat�lacak sesin pitch seviyesi

    public bool Loop; //loop se�ene�i

    public bool Playonawake;

    [HideInInspector]
    public AudioSource Asource; //source ile ba�lant�
}
