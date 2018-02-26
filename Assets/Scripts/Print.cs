using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Print : MonoBehaviour
{
    // Переменная содержащая массив спрайтов на которые можно менять текущюю.
    public Sprite[] imgs;

    // Переменная для указания необходимого спрайта.
    public int Index = 0;

    // Переменная для скрытия кораблей противника.
    public bool hidePrint;

    // Смена спрайтов.
    private void ChangeImgs()
    {
        if(imgs.Length > Index)
        {
            if((hidePrint) && Index == 1)
            {
                GetComponent<SpriteRenderer>().sprite = imgs[0];
            }
            else
            {
                // Задаю спрайт блока.
                GetComponent<SpriteRenderer>().sprite = imgs[Index];
            }
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
