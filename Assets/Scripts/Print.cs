using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Print : MonoBehaviour
{
    public Sprite[] imgs;
    public int Indrx = 0;

    void ChangeImgs()
    {
        if(imgs.Length > Indrx)
        {
            GetComponent <SpriteRenderer>().sprite = imgs[Indrx];
        }
    }

    private void Start()
    {
        ChangeImgs();
    }

    private void Update()
    {
        ChangeImgs();
    }
}
