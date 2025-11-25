using UnityEngine;

public class BigButton2 : MonoBehaviour
{

    public GameObject smallBridges;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        smallBridges.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

      private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            smallBridges.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            smallBridges.SetActive(false);
        }
    }
}
