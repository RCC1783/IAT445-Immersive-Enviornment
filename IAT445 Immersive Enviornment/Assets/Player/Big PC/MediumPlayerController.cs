using System;
using Oculus.Platform;
using UnityEngine;
using UnityEngine.InputSystem;

public class MediumBodyController : PlayerController_Base
{
    [SerializeField] private Transform handTransform;
    private GameObject objectInHand = null;
    
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
        if (isActive == false) return;
        CameraUpdate();

        if (OVRManager.isHmdPresent)
        {
            handTransform = leftVRController.transform;
        }

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
        if (isActive == false) return;
        Movement();

        ShrinkRay();
    }

    void ShrinkRay()
    {
        Transform rayEmmiter = camTransform.transform;
        RaycastHit hitInfo;

        if (OVRManager.isHmdPresent)
        {
            rayEmmiter = rightVRController.transform;
        }

        if (Physics.Raycast(rayEmmiter.position, rayEmmiter.forward.normalized, out hitInfo, rayDistance, layerMask))
        {
            if (rayTargetPoint != null)
            {
                rayTargetPoint.SetActive(true);
                GameObject target = hitInfo.collider.gameObject;

                if (objectInHand != null && target == objectInHand) return;

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
            rayTargetPoint.SetActive(false);
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

        Transform rayEmmiter = camTransform.transform;
        RaycastHit hitInfo;
        GameObject target = null;

        if (OVRManager.isHmdPresent)
        {
            rayEmmiter = leftVRController.transform;
        }

        if (Physics.Raycast(rayEmmiter.position, rayEmmiter.forward.normalized, out hitInfo, rayDistance, layerMask))
        {
            if (rayTargetPoint != null)
            {
                rayTargetPoint.SetActive(true);
                target = hitInfo.collider.gameObject;

                if (objectInHand != null && target == objectInHand) return;

                rayTargetPoint.transform.position = hitInfo.point;

                Renderer rayTargetRenderer = rayTargetPoint.GetComponent<Renderer>();

                rayTargetRenderer.material.SetColor("_BaseColor", Color.red);
            }
        }
        else if (rayTargetPoint != null)
        {
            rayTargetPoint.SetActive(false);
        }

        if (target == null) return;

        ObjectComponent oc = target.GetComponent<ObjectComponent>();
        Weight targetWeight;

        if (oc == null) targetWeight = Weight.IMMOVABLE;

        else targetWeight = oc.weight;
        if (targetWeight <= Weight.MEDIUM)
        {
            target.transform.position = handTransform.position;
            objectInHand = target;
            objectInHand.GetComponent<Rigidbody>().isKinematic = true;
            return;
        }

    }
}
