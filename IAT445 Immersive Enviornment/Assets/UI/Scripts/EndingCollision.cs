using UnityEngine;

public class EndingCollision : MonoBehaviour
{
    public BoxCollider endingCollider;

    public GameObject endScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter() 
    {
        endScreen.SetActive(true);
    }
}
