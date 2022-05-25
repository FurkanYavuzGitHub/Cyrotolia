using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OyuncuYerde_mi : MonoBehaviour
{
    // Start is called before the first frame update
    public LayerMask zemin;
    public bool yerde;
    public Rigidbody2D rigid;
    public float guc;
    public Animator animasyon;
    private OyuncuAnimasyonKontrol OyuncuAnimasyon;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Zipla();
    }

    public void JoyZipla()
    {
        if (yerde == true)
        {
            rigid.velocity += new Vector2(0, guc);
        }

    }
    public void Zipla()
    {
        RaycastHit2D zeminhit = Physics2D.Raycast(transform.position, Vector2.down, 0.03f, zemin);
        if (zeminhit.collider != null)
        {
            yerde = true;
        }
        else
        {
            yerde = false;
        }

        if (Input.GetKeyDown(KeyCode.W) && yerde == true)
        {
            rigid.velocity += new Vector2(0, guc);
            OyuncuAnimasyon.Zipla(yerde);
        }
    }
}
