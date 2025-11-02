using System;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public enum Size { TINY, HUGE }
public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject[] characters;

    [SerializeField] private Size currentSize = Size.HUGE;

    private InputAction changeSizeBut;

    private int scaleEnumCount;

    void Start()
    {
        changeSizeBut = InputSystem.actions.FindAction("Change Size");

        scaleEnumCount = Enum.GetValues(typeof(Size)).Length;

        //Sets default character to active
        PlayerController_Base pcScript = characters[(int)currentSize].GetComponent<PlayerController_Base>();
        pcScript.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (changeSizeBut.WasPressedThisFrame())
        {
            Size newSize = (Size)(((int)currentSize + 1) % characters.Length);
            SwitchPlayerSize(newSize);
            return;

        }
    }
    
    void SwitchPlayerSize(Size size)
    {
        if ((int)size > characters.Length || (int)size < 0) return;

        PlayerController_Base pcScript = characters[(int)currentSize].GetComponent<PlayerController_Base>();

        if (pcScript == null) return;

        pcScript.SetActive(false);

        pcScript = characters[(int)size].GetComponent<PlayerController_Base>();

        if (pcScript == null) //If character we are switching to does not exist, remain on current
        {
            SwitchPlayerSize(currentSize);
        }
        
        pcScript.SetActive(true);
        
        currentSize = size;
    }
}
