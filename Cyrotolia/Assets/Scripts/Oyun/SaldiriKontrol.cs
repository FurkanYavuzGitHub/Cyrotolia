using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaldiriKontrol : MonoBehaviour
{
    [SerializeField] private int Hasar;
    [SerializeField] private float SaldirveBekle;
    public bool Saldirabiliyormu = true;

    private void OnTriggerEnter2D(Collider2D N)
    {
        IHasarKontrol HasarVerilecekNesne = N.GetComponent<IHasarKontrol>();
        if (HasarVerilecekNesne != null)
        {
            
            if (Saldirabiliyormu)
            {
                Saldirabiliyormu = false;
                HasarVerilecekNesne.HasarAl(Hasar);
                StartCoroutine(Bekle());
            }
        }
    }
    private IEnumerator Bekle()
    {
        yield return new WaitForSeconds(SaldirveBekle);
        Saldirabiliyormu = true;
    }
}
