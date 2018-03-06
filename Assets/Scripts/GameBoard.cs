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
    public GameObject[,] _slots;
    // Массив префабов gif.
    public GameObject[] prefMLG;
    public GameObject[] prefMLGDestroy;

    public GameObject mapDestination;

    // Координаты в диапазоне которых отображаются gif-анимации.
    public float minX, minY, maxX, maxY;

    // Прячю корабли противника.
    public bool hideShip;

    // Размер поля (10 на 10).
    private int _lengBoard = 10;

    // Количество кораблей на поле.
    private int[] shipCount = { 0, 4, 3, 2, 1 };

    public List<Ship> listShip = new List<Ship>();

    void Start()
    {
        CreateBoard();

        if(hideShip)
            EnterRandomShip();      
    }

    public struct TestCoord
    {
        public int X, Y;
    }

    public struct Ship
    {
        public TestCoord[] shipCoord;
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
            posX+= 0.67f;

            _numbers[i] = Instantiate(number);
            _numbers[i].transform.position = new Vector2(startPosition.x, posY);
            _numbers[i].GetComponent<Print>().Index = i;
            posY-= 0.67f;
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
                _slots[i, j].GetComponent<Print>().hidePrint = hideShip;

                if(hideShip)
                    _slots[i, j].GetComponent<ClickOnBoard>().WhoParent = gameObject;

                _slots[i, j].GetComponent<ClickOnBoard>().coordinateX = i;
                _slots[i, j].GetComponent<ClickOnBoard>().coordinateY = j;

                posX += 0.67f;
            }
            posX = startPosition.x + 1;
            posY -= 0.67f;
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

        // Если получилось поместить корабль то ставит его.
        if(shipCoord != null)
        {
            foreach(TestCoord type in shipCoord)
            {
                _slots[type.X, type.Y].GetComponent<Print>().Index = 1;
            }

            Ship deck;

            // Сохранение кординат корабля.
            deck.shipCoord = shipCoord;

            // Сохранение корабля в список.
            listShip.Add(deck);

            // Поставил корабль.
            return true;
        }
        // Не смогли поставить корабль.
        return false;
    }

    private bool CountShips()
    {
        // Переменная для подсчета кораблей.
        int Amount = 0;

        // Суммирует все значения.
        foreach(var Ship in shipCount)
        {
            Amount += Ship;
        }

        // Если сумма не ноль значит не все корабли на поле.
        if(Amount != 0)
        {
            return true;
        }
        // Если сумма 0 значит все корабли на поле.
        return false;
    }

    // Метод очистки поля.
    public void ClearBoard()
    {
        // Возвращаю все корабли в ангар.
        shipCount = new int[] { 0, 4, 3, 2, 1 };
        // Очищаю список кораблей.
        listShip.Clear();

        for (var i = 0; i < _lengBoard; i++)
        {
            for (var j = 0; j < _lengBoard; j++)
            {
                _slots[i, j].GetComponent<Print>().Index = 0;
            }
        }
    }

    // Рандомная расстановка кораблей.
    public void EnterRandomShip()
    {
        ClearBoard();

        // Номер выбранного корабля.
        int selectShip = 4;

        // Координаты по которым будет выставлен корабль.
        int X, Y;
        // Положение корабля на поле ( вертикально или горизонтально).
        int direction;

        while(CountShips())
        {
            X = Random.Range(0, 10);
            Y = Random.Range(0, 10);
            // Получает направление: 0 горизонтально, 1 вертикально.
            direction = Random.Range(0, 2);

            if(EnterDeck(selectShip, direction, X, Y))
            {
                // Если получилось установить то уменьшаем количество кораблей.
                shipCount[selectShip]--;
                // Если закончились корабли данного типа то выбирает следующий.
                if(shipCount[selectShip] == 0)
                {
                    // Смещает на следующую группу кораблей.
                    selectShip--;
                }
            }
        }
    }

    // Слот на который кликнули будеть сообщать об этом.
    public void WhoClick(int X, int Y)
    {
        Shoot(X, Y);
    }

    // Выстрел.
    private bool Shoot(int X, int Y)
    {
        // Получаю индекс спрайта слота.
        int selectSlot = _slots[X, Y].GetComponent<Print>().Index;

        bool result = false;
        Music.Instance.PlaySound(0);

        switch (selectSlot)
        {
            // Промах.
            case 0:
                _slots[X, Y].GetComponent<Print>().Index = 2;
                result = false;

                break;
            // Попадание.
            case 1:
                _slots[X, Y].GetComponent<Print>().Index = 3;
                result = true;

                // Убил.
                if (TestShoot(X, Y))
                {
                    if (prefMLGDestroy != null)
                        StartCoroutine(MlgDestroyShip());

                    StopCoroutine(MlgDestroyShip());
                }
                // Ранил.
                else
                {
                    if (prefMLG != null)
                        StartCoroutine(MlgHit());

                    StopCoroutine(MlgHit());                
                }
                break;
        }
        return result;
    }
    
    // Вызов и удаление gif анимации.
    IEnumerator MlgDestroyShip()
    {
        var mlg = Instantiate(prefMLGDestroy[Random.Range(0, prefMLGDestroy.Length)]);
        // Задержка Х секунд.
        yield return new WaitForSeconds(1.2f);
        Destroy(mlg);
    }

    IEnumerator MlgHit()
    {
        float x = Random.Range(minX, maxX); //позиция.
        float y = Random.Range(minY, maxY); //позиция.
        var mlg = Instantiate(prefMLG[Random.Range(0, prefMLG.Length)], new Vector3(x, y, -1), transform.rotation);
        yield return new WaitForSeconds(0.8f);
        Destroy(mlg);
    }



    // Проверка попадания по кораблю.
    private bool TestShoot(int X, int Y)
    {
        bool result = false;

        // Перебирает корабли и смотрит в какой из них произошло попадание.
        foreach(Ship test in listShip)
        {
            // Перебирает палубы корабля и проверяет попали ли в нее.
            foreach(TestCoord deck in test.shipCoord)
            {
                // Сравнивает координаты выстрела с координатами палубы.
                if((deck.X == X) && (deck.Y == Y))
                {
                    // Переменная отвечающая за количество попаданий по кораблю.
                    int killCount = 0;
                    // Если игрок попал по палубе то проверяет сколько палуб разрушено в корабле.
                    foreach(TestCoord killDeck in test.shipCoord)
                    {
                        // Проверяет что записано в поле по данным координатам.
                        int testBlock = _slots[killDeck.X, killDeck.Y].GetComponent<Print>().Index;
                        // Если записано 3 то подсчитывает ранение.
                        if(testBlock == 3)
                            killCount++;
                    }
                    // Если количество палуб равно количеству попаданий то уничтожаем корабль.
                    if(killCount == test.shipCoord.Length)
                    {
                        foreach (TestCoord killDeck in test.shipCoord)
                        {
                            _slots[killDeck.X, killDeck.Y].GetComponent<Print>().Index = 4;                          
                        }                        
                        // Если убит вернем true.
                        result = true;
                    }
                    // Иначе корабль еще живой и вернет false.
                    else
                        result = false;

                    // Завершение цикла и возврат результата.
                    return result;
                }
            }
        }

        return result;
    }

    // Возвращает количество живих кораблей.
    public int LifeShip()
    {
        // Подчитывает количество живих палуб.
        int countLife = 0;

        // Перебор кораблей.
        foreach (Ship test in listShip)
        {
            // Перебирает палубы корабля.
            foreach (TestCoord deck in test.shipCoord)
            {
                // Состояние палубы.
                int testDeck = _slots[deck.X, deck.Y].GetComponent<Print>().Index;

                // Если 1 то палуба жива.
                if (testDeck == 1)
                {
                    countLife++;
                }
            }
        }
        // Вернет найденные палубы.
        return countLife;
    }

    // Копирует поле из редактора в боевую зону.
    public void CopyBoard()
    {
        if (mapDestination != null)
        {
            for (var i = 0; i < _lengBoard; i++)
            {
                for (var j = 0; j < _lengBoard; j++)
                {
                    // Записывает данные одного поля в другое.
                    mapDestination.GetComponent<GameBoard>()._slots[i, j].GetComponent<Print>().Index = _slots[i, j].GetComponent<Print>().Index;
                }
            }
            // Читит список.
            mapDestination.GetComponent<GameBoard>().listShip.Clear();

            // Зписывает сгенерированные корабли.
            mapDestination.GetComponent<GameBoard>().listShip.AddRange(listShip);
        }
    }
}
