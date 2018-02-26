using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Вид ячейки поля.
    public GameObject healthImg;
    // Количество живих палуб.
    public GameObject gameBoard;
    // Панель отображения палуб.
    private GameObject[] _healthBar = new GameObject[20];

    private void CreateHealthBar()
    {
        // Tочка создания HealthBar.
        Vector2 HealthPosition = this.transform.position;
        // Смещенние относительно точки создания поля.
        float PosX = 0.34f;

        for (int i = 0; i < _healthBar.Length; i++)
        {
            // Создает одну ячейку здоровья.
            _healthBar[i] = Instantiate(healthImg) as GameObject;
            // Задает ей позицию.
            _healthBar[i].transform.position = HealthPosition;
            // Смещает на указанное расстояние.
            HealthPosition.x += PosX;
        }
    }

    // Обновляет полосу здоровья.
    private void RefreshHealthBar()
    {
        int LifeCell = 0;

        // Обнуляет все клетки.
        for (int i = 0; i < _healthBar.Length; i++)
        {
            _healthBar[i].GetComponent<Print>().Index = 0;
        }

        // Получает количество живых палуб.
        if(gameBoard != null)
        {
            LifeCell = gameBoard.GetComponent<GameBoard>().LifeShip();
        }

        // Записывает количество ХП в полосу здоровья.
        for (int i = 0; i < LifeCell; i++)
        {
            _healthBar[i].GetComponent<Print>().Index = 1;
        }
    }

	void Start ()
    {
        if(healthImg != null)
            CreateHealthBar();

    }
	
	void Update ()
    {
        if ((gameBoard != null) && (healthImg != null))
            RefreshHealthBar();

    }
}
