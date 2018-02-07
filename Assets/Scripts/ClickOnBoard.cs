using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOnBoard : MonoBehaviour
{
    public GameObject WhoParent = null;
    // Позиция слота на поле.
    public int coordinateX, coordinateY;

    private void OnMouseDown()
    {
        // Если ссылка существует то буду что-то делать.
        if(WhoParent != null)
            WhoParent.GetComponent<GameBoard>().WhoClick(coordinateX, coordinateY);
    }
}
