using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public bool Paused = false;
    public GameObject pauseMenuScreen;

    public GameObject howToPlay;

    public GameObject settings;
    private InputAction pause;

    [SerializeField] private AudioClip menuOpen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //pauseMenuScreen.SetActive(false);
        pause = InputSystem.actions.FindAction("Pause");
    }

    // Update is called once per frame
    void Update()
    {
        if (pause.WasPressedThisFrame())
        {
            if (Paused)
            {
                Resume();
            }
            else
            {
                SoundManager.instance.PlaySFX(menuOpen, transform, 1f);
                Pause();
            }
        }
    }


    public void Resume()
    {
        Debug.Log("resumed");
        howToPlay.SetActive(false);
        settings.SetActive(false);
        pauseMenuScreen.SetActive(false);
        Time.timeScale = 1f;
        Paused = false;
    }

    void Pause()
    {
        Debug.Log("paused");
        pauseMenuScreen.SetActive(true);
        Time.timeScale = 0f;
        Paused = true;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void HowToPlay()
    {
        howToPlay.SetActive(true);
        pauseMenuScreen.SetActive(false);
    }

    public void Settings()
    {
        settings.SetActive(true);
        pauseMenuScreen.SetActive(false);
    }
}
