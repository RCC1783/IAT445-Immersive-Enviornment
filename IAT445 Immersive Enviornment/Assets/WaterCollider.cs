using UnityEngine;

public class WaterCollider : MonoBehaviour
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

    void OnTriggerEnter() {
        if(gameObject.CompareTag("Player")) {
            waterCollider.enabled = false;
        }

    }

    void OnTriggerExit() {
         if(gameObject.CompareTag("Player")) {
            waterCollider.enabled = true;
        }
    }
}
