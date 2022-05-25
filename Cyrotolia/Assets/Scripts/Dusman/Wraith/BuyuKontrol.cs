using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyuKontrol : MonoBehaviour
{
    Vector3 Hedef;
    Vector3 Wraith;
    public float Hiz;
    public int BuyuHasari;
    private OyuncuKontrol Karakter;
    private bool GeriSekKontrol = false;

    void Start()
    {
        Hedef = FindObjectOfType<OyuncuKontrol>().OyuncuSprite.transform.position;
        Karakter = FindObjectOfType<OyuncuKontrol>();
        Wraith = FindObjectOfType<WraithKontrol>().transform.position;
    }

    void Update()
    {
        if (!GeriSekKontrol)
        {
            Ateset();
        }
    }

    #region Ateþ Et
    public void Ateset()
    {
        transform.position = Vector2.MoveTowards(transform.position, Hedef, Hiz * Time.deltaTime);
    }
    #endregion

    #region HasarVer
    private void OnCollisionEnter2D(Collision2D Hedef)
    {
        if (Hedef.collider.tag == "Karakter")
        {
            if (!OyuncuKontrol.B_SavunmaKontrol)
            {
                Karakter.HasarAl(BuyuHasari);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
                GeriSekKontrol = true;
            }
        }
    }
    #endregion
}
