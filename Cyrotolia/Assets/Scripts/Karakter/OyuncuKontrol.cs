using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum OyuncuKontrolYonu
{
    OKY_Sol = 0,
    OKY_Sag = 1,
    OKY_Sifir = 2
}

public class OyuncuKontrol : MonoBehaviour, IHasarKontrol
{


    [SerializeField]
    private float ZiplamaGucu = 4.5f;
    private bool ZiplamayiResetle = false;
    private bool yerde_mi = false;

    private bool oyuncuGirdiAldi_mi = false;
    private OyuncuKontrolYonu oyuncuKontrolYonu = OyuncuKontrolYonu.OKY_Sag;

    [SerializeField]
    public float hiz;
    [SerializeField]
    protected int OyuncuCan;
    [SerializeField]
    protected int OyuncuBuyu;

    private float HareketHorizontal;

    //Ba�lant�
    private Rigidbody2D Rigid;
    private OyuncuAnimasyonKontrol OyuncuAnimasyon;
    public SpriteRenderer OyuncuSprite;
    private SaldiriKontrol Kilic;
    public GameObject Ui;
    public GameObject Kamera;

    //Oyun i�i Ba�lant�
    public GameObject FenerLight;
    public GameObject P_OyunSonu;

    //Can ve Buyu
    public Image Canbar;
    RectTransform Can_Bar_Rec;
    public Image Buyubar;
    RectTransform Buyu_Bar_Rec;
    int Maxcan = 100;
    int Maxbuyu = 100;

    //SahneKontrol
    private SahneDegistirme SahneKontrol;

    //Kontrol
    public static bool B_SavunmaKontrol;

    //Ses
    public static bool Oyundurdu;
    public int Can { get; set; }



    void Start()
    {

        #region Ba�lant� Kur

        Rigid = GetComponent<Rigidbody2D>();
        Kilic = GetComponentInChildren<SaldiriKontrol>();
        OyuncuAnimasyon = GetComponent<OyuncuAnimasyonKontrol>();
        OyuncuSprite = GetComponentInChildren<SpriteRenderer>();
        SahneKontrol = FindObjectOfType<SahneDegistirme>();
        Ui = GameObject.Find("Ui");
        Kamera = GameObject.Find("Kamera");
        #endregion

        #region Baslang�� Ayarlar�
        Ui.gameObject.SetActive(false);
        Kamera.gameObject.SetActive(false);
        OyuncuSprite.GetComponentInParent<Rigidbody2D>().gravityScale = 0;
        Can = OyuncuCan;
        OyuncuCan = Maxcan;
        OyuncuBuyu = Maxbuyu;
        Can_Bar_Rec = Canbar.rectTransform;
        Buyu_Bar_Rec = Buyubar.rectTransform;
        FenerLight.SetActive(false);
        P_OyunSonu.SetActive(false);
        #endregion

    }

    void Update()
    {
        BarKontrol();
        if (Can > 0)
        {
            GenelKontroller();

            HareketKontrol();
            //ZiplamaKontrol();
            //SaldiriKontrol();
            SavunmaKontrol();
        }
    }

    #region Genel Kontroller

    void GenelKontroller()
    {
        Yerde_miKontrol();
        HaritadanDus();
        FenerKontrol();
    }

    #region BarKontrol (Can / Buyu)
    public void BarKontrol()
    {
        Can_Bar_Rec.sizeDelta = new Vector2(Can, Can_Bar_Rec.sizeDelta.y);
        Buyu_Bar_Rec.sizeDelta = new Vector2(OyuncuBuyu, Buyu_Bar_Rec.sizeDelta.y);
    }
    #endregion

    #region Yerdemi Kontrol
    bool Yerde_miKontrol()
    {
        RaycastHit2D Yerde_mi = Physics2D.Raycast(transform.position, Vector2.down, 1f, 1 << 6);

        if (Yerde_mi.collider != null)
        {

            if (ZiplamayiResetle == false)
            {
                OyuncuAnimasyon.Zipla(false);
                return true;
            }
        }
        return false;
    }
    #endregion

    #region Olum Kontrol(Haritadan D��erse)
    public void HaritadanDus()
    {
        if (transform.position.y < -5f)
        {
            Can = 0;
            OlumKontrol();
        }

    }
    #endregion

    #region Yon Kontrol
    void YonKontrol(float hareket)
    {
        if (hareket > 0)
        {
            OyuncuSprite.flipX = false;
        }
        else if (hareket < 0)
        {
            OyuncuSprite.flipX = true;
        }
    }
    #endregion

