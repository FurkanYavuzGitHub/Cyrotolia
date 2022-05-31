using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SahneDegistirme : MonoBehaviour
{
    public int _OncekiSahne;
    public int _SahneSayi;
    public static bool Geri;

    private OyuncuKontrol Oyuncu;

    public SpriteRenderer Karakter;
    public GameObject Ui;


    string[] _Sahneler = { "Zirve", "Orman", "Koy_1", "Koy_2", "Koy_3", "Koy_4", "DenizKiyisi", "Magara", "Magara_2", "Zindan", "Zindan_1", "Boss"};
    void Start()
    {
        #region Baðlantý
        Oyuncu = FindObjectOfType<OyuncuKontrol>();
        Karakter = GameObject.Find("Karakter").GetComponentInChildren<SpriteRenderer>();
        Ui = GameObject.Find("Ui");
        #endregion

        #region CPPoint Kontrol
        PlayerPrefs.HasKey("CPPoint");

        if (PlayerPrefs.HasKey("CPPoint"))
        {
            CheckPoint.CPPoint = PlayerPrefs.GetInt("CPPoint");
        }
        else
        {
            PlayerPrefs.SetInt("CPPoint", 0);
        }

        #endregion

        #region Sahne Sayisini Kontrol Et Eðer Yoksa Oluþtur
        PlayerPrefs.HasKey("_SahneSayi"); //_SahneSayi isminde bir perfs oluþtur.

        if (PlayerPrefs.HasKey("_SahneSayi"))   //Sahnesayisinin olup olmadýðýný kontrol et yoksa oluþtur.
        {
            _SahneSayi = PlayerPrefs.GetInt("_SahneSayi");
        }
        else
        {
            PlayerPrefs.SetInt("_SahneSayi", 0);
        }
        #endregion

        #region Önceki Sahneyi Kontrol Et Eðer Yoksa Oluþtur
        PlayerPrefs.HasKey("_OncekiSahne"); // _OncekiSahne isminde bir perfs oluþtur.

        if (PlayerPrefs.HasKey("_OncekiSahne"))   //_OncekiSahnenin olup olmadýðýný kontrol et yoksa oluþtur.
        {
            _OncekiSahne = PlayerPrefs.GetInt("_OncekiSahne");
        }
        else
        {
            PlayerPrefs.SetInt("_OncekiSahne", 0);
        }
        #endregion
    }

    #region Tekrar Oyna (Ölüm Panelinde Buton Kontrol)
    public void TekrarOyna() //öldüðünde haritayý tekrar baþlat
    {
        Time.timeScale = 1;
        if(CheckPoint.CPPoint == 0)
        {
            CheckPoint0();
        }
        else if (CheckPoint.CPPoint == 1)
        {
            CheckPoint1();
        }
        
        Oyuncu.YenidenDog();
    }
    #endregion

    #region OyunaBasla (Menüde Buton Kontrol)
    public void OyunaBasla()  //menüden oyna butonuna basýldýðýnda oyunu baþlat
    {
        if (CheckPoint.CPPoint == 0)
        {
            CheckPoint0();
        }
        else if (CheckPoint.CPPoint == 1) 
        {
            CheckPoint1();
        }
        Time.timeScale = 1;
        Oyuncu.YenidenDog();
        
    }
    #endregion

    #region AnaMenüyeDön (Ölüm Panelinde Buton Kontrol)
    public void AnaMenuyeDon()  //Anamenüye dön 
    {
        Oyuncu.OyuncuSprite.gameObject.SetActive(false);
        Oyuncu.Kamera.SetActive(false);
        Oyuncu.Ui.SetActive(false);
        SceneManager.LoadScene(0);
    }
    #endregion

    #region Sonraki Sahneyi Aç
    public void SonrakiSahne() //Sonraki haritaya geç
    {
        Geri = false;
        _SahneSayi++;
        OncekiSahneyiKaydet();
        PlayerPrefs.SetInt("_SahneSayi", _SahneSayi);
        SceneManager.LoadScene(_Sahneler[_SahneSayi]);
    }
    #endregion

    #region Önceki Sahneyi Aç
    public void OncekiSahne()   //Onceki haritaya geç
    {
        Geri = true;
        _SahneSayi--;
        OncekiSahneyiKaydet();
        PlayerPrefs.SetInt("_SahneSayi", _SahneSayi);
        SceneManager.LoadScene(_Sahneler[_SahneSayi]);
    }
    #endregion

    #region Önceki Sahneyi Kaydet (Pozisyon Ayarý için)
    void OncekiSahneyiKaydet() //Pozisyonayarla dosyasýnda kullanýlýyor
    {
        _OncekiSahne = _SahneSayi;
        Oyuncu.PozisyonAyarla(_OncekiSahne);
        PlayerPrefs.SetInt("_OncekiSahne", _OncekiSahne);
    }
    #endregion

    #region Oyun Kapatýldýðýnda Sahne Sayýsýný Sýfýrla
    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("CPPoint", CheckPoint.CPPoint);
        if (CheckPoint.CPPoint < 1)
        {
            PlayerPrefs.SetInt("_SahneSayi", 1);
            PlayerPrefs.SetInt("_OncekiSahne", 0);
        }
        else
        {
            PlayerPrefs.SetInt("_SahneSayi", 4);
            PlayerPrefs.SetInt("_OncekiSahne", 3);
        }
    }
    #endregion

    #region CheckPoint

    public void CheckPoint0()
    {
        PlayerPrefs.SetInt("_SahneSayi", 0);
        PlayerPrefs.SetInt("_OncekiSahne", 0);
        SceneManager.LoadScene(1);
    }
    
    public void CheckPoint1()
    {
        PlayerPrefs.SetInt("_SahneSayi", 3);
        PlayerPrefs.SetInt("_OncekiSahne", 2);
        SceneManager.LoadScene(4);
    }
    #endregion


    #region Trigger Kontrol
    private void OnTriggerEnter2D(Collider2D _Karakter) // belirlenen objelere dokunduðumda kodu çalýþtýr
    {
        if (this.gameObject.name == "SonrakiSahne" && _Karakter.tag == "Karakter")
        {
            SonrakiSahne();
        }
        else if (this.gameObject.name == "OncekiSahne" && _Karakter.tag == "Karakter")
        {
            OncekiSahne();
        }
    }
    #endregion

}
