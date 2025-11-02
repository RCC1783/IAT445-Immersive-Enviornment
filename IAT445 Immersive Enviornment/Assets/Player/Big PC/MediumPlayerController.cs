using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MediumBodyController : PlayerController_Base
{
    [SerializeField] private float rayDistance = 2;
    [SerializeField] private float scalingFac = 0.02f;
    private Vector3 scaleVec;
    [SerializeField] private LayerMask layerMask;

    private enum RayModes {SHRINK, GROW, MOVE};
    private RayModes rayMode = RayModes.SHRINK;

    private InputAction rayModeButton;

    [SerializeField] private GameObject rayTargetPoint;

    [SerializeField] private Transform handTransform;
    private GameObject objectInHand = null;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnStart();

        rayModeButton = InputSystem.actions.FindAction("Unique 2");

        scaleVec = new Vector3(scalingFac, scalingFac, scalingFac);

        // objectTransform = Instantiate(new GameObject("objectTransform"), camera.transform);
    }

    // Update is called once per frame
    void Update()
    {
        CameraUpdate();

        if (rayModeButton.WasPressedThisFrame())
        {
            rayMode = (RayModes)(((int)rayMode + 1) % Enum.GetValues(typeof(RayModes)).Length);
        }

        if(objectInHand != null) //holding something
        {
            objectInHand.transform.position = handTransform.position;
            rayTargetPoint.transform.position = handTransform.position;

            if (uniqueAction.WasPressedThisFrame())
            {
                objectInHand.GetComponent<Rigidbody>().isKinematic = false;
                objectInHand = null;
                rayTargetPoint.transform.position = transform.position;
            }
            return;
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
        RaycastHit hitInfo;

        if (Physics.Raycast(camera.transform.position, camera.transform.forward.normalized, out hitInfo, rayDistance, layerMask))
        {
            if (rayTargetPoint != null && isActive)
            {
                GameObject target = hitInfo.collider.gameObject;
                if (target == objectInHand) return;

                rayTargetPoint.transform.position = hitInfo.point;

                Renderer rayTargetRenderer = rayTargetPoint.GetComponent<Renderer>();

                ObjectComponent oc = target.GetComponent<ObjectComponent>();
                Weight targetWeight;
                if (oc == null) targetWeight = Weight.IMMOVABLE;
                else targetWeight = oc.weight;

                switch (rayMode)
                {
                    case RayModes.SHRINK:
                        rayTargetRenderer.material.SetColor("_BaseColor", Color.red);
                        if (uniqueAction.ReadValue<float>() > 0)
                        {
                            target.transform.localScale -= scaleVec;
                            if (target.transform.localScale.x <= 0)
                            {
                                target.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                            }
                        }
                        break;
                    case RayModes.GROW:
                        rayTargetRenderer.material.SetColor("_BaseColor", Color.green);
                        if (uniqueAction.ReadValue<float>() > 0)
                        {
                            target.transform.localScale += scaleVec;
                            target.transform.position += new Vector3(0, scalingFac, 0) / 2f;
                        }
                        break;
                    case RayModes.MOVE:
                        rayTargetRenderer.material.SetColor("_BaseColor", Color.blue);

                        if (uniqueAction.WasPressedThisFrame() && targetWeight < Weight.MEDIUM)
                        {
                            handTransform.position = rayTargetPoint.transform.position;
                            objectInHand = target;
                            objectInHand.GetComponent<Rigidbody>().isKinematic = true;
                            return;
                        }
                        break;
                    default:
                        rayTargetRenderer.material.SetColor("_BaseColor", Color.white);
                        break;
                }
            }
        }
        else if (rayTargetPoint != null)
        {
            rayTargetPoint.transform.position = body.transform.position;
        }
    }
}
