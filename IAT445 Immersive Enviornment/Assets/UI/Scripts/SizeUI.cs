using UnityEngine;
using UnityEngine.InputSystem;

public class SizeUI : MonoBehaviour
{
    public bool big = true;

    public GameObject bigPicture;

    public GameObject smallPicture;

    private InputAction change;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bigPicture.SetActive(true);
        change = InputSystem.actions.FindAction("Change Size");
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.Shift))
        // if(GameObject.Find("Big Player").GetComponent<h>()
        //     .isActive = true)
        if(change.WasPressedThisFrame())
        {
            if (big)
            {
                smallUI();
            }
            else
            {
                bigUI();
            } 
        }
    }

    public void bigUI()
    {
        Debug.Log("big");
        smallPicture.SetActive(false);
        bigPicture.SetActive(true);
        big = true;
    }
    
    public void smallUI()
    {
        Debug.Log("small");
        bigPicture.SetActive(false);
        smallPicture.SetActive(true);
        big = false;
    }
}
