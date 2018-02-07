using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public GameObject number, letter, slot;

    // Массив цифр.
    private GameObject[] _numbers;
    // Массив букв.
    private GameObject[] _letters;
    // Игровое поле.
    private GameObject[,] _slots;

    // Размер поля (10 на 10).
    private int _lengBoard = 10;

    void Start()
    {
        CreateBoard();
    }

    void Update()
    {

    }

    struct TestCoord
    {
        public int X, Y;
    }

    // Вывод координатных линий и игрового поля.
    void CreateBoard()
    {
        Vector2 startPosition = transform.position;

        var posX = startPosition.x + 1;
        var posY = startPosition.y - 1;

        _letters = new GameObject[_lengBoard];
        _numbers = new GameObject[_lengBoard];

        for(var i = 0; i < _lengBoard; i++)
        {
            _letters[i] = Instantiate(letter);
            _letters[i].transform.position = new Vector2(posX, startPosition.y);
            _letters[i].GetComponent<Print>().Index = i;
            posX++;

            _numbers[i] = Instantiate(number);
            _numbers[i].transform.position = new Vector2(startPosition.x, posY);
            _numbers[i].GetComponent<Print>().Index = i;
            posY--;
        }

        posX = startPosition.x + 1;
        posY = startPosition.y - 1;

        _slots = new GameObject[_lengBoard, _lengBoard];

        // Отрисовка поля по Х
        for (var i = 0; i < _lengBoard; i++)
        {
            // Отрисовка поля по Y.
            for (var j = 0; j < _lengBoard; j++)
            {
                _slots[i,j] = Instantiate(slot);
                _slots[i,j].transform.position = new Vector2(posX, posY);
                _slots[i,j].GetComponent<Print>().Index = 0;

                _slots[i, j].GetComponent<ClickOnBoard>().WhoParent = gameObject;
                _slots[i, j].GetComponent<ClickOnBoard>().coordinateX = i;
                _slots[i, j].GetComponent<ClickOnBoard>().coordinateY = j;

                posX++;
            }
            posX = startPosition.x + 1;
            posY--;
        }
    }

    // Контроль правил расстановки кораблей. 
    private bool TestEnterDeck(int X, int Y)
    {
        // Если точка проверки за границами поля то возвращаю false.
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
            for(var i = 0; i < 9; i++)
            {
                // Проверяю существует ли координата.
                if ((arrX[i] > -1) && (arrY[i] > -1) && (arrX[i] < 10) && (arrY[i] < 10))
                {
                    // Если несуществует возвращаем false.
                    if (_slots[arrX[i], arrY[i]].GetComponent<Print>().Index != 0)
                        return false;
                }
            }
            // Если не в одном слоте не появилось помехи возвращаю true.
            return true;
        }
        return false;
    }

    /// <summary>
    /// Проверяю установку палуб в определенном напровлении.
    /// </summary>
    /// <param name="ShipType"> Тип корабля и количество палуб. </param>
    /// <param name="XDir"> Смещение по осям проверки. </param>
    /// <param name="YDir"> Смещение по осям проверки. </param>
    /// <param name="X"> Начальная точка установки корабля. </param>
    /// <param name="Y"> Начальная точка установки корабля. </param>
    /// <returns> Возможно ли поставить в этом месте. </returns>
    private TestCoord[] TestEnterShipDirection(int ShipType, int XDir, int YDir, int X, int Y)
    {
        // Массив для результатов.
        TestCoord[] ResultCoord = new TestCoord[ShipType];
        // Дважение в указанную сторону.
        for(var i = 0; i < ShipType; i++)
        {
            // Проверяю можно ли поставить палубу в указанном слоте.
            if (TestEnterDeck(X, Y))
            {
                // Записываю значения в результат.
                ResultCoord[i].X = X;
                ResultCoord[i].Y = Y;
            }
            else
                // Щстанавливаю проверку и возвращаю null.
                return null;
            // Смещаю проверку в указанном направлении.
            X += XDir;
            Y += YDir;
        }
        return ResultCoord;
    }

    /// <summary>
    /// Сообщаем какой корабль мы хотим поставить в указанном месте,
    /// система вернет список кораблей либо null если не нашла места.
    /// </summary>
    /// <param name="ShipType"> Тип корабля и количество палуб. </param>
    /// <param name="Direction"> Напровление ( 0 вертикально, 1 горизонтально). </param>
    /// <param name="X"> Начальная точка установки корабля. </param>
    /// <param name="Y"> Начальная точка установки корабля. </param>
    /// <returns> Кординаты для установки корабля. </returns>
    private TestCoord[] TestEnterShip(int ShipType, int Direction, int X, int Y)
    {
        // Массив для результатов.
        TestCoord[] ResultCoord = new TestCoord[ShipType];

        // Проверяю можно ли поставить палубу в указанном слоте.
        if (TestEnterDeck(X, Y))
        {
            // Выбор напровления.
            switch(Direction)
            {
                case 0:                
                    // Попытка установить палубу в положительном напровлении по оси Х.
                    ResultCoord = TestEnterShipDirection(ShipType, 0, 1, X, Y);

                    // Если не вышло побую в отрицательном.
                    if (ResultCoord == null)
                    {
                        ResultCoord = TestEnterShipDirection(ShipType, 0, -1, X, Y);
                    }
                    break;
                case 1:
                    // Попытка установить палубу в положительном напровлении по оси Y.
                    ResultCoord = TestEnterShipDirection(ShipType, 1, 0, X, Y);

                    // Если не вышло побую в отрицательном.
                    if (ResultCoord == null)
                    {
                        ResultCoord = TestEnterShipDirection(ShipType, -1, 0, X, Y);
                    }
                    break;
            }
            // Возвращаю кординаты для установки корабля.
            return ResultCoord;
        }
        // Если не удалось расчитать место под корабль опираясь на указанную точку возвращаю null.
        return null;
    }

    // Установка корабля в указанный слот.
    private bool EnterDeck(int ShipType, int Direction, int X, int Y)
    {
        // Получаю кординаты для установки корабля.
        TestCoord[] shipCoord = TestEnterShip(ShipType, Direction, X, Y);

        // Если получилось поместить корабль то ставлю его.
        if(shipCoord != null)
        {
            foreach(TestCoord T in shipCoord)
            {
                _slots[T.X, T.Y].GetComponent<Print>().Index = 1;
            }
            // Gставили корабль.
            return true;
        }
        // Не смогли поставить корабль.
        return false;
    }

    // Слот на который кликнули будеть сообщать об этом.
    public void WhoClick(int X, int Y)
    {
        //if(TestEnterDeck(X,Y))
        //    _slots[X, Y].GetComponent<Print>().Index = 1;

        EnterDeck(4, 1, X, Y);
        //EnterDeck(4, 0, X, Y);
    }
}
