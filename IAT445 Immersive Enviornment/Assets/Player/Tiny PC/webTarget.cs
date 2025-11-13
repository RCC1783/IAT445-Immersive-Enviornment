using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

public class WebTarget : MonoBehaviour
{
    [SerializeField] private string moveableObjectTag;
    [SerializeField] private string immovableObjectTag;

    private TinyBodyController parent;

    Rigidbody rb;

    public Weight targetWeight = Weight.NULL;

    public void Init(TinyBodyController parent)
    {
        this.parent = parent;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject gameObject = collision.collider.gameObject;
        ObjectComponent objectComponent = gameObject.GetComponent<ObjectComponent>();

        if (objectComponent == null)
        {
            targetWeight = Weight.NULL;
        }
        else targetWeight = objectComponent.weight;

        parent.StickWebToObj(gameObject);
        rb.isKinematic = true;
    }
}
