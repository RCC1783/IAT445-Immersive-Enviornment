using UnityEngine;

public class WaterScript : MonoBehaviour
{

    private BoxCollider waterCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waterCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("BridgeObject"))
        {
            waterCollider.enabled = false;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("BridgeObject"))
        {
            waterCollider.enabled = true;
        }
    }
}
