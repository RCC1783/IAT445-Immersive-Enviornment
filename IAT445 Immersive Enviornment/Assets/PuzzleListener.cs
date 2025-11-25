using UnityEngine;

public class PuzzleListener : MonoBehaviour
{
    [SerializeField] private int totalButtons = 3;
    [SerializeField] private string triggerName = "PuzzleSolved";

    //private ButtonWebInteract buttonWebInteract;

    private int pressedCount = 0;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
            Debug.LogError("[PuzzleListener] No Animator found on this object.");
    }

    private void OnEnable()
    {
        ButtonWebInteract.OnAnyButtonPressed += HandleButtonPressed;
    }

    private void OnDisable()
    {
        ButtonWebInteract.OnAnyButtonPressed -= HandleButtonPressed;
    }

    private void HandleButtonPressed()
    {
        pressedCount++;
        Debug.Log($"[PuzzleListener] Button pressed ({pressedCount}/{totalButtons})");

        if (pressedCount >= totalButtons)
        {
            Debug.Log("[PuzzleListener] PUZZLE COMPLETE -> Trigger animation");
            anim.SetTrigger(triggerName);
        }
    }
}
