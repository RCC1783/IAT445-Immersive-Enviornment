using UnityEngine;

public class ButtonWebInteract : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string triggerName = "PuzzleSolved";

    [Header("Button Settings")]
    [SerializeField] private GameObject lightObject;   // <-- your light dome object
    [SerializeField] private Material litMaterial;     // material when lit
    [SerializeField] private Material unlitMaterial;   // material when off
    [SerializeField] private string playerTag = "Web";

    [Header("Extra")]
    [SerializeField] private GameObject dome;          // object that disappears when puzzle solved

    private bool isPressed = false;

    private Renderer lightRenderer;

    // Shared between all buttons
    private static int pressedCount = 0;
    private static int totalButtons = 3;


    private void Start()
    {
        // Cache renderer once
        lightRenderer = lightObject.GetComponent<Renderer>();

        // Start unlit
        if (lightRenderer != null){
             lightRenderer.material = unlitMaterial;
        }
           

            animator.SetTrigger("PuzzleSolved");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && !isPressed)
        {
            isPressed = true;

            // Change material to lit
            lightRenderer.material = litMaterial;

            pressedCount++;
            CheckIfAllPressed();
        }
    }



    private void CheckIfAllPressed()
{
    Debug.Log($"PressedCount: {pressedCount}/{totalButtons}");

    if (pressedCount == totalButtons)
    {
        Debug.Log("PUZZLE SOLVED -> Triggering animation!");
        animator.SetTrigger(triggerName);
        dome.SetActive(false);
    }
}


}
