using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public GameObject number, letter, slot;

    private GameObject[] _numbers;
    private GameObject[] _letters;
    private GameObject[,] _slots;

    int lengBoard = 10;

    void CreateBoard()
    {
        Vector2 startPosition = transform.position;

        var posX = startPosition.x + 1;
        var posY = startPosition.y - 1;

        _letters = new GameObject[lengBoard];
        _numbers = new GameObject[lengBoard];

        for(var i = 0; i < lengBoard;i++)
        {
            _letters[i] = Instantiate(letter);
            _letters[i].transform.position = new Vector2(posX, startPosition.y);
            _letters[i].GetComponent<Print>().Indrx = i;
            posX++;

            _numbers[i] = Instantiate(number);
            _numbers[i].transform.position = new Vector2(startPosition.x, posY);
            _numbers[i].GetComponent<Print>().Indrx = i;
            posY--;
        }

        posX = startPosition.x + 1;
        posY = startPosition.y - 1;

        _slots = new GameObject[lengBoard, lengBoard];

        for (var i = 0; i < lengBoard; i++)
        {
            for (var j = 0; j < lengBoard; j++)
            {
                _slots[i,j] = Instantiate(slot);
                _slots[i,j].transform.position = new Vector2(posX, posY);
                _slots[i,j].GetComponent<Print>().Indrx = 0;

                _slots[i, j].GetComponent<ClickOnBoard>().WhoParent = gameObject;
                _slots[i, j].GetComponent<ClickOnBoard>().coordinateX = i;
                _slots[i, j].GetComponent<ClickOnBoard>().coordinateY = j;

                posX++;
            }
            posX = startPosition.x + 1;
            posY--;
        }
    }

    // ДОПИЛИТЬ!!!
    bool TestEnterDeck(int X, int Y)
    {
        if((X > -1) && (Y > -1) && (X < 10) && (Y < 10))
        {
            int[] arrX = new int[9];
            int[] arrY = new int[9];

            arrX[0] = X + 1;  /*|*/   arrX[0] = X;      /*|*/   arrX[0] = X - 1;  
            arrY[0] = Y + 1;  /*|*/   arrY[0] = Y + 1;  /*|*/   arrY[0] = Y + 1;

            arrX[0] = X + 1;  /*|*/   arrX[0] = X;      /*|*/   arrX[0] = X - 1;
            arrY[0] = Y;      /*|*/   arrY[0] = Y;      /*|*/   arrY[0] = Y;

            arrX[0] = X + 1;  /*|*/   arrX[0] = X;      /*|*/   arrX[0] = X - 1;
            arrY[0] = Y - 1;  /*|*/   arrY[0] = Y - 1;  /*|*/   arrY[0] = Y - 1;

            // Сомотим что вокруг поля.
            for(int I = 0; I < 9; I++)
            {
                // Проверяю существует ли координата.
                if((arrX[I] > -1) && (arrY[I] > -1) && (arrX[I] < 10) && (arrY[I] < 10))
                {
                    if(_slots[arrX[I], arrY[I]].GetComponent<Print>().Indrx != 0) return false;
                }
            }
            return true;
        }
        return false;
    }

	void Start ()
    {
        CreateBoard();
	}
	
	void Update ()
    {
		
	}

    public void WhoClick(int X, int Y)
    {
        if(TestEnterDeck(X,Y))
        {
            _slots[X, Y].GetComponent<Print>().Indrx = 1;
        }    
    }
}
