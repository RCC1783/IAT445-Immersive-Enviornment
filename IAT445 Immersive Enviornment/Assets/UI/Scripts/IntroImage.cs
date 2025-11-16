using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IntroImage : MonoBehaviour
{
    public GameObject intro;

    private InputAction click;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        intro.SetActive(true);
        click = InputSystem.actions.FindAction("Click");
    }

    // Update is called once per frame
    void Update()
    {
        if (click.WasPressedThisFrame())
        {
            exit();
        }
    }
    
    public void exit()
    {
        intro.SetActive(false);
        Destroy(intro);
    }
}
