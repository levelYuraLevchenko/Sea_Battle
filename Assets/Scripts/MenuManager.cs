using UnityEngine;


public class MenuManager : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject settings;
    public GameObject mainMenu;
    public GameObject enterShipMenu;
    public GameObject playerBoard, computerBoard;

    public GameObject startButton; //кнопка.
    public GameObject startImage; //картинка.

    // Main Menu.

    public void Play()
    {
        mainCamera.orthographicSize = 4.0f;
        mainCamera.transform.position = new Vector3(-20, 0, -10);
     
        // Изменения в convas.
        mainMenu.SetActive(false);
        enterShipMenu.SetActive(true);
    }

    public void Settings()
    {
        settings.SetActive(!settings.activeSelf);
    }

    public void Exit()
    {
        Application.Quit();
    }

    //Enter Ship Menu.

    public void StartGame()
    {
        mainCamera.orthographicSize = 5.0f;
        mainCamera.transform.position = new Vector3(0, 0, -10);
        enterShipMenu.SetActive(false);

        playerBoard.GetComponent<GameBoard>().CopyBoard();
    }

    public void Arrange()
    {
        // Активация кнопки "старn" после расстоковки.
        startImage.SetActive(false);
        startButton.SetActive(true);

        // Расстановка кораблей в Enter Ship Menu.
        playerBoard.GetComponent<GameBoard>().EnterRandomShip();
    }

    public void BeckOnMenu()
    {
        mainCamera.orthographicSize = 5.0f;
        mainCamera.transform.position = new Vector3(-40, 0, -10);

        // Очистка поля в Enter Ship Menu.
        playerBoard.GetComponent<GameBoard>().ClearBoard();

        // Изменения в convas.
        mainMenu.SetActive(true);
        enterShipMenu.SetActive(false);
        startImage.SetActive(true);
        startButton.SetActive(false);
    }
}
