using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public GameObject number, letter, slot;

    GameObject[] Numbers;
    GameObject[] Letters;
    GameObject[,] Slots;

    int lengBoard = 10;

    void CreateBoard()
    {
        Vector2 startPosition = transform.position;

        var posX = startPosition.x + 1;
        var posY = startPosition.y - 1;

        Letters = new GameObject[lengBoard];
        Numbers = new GameObject[lengBoard];

        for(var i = 0; i < lengBoard;i++)
        {
            Letters[i] = Instantiate(letter);
            Letters[i].transform.position = new Vector2(posX, startPosition.y);
            Letters[i].GetComponent<Print>().Indrx = i;
            posX++;

            Numbers[i] = Instantiate(number);
            Numbers[i].transform.position = new Vector2(startPosition.x, posY);
            Numbers[i].GetComponent<Print>().Indrx = i;
            posY--;
        }

        posX = startPosition.x + 1;
        posY = startPosition.y - 1;

        Slots = new GameObject[lengBoard, lengBoard];

        for (var i = 0; i < lengBoard; i++)
        {
            for (var j = 0; j < lengBoard; j++)
            {
                Slots[i,j] = Instantiate(slot);
                Slots[i,j].transform.position = new Vector2(posX, posY);
                Slots[i,j].GetComponent<Print>().Indrx = 0;
                posX++;
            }
            posX = startPosition.x + 1;
            posY--;
        }
    }

	void Start ()
    {
        CreateBoard();
	}
	
	void Update () {
		
	}
}
