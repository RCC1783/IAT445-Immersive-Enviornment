using UnityEngine;

public class DoorProximity : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private string parameterName = "character_nearby";
    [SerializeField] private string playerTag = "Player";

    //[SerializeField] private AudioClip doorOpenSFX;
    [SerializeField] private AudioSource doorOpenObj;

    [SerializeField] private AudioSource doorCloseObj;

    //[SerializeField] private AudioClip doorCloseSFX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            //SoundManager.instance.PlaySFX(doorOpenSFX, transform, 1f);
            doorOpenObj.Play();
            doorAnimator.SetBool(parameterName, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            //SoundManager.instance.PlaySFX(doorCloseSFX, transform, 1f);
            doorCloseObj.Play();
            doorAnimator.SetBool(parameterName, false);
        }
    }
}