    #region Fener Kontrol
    private void FenerKontrol()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!FenerLight.activeSelf)
            {
                FenerLight.SetActive(true);
            }
            else
            {
                FenerLight.SetActive(false);
            }
        }
    }
    #endregion

    #endregion

    #region Hareket Kontrol
    void HareketKontrol()
    {
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1.0f)
        {
            HareketHorizontal = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            if (oyuncuGirdiAldi_mi)
            {
                if (oyuncuKontrolYonu == OyuncuKontrolYonu.OKY_Sag)
                {
                    HareketHorizontal = 1.0f;
                }
                else if (oyuncuKontrolYonu == OyuncuKontrolYonu.OKY_Sol)
                {
                    HareketHorizontal = -1.0f;
                }
            }
            else
            {
                HareketHorizontal = 0.0f;
            }
        }

        if (B_SavunmaKontrol)
        {
            HareketHorizontal = 0;
        }
        Rigid.velocity = new Vector2(HareketHorizontal * hiz, Rigid.velocity.y); //oyuncuyu hareket ettir
        OyuncuAnimasyon.Hareket(HareketHorizontal); // ko�ma animasyonunu �al��t�r
        YonKontrol(HareketHorizontal); //bakt���m y�n� ayarla
    }
    #endregion

    #region Ziplama Kontrol
    public void ZiplamaKontrol()
    {
        yerde_mi = Yerde_miKontrol();  //yerde olup olmad���m� kontrol et

        if (/*Input.GetKeyDown(KeyCode.Space) && */Yerde_miKontrol() == true)  //Z�pla
        {
            Rigid.velocity = new Vector2(Rigid.velocity.x, ZiplamaGucu);
            StartCoroutine(Z_Reset());
            OyuncuAnimasyon.Zipla(true);
        }
    }
    #endregion

    #region Saldiri Kontrol
    public void SaldiriKontrol()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Yerde_miKontrol())
        {

            OyuncuAnimasyon.Saldir();
        }
    }
    #endregion

    #region Savunma Kontrol
    private void SavunmaKontrol()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            B_SavunmaKontrol = true;
            OyuncuAnimasyon.Savunma(true);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            B_SavunmaKontrol = false;
            OyuncuAnimasyon.Savunma(false);
        }
    }
    #endregion

    #region HasarAl Kontrol
    public void HasarAl(int Hasar)
    {
        if(!B_SavunmaKontrol)
        {
            Can -= Hasar;
            OyuncuAnimasyon.HasarAl();
            OlumKontrol();
        }
    }
    #endregion

    #region Olum Kontrol
    public void OlumKontrol()
    {
        if (Can < 1)
        {
            OyuncuAnimasyon.Ol();
            P_OyunSonu.SetActive(true);
            
            return;
        }
        else
        {
            P_OyunSonu.SetActive(false);
        }
    }
    #endregion

    #region Yeniden Do�
    public void YenidenDog()
    {
        Kamera.gameObject.SetActive(true);
        OyuncuSprite.gameObject.SetActive(true);
        Ui.gameObject.SetActive(true);
        OyuncuSprite.GetComponentInParent<Rigidbody2D>().gravityScale = 1;
        P_OyunSonu.SetActive(false);
        Can = 100;
        OyuncuAnimasyon.YenidenDog();

        if (CheckPoint.CPPoint == 0)
        {
            transform.position = new Vector2(0.5f, 1.09f);
        }
        else if(CheckPoint.CPPoint == 1)
        {
            transform.position = new Vector2(-3.5f, 1f);
        }
       
    }
    #endregion

    #region IEnumarator Kontrol
    IEnumerator Z_Reset()
    {
        ZiplamayiResetle = true;
        yield return new WaitForSeconds(0.1f);
        ZiplamayiResetle = false;
    }
    #endregion

    #region Pozisyon Kontrol
    public void PozisyonAyarla(int OncekiSahne)
    {
        switch (OncekiSahne)
        {
            case 0: //Zirve Haritas�

                //E�er Zirve Haritas�ndan Orman Haritas�na Ge�tiyse �al��acak Kod
                transform.position = new Vector2(-7, transform.position.y);
                break;

            case 1: // Orman Haritas�

                // E�er K�y 1'den Ormana Ge�tiyse �al��acak Kod
                if (SahneDegistirme.Geri)
                {
                    transform.position = new Vector2(-7, transform.position.y);
                    break;
                }
                //E�er Zirveden Ormana Ge�tiyse �al��acak Kod
                transform.position = new Vector2(7, transform.position.y);
                break;

            case 2: //K�y 1 Haritas�

                //E�er K�y 2'den K�y 1'e Ge�tiyse �al��acak Kod
                if (SahneDegistirme.Geri)
                {
                    transform.position = new Vector2(-7, transform.position.y);
                    break;
                }
                //E�er Ormadan K�y 1'e Ge�tiyse �al��acak Kod
                transform.position = new Vector2(7, transform.position.y);
                break;

            case 3://K�y 2 Haritas�

                //E�er K�y 3'den K�y 2'e Ge�tiyse �al��acak Kod
                if (SahneDegistirme.Geri)
                {
                    transform.position = new Vector2(-7, transform.position.y);
                    break;
                }

                //E�er K�y 1'den K�y 2'ye Ge�tiyse �al��acak Kod
                transform.position = new Vector2(7, transform.position.y);
                break;

            case 4://K�y 3 Haritas�

                //E�er K�y 4'den K�y 3'e Ge�tiyse �al��acak Kod
                if (SahneDegistirme.Geri)
                {
                    transform.position = new Vector2(-7, transform.position.y);
                    break;
                }
                //E�er K�y 2'den K�y 3'e Ge�tiyse �al��acak Kod
                transform.position = new Vector2(7, transform.position.y);
                break;

            case 5://K�y 4 Haritas�

                //E�er DenizK�y�s�n'dan K�y 4'e Ge�tiyse �al��acak Kod
                if (SahneDegistirme.Geri)
                {
                    transform.position = new Vector2(-7, transform.position.y);
                    break;
                }
                //E�er K�y 3'den K�y 4'e Ge�tiyse �al��acak Kod
                transform.position = new Vector2(7, transform.position.y);
                break;

            case 6://DenizK�y�s� Haritas�

                //E�er Ma�ara'dan DenizK�y�s�na Ge�tiyse �al��acak Kod
                if (SahneDegistirme.Geri)
                {
                    transform.position = new Vector2(-1.5f, 0.5f);
                    break;
                }
                //E�er K�y 4'den DenizK�y�s�na Ge�tiyse �al��acak Kod 
                transform.position = new Vector2(7, transform.position.y);
                break;

            case 7://Ma�ara Haritas�

                //E�er Ma�ara2'den Ma�ara Haritas�na Ge�tiyse �al��acak Kod
                if (SahneDegistirme.Geri)
                {
                    transform.position = new Vector2(-7, 0.8f);
                    break;
                }
                //E�er DenizK�y�s�ndan Ma�ara Haritas�na Ge�tiyse �al��acak Kod
                transform.position = new Vector2(7, 3.2f);
                break;

            case 8://Ma�ara2 Haritas�

                //E�er Zindan'dan Ma�ara2 Haritas�na Ge�iyorsa �al��acak Kod
                if (SahneDegistirme.Geri)
                {
                    transform.position = new Vector2(-2.8f, 0.35f);
                    break;
                }
                //E�er Ma�ara'dan Ma�ara2 Haritas�na Ge�iyorsa �al��acak Kod
                transform.position = new Vector2(7, 0.5f);
                break;

            case 9://Zindan Haritas�

                //E�er Zindan1'den Zindan Haritas�na Ge�iyorsa �al��acak Kod
                if (SahneDegistirme.Geri)
                {
                    transform.position = new Vector2(7, 0.4f);
                    break;
                }
                //E�er Ma�ara2'den Zindan Haritas�na Ge�iyorsa �al��acak Kod
                transform.position = new Vector2(-7, 0.4f);
                break;

            case 10://Zindan1 Haritas�

                //E�er Boss Haritas�ndan Zindan1 Haritas�na Ge�iyorsa �al��acak Kod
                if (SahneDegistirme.Geri)
                {
                    transform.position = new Vector2(7, -0.6f);
                    break;
                }
                //E�er Zindan Haritas�ndan Zindan1 Haritas�na Ge�iyorsa �al��acak Kod
                transform.position = new Vector2(-7, 2.4f);
                break;

            case 11://Boss Haritas�

                //E�er Zindan1 Haritas�ndan Boss Haritas�na Ge�iyorsa �al��acak Kod
                transform.position = new Vector2(-7, -0.2f);
                break;
        }
    }

    #endregion

    #region Joy Kontrol
    public void J_Hareket_Basla(float Horizontal)
    {
        oyuncuGirdiAldi_mi = true;

        if (Horizontal == 1.0f)
        {
            oyuncuKontrolYonu = OyuncuKontrolYonu.OKY_Sag;
        }
        else if (Horizontal == -1.0f)
        {
            oyuncuKontrolYonu = OyuncuKontrolYonu.OKY_Sol;
        }
    }

    public void J_Hareket_Bitir(float Horizontal)
    {
        oyuncuGirdiAldi_mi = false;

        oyuncuKontrolYonu = OyuncuKontrolYonu.OKY_Sifir;
    }

    public void J_Saldir()
    {
        B_SavunmaKontrol = false;
        if (Kilic.Saldirabiliyormu)
        {
            OyuncuAnimasyon.Saldir();
        }
    }

    public void J_Zipla()
    {
        B_SavunmaKontrol = false;
        ZiplamaKontrol();
    }

    public void J_Fener()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            B_SavunmaKontrol = false;
            if (!FenerLight.activeSelf)
            {
                FenerLight.SetActive(true);
            }
            else
            {
                FenerLight.SetActive(false);
            }
        }
    }

    public void J_SavunBasla()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            B_SavunmaKontrol = true;
            OyuncuAnimasyon.Savunma(true);
        }
    }
    public void J_Savunbitir()
    {
        B_SavunmaKontrol = false;
        OyuncuAnimasyon.Savunma(false);
    }

    public void J_Durdur()
    {
        B_SavunmaKontrol = false;
        if (P_OyunSonu.activeSelf)
        {
            Oyundurdu = false;
            Time.timeScale = 1;
            P_OyunSonu.SetActive(false);
            return;
        }
        else
        {
            Oyundurdu = true;
            Time.timeScale = 0;
            P_OyunSonu.SetActive(true);
        }
       
    }

    #endregion
}

