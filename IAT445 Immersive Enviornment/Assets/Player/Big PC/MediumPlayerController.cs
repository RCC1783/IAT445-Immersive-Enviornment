using System;
using Oculus.Platform;
using UnityEngine;
using UnityEngine.InputSystem;

public class MediumBodyController : PlayerController_Base
{
    [SerializeField] private Transform handTransform;
    private Vector3 defaultHandLocation;
    private GameObject objectInHand = null;
    [SerializeField] private GameObject handRayTargetPoint;
    private GameObject handTarget = null;

    private InputAction enableMoveHandButton;
    private bool movingHand = false;
    private Vector3 objectOffset = Vector3.zero;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnStart();

        enableMoveHandButton = InputSystem.actions.FindAction("Unique 2");

        // objectTransform = Instantiate(new GameObject("objectTransform"), camera.transform);
        defaultHandLocation = handTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive == false) return;
        CameraUpdate();

        if (uniqueAction.WasPressedThisFrame())
        {
            PickUpObject();
        }

        if(objectInHand != null) //holding something
        {
            handRayTargetPoint.SetActive(true);
            handRayTargetPoint.transform.position = handTransform.position;

            if(enableMoveHandButton.ReadValue<float>() > 0)
            {
                movingHand = true;
                Vector2 input = moveInp.ReadValue<Vector2>();
                objectOffset += new Vector3(0, input.y, input.x) * 10;
            }
            else
            {
                movingHand = false;
            }

            if (OVRManager.isHmdPresent)
            {
                objectInHand.transform.position = 
                    handTransform.position
                    + camTransform.right.normalized * leftVRController.transform.localPosition.x
                    + new Vector3(0, objectOffset.y + (1000 * leftVRController.transform.localPosition.y), 0)
                    + camTransform.forward.normalized * objectOffset.z;
            }
            else
            {
                objectInHand.transform.position = 
                    handTransform.position 
                    + objectOffset;
            }

            // objectInHand.transform.position = handTransform.position + objectOffset;
            // rayTargetPoint.transform.position = inHandPos.position;
        }

        
    }

    void FixedUpdate()
    {
        if (isActive == false) {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        if(!movingHand) Movement();
        ApplyGravity();

        rb.linearVelocity = linVel;

        ShrinkRay();

        if(objectInHand == null) DoHandRaycast();
    }

    void ShrinkRay()
    {
        Transform rayEmmiter = camTransform.transform;
        RaycastHit hitInfo;
        GameObject target = null;

        if (OVRManager.isHmdPresent)
        {
            rayEmmiter = rightVRController.transform;
        }

        if (Physics.Raycast(rayEmmiter.position, rayEmmiter.forward.normalized, out hitInfo, rayDistance))
        {
            target = hitInfo.collider.gameObject;
            ObjectComponent OC = target.GetComponent<ObjectComponent>();
            if (rayTargetPoint != null && OC != null)
            {
                rayTargetPoint.SetActive(true);
                
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
                    OC.updateWeight();
                    return;
                }
            }
        }
        else if (rayTargetPoint != null)
        {
            rayTargetPoint.SetActive(false);
        }
    }

    private void DoHandRaycast()
    {
        Transform rayEmmiter = handTransform;
        RaycastHit hitInfo;
        GameObject target = null;

        if (OVRManager.isHmdPresent)
        {
            rayEmmiter = leftVRController.transform;
        }

        if (Physics.Raycast(rayEmmiter.position, rayEmmiter.forward.normalized, out hitInfo, rayDistance))
        {
            target = hitInfo.collider.gameObject;
            ObjectComponent OC = target.GetComponent<ObjectComponent>();
            if (handRayTargetPoint != null && OC != null)
            {
                handRayTargetPoint.SetActive(true);

                if (objectInHand != null && target == objectInHand) return;

                handRayTargetPoint.transform.position = hitInfo.point;

                Renderer rayTargetRenderer = handRayTargetPoint.GetComponent<Renderer>();

                rayTargetRenderer.material.SetColor("_BaseColor", Color.blue);

                handTarget = target;
            }
        }
        else if (rayTargetPoint != null)
        {
            handRayTargetPoint.SetActive(false);
            handTarget = null;
        }

        // handTarget = null;
        // if (target == null) return;
        // handTarget = target;
    }
    
    private void PickUpObject()
    {
        //If holding object, drop it
        if (objectInHand != null)
        {
            objectInHand.GetComponent<Rigidbody>().isKinematic = false;
            objectInHand.GetComponent<Collider>().enabled = true;
            objectInHand = null;
            rayTargetPoint.transform.position = transform.position;
            objectOffset = Vector3.zero;
            return;
        }

        if(handTarget == null) return;
        ObjectComponent oc = handTarget.GetComponent<ObjectComponent>();
        Weight targetWeight;

        if (oc == null) targetWeight = Weight.IMMOVABLE;
        else targetWeight = oc.weight;

        if (targetWeight <= Weight.MEDIUM)
        {
            handTarget.transform.position = handTransform.position;
            objectInHand = handTarget;
            objectInHand.GetComponent<Rigidbody>().isKinematic = true;
            objectInHand.GetComponent<Collider>().enabled = false;
            return;
        }
    }
}
