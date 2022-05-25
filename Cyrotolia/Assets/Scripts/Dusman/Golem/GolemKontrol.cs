using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemKontrol : DusmanKontrol
{
    [SerializeField] private float Saldirmakicinbekle;
    public bool Sinirlendi = false;
    public override void Baglanti()
    {
        base.Baglanti();
    }
    public override void HedefiTakipEt()
    {
        if(Vector2.Distance(transform.position,Hedef.transform.position) > Uzaklik && Oyuncu.Can > 1)
        {
            DusmanAnimasyon.Dur();
            StartCoroutine(Bekle());
            if (Sinirlendi)
            {
                DusmanAnimasyon.SinirlenVeHareketet();
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(Hedef.transform.position.x, transform.position.y), DusmanHiz * Time.deltaTime);
            }
        }
        else
        {
            DusmanAnimasyon.SinirliDur();
            SaldiriKontrol();
        }
    }

    #region DüþmanYz
    public override void DusmanYz()
    {

        if (Durum == Enum.Oldu)
        {
            Destroy(this.gameObject, 3);
            return;
        }
        else
        {
            YonKontrol();
            GorusAlaniKontrol();
            if (Durum == Enum.HedefiGormedi)
            {
                Sinirlendi = false;
                DusmanAnimasyon.Sakinles();
                PozisyonArasiHareketKontrol();
            }
            if (Durum == Enum.HedefiGordu)
            {
                HedefiTakipEt();
            }
        }
    }
    #endregion

    private IEnumerator Bekle()
    {
        yield return new WaitForSeconds(Saldirmakicinbekle);
        Sinirlendi = true;
        
    }
}
