using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoyluKontrol : MonoBehaviour
{
    private enum KoyluEnum
    {
        KarakteriGordu,KarakteriGormedi
    }
    private KoyluEnum Durum = KoyluEnum.KarakteriGormedi;

    [SerializeField] private float  Hiz;
    [SerializeField] private float GorusAlani;

    [SerializeField] Transform[] HareketPozisyonlari;
    [SerializeField] private float PozisyonBeklemeSuresi;
    private int PozisyonIndex;
    private bool H_Kontrol;

    private OyuncuKontrol oyuncu;
    private KoyluAnimasyonKontrol Animasyon;

    private void Awake()
    {
        oyuncu = FindObjectOfType<OyuncuKontrol>();
        Animasyon = GetComponent<KoyluAnimasyonKontrol>();

    }

    #region Gorus Alani Kontrol
    public void GorusAlaniKontrol()
    {
        if(Vector2.Distance(transform.position,oyuncu.transform.position) < GorusAlani)
        {
            Durum = KoyluEnum.KarakteriGordu;
        }
        else
        {
            Durum = KoyluEnum.KarakteriGormedi;
        }
    }
    #endregion

    #region Yon Kontrol
    private void YonKontrol()
    {
        switch (Durum)
        {
            case KoyluEnum.KarakteriGormedi:
                {
                    if (HareketPozisyonlari[PozisyonIndex].transform.localScale.x < 0)
                    {
                        transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                    }
                    else
                    {
                        transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                    }
                }
                break;
            case KoyluEnum.KarakteriGordu:
                {
                    if (transform.position.x > oyuncu.transform.position.x)
                    {
                        transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                    }
                    else if (transform.position.x < oyuncu.transform.position.x)
                    {
                        transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                    }
                }
                break;
        }
    }
    #endregion

    #region Pozisyon Kontrol

    public void Pozisyonkontrol()
    {
        if (transform.position != HareketPozisyonlari[PozisyonIndex].position)
        {
            Animasyon.Hareket();
            transform.position = Vector2.MoveTowards(transform.position, HareketPozisyonlari[PozisyonIndex].position, Hiz * Time.deltaTime);
        }
        else
        {
            
            if (H_Kontrol == false)
            {
                H_Kontrol = true;
                Animasyon.Dur();
                StartCoroutine(SonrakiPozisyonIcinBekle());
            }
        }

    }
    #endregion

    #region Kaç
    public void Kac()
    {
        if(transform.position.x > oyuncu.transform.position.x)
        {
            transform.Translate(Vector3.right * Hiz * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.left * Hiz * Time.deltaTime);
        }
    }
    #endregion

    #region KoyluZeka

    private void Update()
    {
        YonKontrol();
        GorusAlaniKontrol();
        if(Durum == KoyluEnum.KarakteriGormedi)
        {
            Pozisyonkontrol();
        }
        else if (Durum == KoyluEnum.KarakteriGordu)
        {
            Kac();
        }
    }
    #endregion

    #region IEnumerator Kontrol

    #region PozisyonIE
    private IEnumerator SonrakiPozisyonIcinBekle()
    {

        yield return new WaitForSecondsRealtime(PozisyonBeklemeSuresi);
        if (PozisyonIndex + 1 < HareketPozisyonlari.Length)
        {
            PozisyonIndex++;
        }
        else
        {
            PozisyonIndex = 0;
        }
        H_Kontrol = false;
    }
    #endregion

    #endregion
}
