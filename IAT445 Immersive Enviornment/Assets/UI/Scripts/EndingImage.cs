using UnityEngine;

public class EndingImage : MonoBehaviour
{
    public GameObject ending;

    //private InputAction click;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        ending.SetActive(false);
        //click = InputSystem.actions.FindAction("Click");
    }

    // Update is called once per frame
    void Update()
    {
        // if (click.WasPressedThisFrame())
        // {
        //     exit();
        // }
    }

    // public void enter()
    // {
    //     ending.SetActive(true);
    // }
    
    public void exit()
    {
        ending.SetActive(false);
        Destroy(this);
    }
}
