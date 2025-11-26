using System;
using UnityEngine;

public enum Weight {NULL, VERY_LIGHT, LIGHT, MEDIUM, HEAVY, VERY_HEAVY, IMMOVABLE}
public class ObjectComponent : MonoBehaviour
{
    public Weight weight = Weight.MEDIUM;

    private Vector3 startScale;
    private Weight startWeight;

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startScale = transform.localScale;
        startWeight = weight;
        
        rb = GetComponent<Rigidbody>();
        if(rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    void FixedUpdate()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, Vector3.down, out hit, transform.localScale.magnitude))
        {
            return;
        }
        else
        {
            rb.linearVelocity -= new Vector3(0, 100, 0);
        }
    }

    public void updateWeight()
    {
        if (transform.localScale.x <= startScale.x / 4)
        {
            weight = startWeight - 2;
            if(weight < Weight.VERY_LIGHT)
            {
                weight = Weight.VERY_LIGHT;
            }
        }
        else if(transform.localScale.x <= startScale.x / 2)
        {
            weight = startWeight - 1;
            if(weight <= 0)
            {
                weight = Weight.VERY_LIGHT;
            }
        }
        else if(transform.localScale.x >= startScale.x * 4)
        {
            weight = startWeight + 2;
            if(weight > Weight.VERY_HEAVY)
            {
                weight = Weight.VERY_LIGHT;
            }
        }
        else if(transform.localScale.x >= startScale.x * 2)
        {
            weight = startWeight + 1;
            if(weight > Weight.VERY_HEAVY)
            {
                weight = Weight.VERY_HEAVY;
            }
        }
    }
}
