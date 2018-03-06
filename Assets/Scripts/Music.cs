using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    public static Music Instance;
    public List<AudioClip> AudioList;
    private AudioSource audioSource;

    public AudioSource musicSource;
    public Slider musicSlider, soundSlider;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //AudioListener.volume = musicSlider.value; //управляет всеми AudioSource.

        musicSource.volume = musicSlider.value;
        audioSource.volume = soundSlider.value;
    }
    
    public void PlaySound(int index)
    {
        audioSource.PlayOneShot(AudioList[index]);
    }
}
