using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DusmanKontrol : MonoBehaviour,IHasarKontrol
{
    //Düþman Durum Bilgileri
    protected enum Enum
    {
        HedefiGormedi, HedefiGordu, Oldu
    }
    protected Enum Durum = Enum.HedefiGormedi;

    //Düþman Baðlantý Bilgileri
    protected DusmanAnimasyonKontrol DusmanAnimasyon;
    protected SpriteRenderer DusmanSprite;
    protected SaldiriKontrol Kilic;

    //Düþman Can , Hiz ve Altin Bilgileri
    public int Can { get; set; }
    [SerializeField] protected int DusmanCan;
    [SerializeField] protected float DusmanHiz;
    [SerializeField] protected int DusmanAltin;

    //Düþman Hareket Bilgileri
    [SerializeField] protected Transform[] HareketPozisyonlari;
    [SerializeField]protected float PozisyonBeklemeSuresi;
    protected int PozisyonIndex;
    protected bool H_Kontrol = false;

    //Düþman Saldýrý Bilgileri
    [SerializeField] protected float Uzaklik;
    [SerializeField] protected float GorusAlani;

    //Karakter Bilgileri
    protected OyuncuKontrol Oyuncu;
    protected Transform Hedef;
    internal static bool Ses;

    //Ses

    private void Start()
    {
        Baglanti();
    }

    #region Baglanti
    public virtual void Baglanti()
    {
        Can = DusmanCan;
        Kilic = GetComponentInChildren<SaldiriKontrol>();
        DusmanAnimasyon = GetComponentInChildren<DusmanAnimasyonKontrol>();
        DusmanSprite = GetComponentInChildren<SpriteRenderer>();
        Hedef = GameObject.FindGameObjectWithTag("Karakter").GetComponent<Transform>();
        Oyuncu = FindObjectOfType<OyuncuKontrol>();
    }
    #endregion

    #region Pozisyon Arasý Hareket Kontrol
    public virtual void PozisyonArasiHareketKontrol()
    {
        if (transform.position != HareketPozisyonlari[PozisyonIndex].position)
        {
            DusmanAnimasyon.Hareket();
            transform.position = Vector2.MoveTowards(transform.position, HareketPozisyonlari[PozisyonIndex].position, DusmanHiz * Time.deltaTime);
        }
        else
        {
            if (H_Kontrol == false)
            {
                H_Kontrol = true;
                DusmanAnimasyon.Dur();
                StartCoroutine(SonrakiPozisyonIcinBekle());
            }
        }
        
    }
    #endregion

    #region Hedefi Takip Et
    public virtual void HedefiTakipEt()
    {
        if (Vector2.Distance(transform.position, Hedef.transform.position) > Uzaklik) //Hedefe Doðru hareket et
        {
            
            DusmanAnimasyon.Hareket();
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(Hedef.position.x, transform.position.y, transform.position.z),DusmanHiz*Time.deltaTime);
        }
        else //Hedefe yakýnsa dur
        {
            DusmanAnimasyon.Dur();
            SaldiriKontrol();
        }
    }
    #endregion

    #region Görüþ Alaný Kontrol
    public virtual void GorusAlaniKontrol()
    {
        if (Vector2.Distance(transform.position, Hedef.transform.position) < GorusAlani && Oyuncu.Can > 1)
        {
            Durum = Enum.HedefiGordu;
            Ses = true;
        }
        else
        {
            Ses = false;
            Durum = Enum.HedefiGormedi;
        }
    }
    #endregion

    #region Yon Kontrol
    public virtual void YonKontrol()
    {
        switch (Durum)
        {
            case Enum.HedefiGormedi:
                {
                    if (PozisyonIndex == 0)
                    {
                        //DusmanSprite.flipX = true;
                        transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                    }
                    else
                    {
                        //DusmanSprite.flipX = false;
                        transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                    }
                }
                break;
            case Enum.HedefiGordu:
                {
                    if (transform.position.x > Hedef.transform.position.x)
                    {
                        //DusmanSprite.flipX = true;
                        transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                    }
                    else if (transform.position.x < Hedef.transform.position.x)
                    {
                        //DusmanSprite.flipX = false;
                        transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                    }
                }
                break;
        }
    }
    #endregion

    #region Saldiri Kontrol
    public virtual void SaldiriKontrol()
    {
        if (Vector2.Distance(transform.position, Hedef.transform.position) < Uzaklik)
        {
            if(Kilic.Saldirabiliyormu)
            {
                DusmanAnimasyon.Saldir();
            }
        }
    }
    #endregion

    #region Hasar Al
    public void HasarAl(int Hasar)
    {
        DusmanAnimasyon.HasarAl();
        DusmanCan -= Hasar;
        OlumKontrol();
    }
    #endregion

    #region Ölüm Kontrol
    public virtual void OlumKontrol()
    {
        if(DusmanCan < 1)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            DusmanAnimasyon.Ol();
            Durum = Enum.Oldu;
        }
        
    }
    #endregion

    #region DüþmanYz
    public virtual void DusmanYz()
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
                PozisyonArasiHareketKontrol();
            }
            if (Durum == Enum.HedefiGordu)
            {
                HedefiTakipEt();
            }
        }
    }
    #endregion

    #region IEnumerator Kontrol

    #region PozisyonIE
    protected IEnumerator SonrakiPozisyonIcinBekle()
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

    public virtual void Update()
    {
        DusmanYz();
    }
}
