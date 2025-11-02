using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MediumBodyController : PlayerController_Base
{
    [SerializeField] private Transform handTransform;
    private GameObject objectInHand = null;

    private RaycastHit hitInfo;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnStart();

        // grabObjectButton = InputSystem.actions.FindAction("Unique");

        // objectTransform = Instantiate(new GameObject("objectTransform"), camera.transform);
    }

    // Update is called once per frame
    void Update()
    {
        CameraUpdate();

        if (uniqueAction.WasPressedThisFrame())
        {
            PickUpObject();
        }

        if(objectInHand != null) //holding something
        {
            objectInHand.transform.position = handTransform.position;
            rayTargetPoint.transform.position = handTransform.position;
        }
    }

    void FixedUpdate()
    {
        Movement();

        DoRaycast();
    }

    void DoRaycast()
    {
        if (objectInHand != null) return;

        if (Physics.Raycast(camera.transform.position, camera.transform.forward.normalized, out hitInfo, rayDistance, layerMask))
        {
            if (rayTargetPoint != null && isActive)
            {
                GameObject target = hitInfo.collider.gameObject;
                if (target == objectInHand) return;

                rayTargetPoint.transform.position = hitInfo.point;

                Renderer rayTargetRenderer = rayTargetPoint.GetComponent<Renderer>();

                rayTargetRenderer.material.SetColor("_BaseColor", Color.red);

                //Scale Object
                if (useRayAction.ReadValue<float>() > 0)
                {
                    target.transform.localScale -= scaleVec;
                    if (target.transform.localScale.x <= 0)
                    {
                        target.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                    }
                    return;
                }
            }
        }
        else if (rayTargetPoint != null)
        {
            rayTargetPoint.transform.position = body.transform.position;
        }
    }
    
    private void PickUpObject()
    {
        //If holding object, drop it
        if (objectInHand != null)
        {
            objectInHand.GetComponent<Rigidbody>().isKinematic = false;
            objectInHand = null;
            rayTargetPoint.transform.position = transform.position;
            return;
        }

        if (hitInfo.collider == null) return;
        GameObject target = hitInfo.collider.gameObject;

        ObjectComponent oc = target.GetComponent<ObjectComponent>();
        Weight targetWeight;
        if (oc == null) targetWeight = Weight.IMMOVABLE;
        else targetWeight = oc.weight;
        if (targetWeight <= Weight.MEDIUM)
        {
            handTransform.position = rayTargetPoint.transform.position;
            objectInHand = target;
            objectInHand.GetComponent<Rigidbody>().isKinematic = true;
            return;
        }

    }
}
