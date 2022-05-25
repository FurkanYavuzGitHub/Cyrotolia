using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasarKontrol 
{
    int Can { get; set; }

    void HasarAl(int HasarMiktari);
}
