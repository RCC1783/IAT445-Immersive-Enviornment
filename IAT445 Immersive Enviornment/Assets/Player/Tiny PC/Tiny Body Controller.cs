using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class TinyBodyController : PlayerController_Base
{
    [SerializeField] private float webFireForce = 2;

    [SerializeField] private GameObject webTargetPrefab;
    private GameObject webTarget = null;
    private WebTarget webTargetScript;

    [SerializeField] private GameObject webMeshPrefab;

    private GameObject webStuckOn = null;

    private List<GameObject> webObjects = new List<GameObject>();

    private InputAction pullOnWebAction;

    [SerializeField] private int webPullForce = 10;

    void Start()
    {
        OnStart();

        pullOnWebAction = InputSystem.actions.FindAction("Unique 2");
    }

    void Update()
    {
        CameraUpdate();

        if (!isActive) return;

        if (uniqueAction.WasPressedThisFrame())
        {
            ShootWeb();
        }

        if (pullOnWebAction.WasPressedThisFrame())
        {
            Debug.Log("Pull button pressed");
            PullOnWeb();
        }
    }
    
    void FixedUpdate()
    {
        Movement();

        if (!isActive) return;

        if(webTarget != null)
        {
            UpdateWeb();
        }
    }

    void ShootWeb()
    {
        Debug.Log("Web Fired");

        if(webTarget != null)
        {
            Destroy(webTarget);
            
            while(webObjects.Count > 0)
            {
                GameObject tmp = webObjects[webObjects.Count - 1];
                webObjects.Remove(tmp);
                Destroy(tmp);
                webStuckOn = null;
            }
        }

        webTarget = Instantiate(webTargetPrefab, camera.transform.position, camera.transform.rotation);
        webTargetScript = webTarget.GetComponent<WebTarget>();
        webTargetScript.Init(this);

        Rigidbody webTarget_rb = webTarget.GetComponent<Rigidbody>();

        webTarget_rb.linearVelocity = camera.transform.TransformDirection(Vector3.forward * webFireForce); 
        
    }
    void UpdateWeb()
    {
        if (webTarget == null) return;

        Vector3 dispVec = webTarget.transform.position - transform.position;

        int webMeshCount = Mathf.CeilToInt(dispVec.magnitude) * 10;
        int webObjectsCount = webObjects.Count;

        if (webMeshCount != webObjectsCount)
        {
            while (webMeshCount > webObjectsCount)
            {
                GameObject tmp = Instantiate(webMeshPrefab, transform.position, transform.rotation);
                webObjects.Add(tmp);
                webObjectsCount = webObjects.Count;
            }

            while (webMeshCount < webObjectsCount)
            {
                GameObject tmp = webObjects[webObjects.Count - 1];
                webObjects.Remove(tmp);
                Destroy(tmp);
                webObjectsCount = webObjects.Count;
            }
        }


        for (int i = 0; i < webObjects.Count; i++)
        {
            webObjects[i].transform.position
                = Vector3.Lerp(transform.position, webTarget.transform.position, i / (float)(webObjects.Count - 1));
            // Debug.Log("WebObject " + i + "/" + (webObjects.Count - 1) + " drawn at " + webObjects[i].transform.position);
        }
    }

    public void StickWebToObj(GameObject targ)
    {
        webStuckOn = targ;
    }
    
    private void PullOnWeb()
    {
        if (webStuckOn == null) return;
        Debug.Log("Pulled");

        Vector3 dir = webTarget.transform.position - transform.position;

        if(webTargetScript.targetWeight == Weight.NULL)
        {
            return;
        }

        if (webTargetScript.targetWeight >= Weight.MEDIUM)
        {
            rb.linearVelocity = dir.normalized * webPullForce;
        }

        if (webTargetScript.targetWeight <= Weight.LIGHT)
        {
            Rigidbody targetRB = webStuckOn.GetComponent<Rigidbody>();
            if (targetRB == null) return;
            targetRB.AddForce(dir.normalized * -1 * webPullForce * 100 / targetRB.mass);
        }
        
        Destroy(webTarget);   
        while(webObjects.Count > 0)
        {
            GameObject tmp = webObjects[webObjects.Count - 1];
            webObjects.Remove(tmp);
            Destroy(tmp);
            webStuckOn = null;
        }
    }

}
