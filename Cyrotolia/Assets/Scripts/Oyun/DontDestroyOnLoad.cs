using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    public string id;

    private void Awake()
    {
        id = name + transform.position.ToString() + transform.eulerAngles.ToString();
    }
    void Start()
    {
        for (int i = 0; i < Object.FindObjectsOfType<DontDestroyOnLoad>().Length; i++)
        {
            if (Object.FindObjectsOfType<DontDestroyOnLoad>()[i] != this)
            {
                if (Object.FindObjectsOfType<DontDestroyOnLoad>()[i].id == id)
                {
                    Destroy(gameObject);
                }
            }

        }
        DontDestroyOnLoad(gameObject);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
