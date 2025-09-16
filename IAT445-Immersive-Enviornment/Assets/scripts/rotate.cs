using UnityEngine;

public class rotate : MonoBehaviour
{
    public float rotationDegrees = 10;

    private float direction = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction = Random.Range(-1, 2);
        if (direction == 0)
        {
            direction += 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationDegrees * direction * Time.deltaTime, 0);
    }
}
