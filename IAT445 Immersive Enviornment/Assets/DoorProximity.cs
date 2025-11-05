using UnityEngine;

public class DoorProximity : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private string parameterName = "character_nearby";
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            doorAnimator.SetBool(parameterName, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            doorAnimator.SetBool(parameterName, false);
        }
    }
}
