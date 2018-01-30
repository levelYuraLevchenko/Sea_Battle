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

    // Функция контроля правил расстановки кораблей. 
    bool TestEnterDeck(int X, int Y)
    {
        // Если точка прверки за границами поля то сразу false.
        if((X > -1) && (Y > -1) && (X < 10) && (Y < 10))
        {
            int[] arrX = new int[9];
            int[] arrY = new int[9];
            // Разчитываю провераемые координаты.
            arrX[0] = X + 1;  /*|*/   arrX[1] = X;      /*|*/   arrX[2] = X - 1;  
            arrY[0] = Y + 1;  /*|*/   arrY[1] = Y + 1;  /*|*/   arrY[2] = Y + 1;

            arrX[3] = X + 1;  /*|*/   arrX[4] = X;      /*|*/   arrX[5] = X - 1;
            arrY[3] = Y;      /*|*/   arrY[4] = Y;      /*|*/   arrY[5] = Y;

            arrX[6] = X + 1;  /*|*/   arrX[7] = X;      /*|*/   arrX[8] = X - 1;
            arrY[6] = Y - 1;  /*|*/   arrY[7] = Y - 1;  /*|*/   arrY[8] = Y - 1;

            // Сомотрю что вокруг поля.
            for(int i = 0; i < 9; i++)
                // Проверяю существует ли координата.
                if((arrX[i] > -1) && (arrY[i] > -1) && (arrX[i] < 10) && (arrY[i] < 10))
                    // Если несуществует возвращаем false.
                    if(_slots[arrX[i], arrY[i]].GetComponent<Print>().Indrx != 0)
                        return false;
            // Если не в одной точку не появилось помехи возвращаю true.
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
            _slots[X, Y].GetComponent<Print>().Indrx = 1;   
    }
}
