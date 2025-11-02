using UnityEngine;

public enum Weight {NULL, VERY_LIGHT, LIGHT, MEDIUM, HEAVY, VERY_HEAVY, IMMOVABLE}
public class ObjectComponent : MonoBehaviour
{
    public Weight weight = Weight.MEDIUM;

    private Vector3 startScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localScale.x <= startScale.x / 2)
        {
            weight = Weight.LIGHT;
        }
    }
}
