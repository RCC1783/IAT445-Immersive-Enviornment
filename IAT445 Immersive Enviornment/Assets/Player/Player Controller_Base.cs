using System;
using Oculus.Platform;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController_Base : MonoBehaviour
{
    [SerializeField] protected bool isActive = false;

    [SerializeField] protected bool godMode = false;

    [SerializeField] protected GameObject body;

    protected Rigidbody rb;
    protected Collider collider;

    // [SerializeField] protected GameObject camera;

    //Input Actions
    protected InputAction moveInp;
    protected InputAction lookInp;
    protected InputAction uniqueAction;
    protected InputAction useRayAction;

    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float lookSpeed = 100f;

    // Shrink/grow ray stuff
    [Header("Grow/Shrink Ray Settings")]
    [SerializeField] protected float rayDistance = 2;
    [SerializeField] protected GameObject rayTargetPoint;
    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected float scalingFac = 0.02f;
    protected Vector3 scaleVec;

    public Transform camTransform;
    public GameObject VRCam;
    public GameObject centerEyeAnchor;


    Vector2 camRot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnStart();
    }

    protected void OnStart()
    {
        moveInp = InputSystem.actions.FindAction("Move");
        lookInp = InputSystem.actions.FindAction("Look");

        uniqueAction = InputSystem.actions.FindAction("Unique");
        useRayAction = InputSystem.actions.FindAction("Size Ray");

        rb = GetComponent<Rigidbody>();

        if (godMode)
        {
            rb.useGravity = false;
            moveSpeed *= 1.5f;
        }

        scaleVec = new Vector3(scalingFac, scalingFac, scalingFac);

        // camera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CameraUpdate();
    }

    protected void CameraUpdate()
    {
        if (isActive == false) return;

        Vector2 lookDir = lookInp.ReadValue<Vector2>() * lookSpeed * Time.deltaTime;

        camRot.x += lookDir.x;
        camRot.y -= lookDir.y;

        // camera.transform.localRotation = Quaternion.Euler(camRot.y, camRot.x, 0);
        camTransform.transform.localRotation = Quaternion.Euler(0, camRot.x, 0);
        VRCam.transform.position = camTransform.position;
        VRCam.transform.localRotation = camTransform.transform.localRotation;
    }

    void FixedUpdate()
    {
        Movement();
    }

    protected virtual void Movement()
    {
        if (isActive == false) return;

        Vector2 input = moveInp.ReadValue<Vector2>();
        // Transform camTransform = camera.transform;

        Vector3 forwardVec;
        Vector3 rightVec;

        if (godMode)
        {
            forwardVec = camTransform.forward * input.y;
            rightVec = new Vector3(camTransform.right.x, 0, camTransform.right.z) * input.x;
        }
        else
        {
            forwardVec = new Vector3(camTransform.forward.x, 0, camTransform.forward.z) * input.y;
            rightVec = new Vector3(camTransform.right.x, 0, camTransform.right.z) * input.x;
        }

        Vector3 moveDir = (forwardVec + rightVec).normalized * moveSpeed;
       // transform.Translate(moveDir);
       rb.linearVelocity = moveDir;
    }

    public void SetActive(bool status)
    {
        isActive = status;
        // camera.SetActive(status);
    }
    
    
}
