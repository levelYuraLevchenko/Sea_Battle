using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOnBoard : MonoBehaviour
{
    public GameObject WhoParent = null;
    public int coordinateX, coordinateY;

    private void OnMouseDown()
    {
        if(WhoParent != null)
            WhoParent.GetComponent<GameBoard>().WhoClick(coordinateX, coordinateY);
    }
}
