using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject muz;

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Settings()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void Awake()
    {
        DontDestroyOnLoad(muz);
    }

    public void SetSound(float value)
    {
        Global.soundVolume = value;
    }

    public void SetMusic()
    {
        //Global.musicVolume = value;
        muz.GetComponent<AudioSource>().volume = settingsPanel.GetComponent<Slider>().value;//
    }


}
