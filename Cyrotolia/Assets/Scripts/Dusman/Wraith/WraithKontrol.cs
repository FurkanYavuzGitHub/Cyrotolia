using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WraithKontrol : DusmanKontrol
{
    
    [SerializeField] protected float SaldiriMenzil;
    protected float SonrakiSaldiri;
    [SerializeField] protected float SaldiriicinBekle;
    public GameObject BuyuSprite;
    public Transform BuyuAtisNoktasi;
    

    public override void Baglanti()
    {
        base.Baglanti();
    }

    #region SaldiriKontrol
    public override void SaldiriKontrol()
    {
        if(Vector2.Distance(transform.position,Hedef.transform.position) <= SaldiriMenzil)
        {
            if (Time.time > SonrakiSaldiri)
            {
                DusmanAnimasyon.Saldir();
                Instantiate(BuyuSprite, BuyuAtisNoktasi.transform.position, Quaternion.identity);
                SonrakiSaldiri = Time.time + SaldiriicinBekle;
            }
        }
        else
        {
            Durum = Enum.HedefiGormedi;
        }
    }
    #endregion

}
