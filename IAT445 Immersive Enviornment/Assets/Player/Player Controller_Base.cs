using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController_Base : MonoBehaviour
{
    [SerializeField] protected bool isActive = false;

    [SerializeField] protected bool godMode = false;

    [SerializeField] protected GameObject body;

    protected Rigidbody rb;
    protected Collider collider;

    [SerializeField] protected Camera camera;

    //Input Actions
    protected InputAction moveInp;
    protected InputAction lookInp;
    protected InputAction uniqueAction;

    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float lookSpeed = 100f;

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
        
        uniqueAction = InputSystem.actions.FindAction("Unique 1");

        rb = GetComponent<Rigidbody>();

        if (godMode)
        {
            rb.useGravity = false;
            moveSpeed *= 1.5f;
        }
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

        camera.transform.localRotation = Quaternion.Euler(camRot.y, camRot.x, 0);
    }

    void FixedUpdate()
    {
        Movement();
    }

    protected virtual void Movement()
    {
        if (isActive == false) return;

        Vector2 input = moveInp.ReadValue<Vector2>();
        Transform camTransform = camera.transform;

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

        Vector3 moveDir = (forwardVec + rightVec).normalized * moveSpeed * Time.fixedDeltaTime;
        transform.Translate(moveDir);
    }
    
    public void SetActive(bool status)
    {
        isActive = status;
        camera.enabled = status;
    }
}
