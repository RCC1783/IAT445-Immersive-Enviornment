using UnityEngine;

public class WebInteract : MonoBehaviour
{
    [SerializeField] private Animator Animator;
    [SerializeField] private string parameterName = "character_nearby";
    [SerializeField] private string playerTag = "Web";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Animator.SetBool(parameterName, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Animator.SetBool(parameterName, false);
        }
    }
}
