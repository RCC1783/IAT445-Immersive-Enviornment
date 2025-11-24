using UnityEngine;

public class Level2Button : MonoBehaviour
{
    [SerializeField] private string parameterName = "character_nearby";
    [SerializeField] private string playerTag = "Web";

    public GameObject trigger;

    public GameObject bridge;

    void Start()
    {
      bridge.SetActive(false);  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            trigger.SetActive(false);
            bridge.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            trigger.SetActive(true);
        }
    }
}
