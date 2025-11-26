using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Oculus.Platform.Models;

public class TinyBodyController : PlayerController_Base
{
    [SerializeField] private float webFireForce = 2;
    [SerializeField] private int maxWebObjects = 50;

    [SerializeField] private GameObject webTargetPrefab;
    private GameObject webTarget = null;
    private WebTarget webTargetScript;

    [SerializeField] private GameObject webMeshPrefab;

    private GameObject webStuckOn = null;

    private List<GameObject> webObjects = new List<GameObject>();

    private InputAction pullOnWebAction;

    [SerializeField] private int webPullForce = 10;

    [SerializeField] private GameObject closestPointMarker;

    void Start()
    {
        OnStart();

        pullOnWebAction = InputSystem.actions.FindAction("Unique 2");
    }

    void Update()
    {
        if (!isActive) return;

        CameraUpdate();

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
        if (isActive == false) {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        Movement();
        Gravity();

        if (webTarget != null)
        {
            UpdateWeb();
        }

        GrowRay();

        rb.linearVelocity = vel;
    }

    void ShootWeb()
    {
        Debug.Log("Web Fired");

        Transform rayEmmiter = camTransform.transform;
        if (OVRManager.isHmdPresent)
        {
            rayEmmiter = leftVRController.transform;
            Debug.Log("HMD Present");
        }
        else
        {
            Debug.Log("HMD Not Present");
        }

        if (webTarget != null)
        {
            DestroyWeb();
        }

        // webTarget = Instantiate(webTargetPrefab, camera.transform.position, camera.transform.rotation);
        webTarget = Instantiate(webTargetPrefab, rayEmmiter.position, rayEmmiter.rotation);
        webTargetScript = webTarget.GetComponent<WebTarget>();
        webTargetScript.Init(this);

        Rigidbody webTarget_rb = webTarget.GetComponent<Rigidbody>();

        // webTarget_rb.linearVelocity = camera.transform.TransformDirection(Vector3.forward * webFireForce);
        webTarget_rb.linearVelocity = rayEmmiter.TransformDirection(Vector3.forward) * webFireForce;

    }
    void UpdateWeb()
    {
        if (webTarget == null) return;

        Transform rayEmmiter = transform;
        if (OVRManager.isHmdPresent)
        {
            rayEmmiter = leftVRController.transform;
        }

        Vector3 dispVec = webTarget.transform.position - transform.position;

        int webMeshCount = Mathf.CeilToInt(dispVec.magnitude) * 5 ;

        if(webMeshCount >= maxWebObjects * 10)
        {
            DestroyWeb();
            return;
        }

        if (webMeshCount > maxWebObjects)
        {
            webMeshCount = maxWebObjects;
        }

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
                = Vector3.Lerp(rayEmmiter.transform.position, webTarget.transform.position, i / (float)(webObjects.Count - 1));
            // Debug.Log("WebObject " + i + "/" + (webObjects.Count - 1) + " drawn at " + webObjects[i].transform.position);
        }
    }

    public void StickWebToObj(GameObject targ)
    {
        webStuckOn = targ;
    }

    private void DestroyWeb()
    {
        Destroy(webTarget);
        while (webObjects.Count > 0)
        {
            GameObject tmp = webObjects[webObjects.Count - 1];
            webObjects.Remove(tmp);
            Destroy(tmp);
            webStuckOn = null;
        }
    }

    private void PullOnWeb()
    {
        if (webStuckOn == null)
        {
            DestroyWeb();
            return;
        }

        Debug.Log("Pulled");
        
        Vector3 dir = webTarget.transform.position - transform.position;

        if (webTargetScript.targetWeight == Weight.NULL)
        {
            // rb.AddForce(dir.normalized * webPullForce * 10, ForceMode.Impulse);
            transform.position = webTarget.transform.position;
            rb.linearVelocity = Vector3.zero;
            DestroyWeb();
            return;
        }

        if (webTargetScript.targetWeight >= Weight.MEDIUM)
        {
            // rb.AddForce(dir.normalized * webPullForce * 10, ForceMode.Impulse);
            // GetClosestUpwardEdge(webStuckOn.GetComponent<MeshFilter>().mesh);
            transform.position = webTarget.transform.position;
            rb.linearVelocity = Vector3.zero;
            DestroyWeb();
        }

        if (webTargetScript.targetWeight <= Weight.LIGHT)
        {
            dir = webTarget.transform.position - leftVRController.transform.position;
            Rigidbody targetRB = webStuckOn.GetComponent<Rigidbody>();
            if (targetRB == null) return;
            targetRB.AddForce(dir.normalized * -1 * webPullForce * 100 / targetRB.mass);
            DestroyWeb(); 
        }
    }

    void GrowRay()
    {
        Transform rayEmmiter = camTransform.transform;
        RaycastHit hitInfo;

        if (OVRManager.isHmdPresent)
        {
            rayEmmiter = rightVRController.transform;
        }

        if (Physics.Raycast(rayEmmiter.position, rayEmmiter.TransformDirection(Vector3.forward), out hitInfo, rayDistance, layerMask))
        {
            if (rayTargetPoint != null && isActive)
            {
                GameObject target = hitInfo.collider.gameObject;

                rayTargetPoint.transform.position = hitInfo.point;

                Renderer rayTargetRenderer = rayTargetPoint.GetComponent<Renderer>();

                //Grow Target
                rayTargetRenderer.material.SetColor("_BaseColor", Color.green);
                if (useRayAction.ReadValue<float>() > 0)
                {
                    ObjectComponent oc = target.GetComponent<ObjectComponent>();
                    target.transform.localScale += scaleVec;
                    target.transform.position += new Vector3(0, scalingFac, 0) / 2f;
                    
                    oc.updateWeight();
                }
            }
        }
        else if (rayTargetPoint != null)
        {
            rayTargetPoint.transform.position = body.transform.position;
        }
    }

    private Vector3[] GetClosestUpwardEdge(Mesh mesh)
    {
        if (mesh == null) return new Vector3[0];

        Vector3[] verts = mesh.vertices;

        transform.TransformPoints(verts);

        Vector3 closestPoint = new Vector3(999, 999, 999);
        foreach (Vector3 vert in verts)
        {
            if(Vector3.Distance(vert, webTarget.transform.position) < Vector3.Distance(closestPoint, webTarget.transform.position))
            {
                closestPoint = vert;
            }
        }

        // Vector3_BST bst = new Vector3_BST();

        // for (int i = 0; i < verts.Length - 1; i++)
        // {
        //     Vector3_BST_Node newNode = new Vector3_BST_Node(verts[i], i, webTarget.transform.position);

        //     bst.Insert(newNode);
        // }

        if(closestPointMarker != null)
        {
            closestPointMarker.transform.position = closestPoint;
        }

        return new Vector3[2];
    }

}

// class Vector3_BST_Node
// {
//     public Vector3 pos;
//     public float distance; //distance from 
//     public int index; //index in mesh vert array

//     public Vector3_BST_Node left;
//     public Vector3_BST_Node right;

//     public Vector3_BST_Node(Vector3 pos, int index, Vector3 refPoint)
//     {
//         this.pos = pos;
//         this.index = index;

//         distance = Vector3.Distance(refPoint, pos);
//     }
// }


// class Vector3_BST
// {
//     public Vector3_BST_Node root;

//     public void Insert(Vector3_BST_Node newNode)
//     {
//         if (root == null)
//         {
//             root = newNode;
//             return;
//         }

//         Vector3_BST_Node current = root;
//         while (current != null)
//         {
//             if (current.distance >= newNode.distance && current.left != null)
//             {
//                 current = current.left;
//             }
//             else if (current.distance < newNode.distance && current.right != null)
//             {
//                 current = current.right;
//             }
//             else break;
//         }

//         if (current.distance > newNode.distance)
//         {
//             current.left = newNode;
//         }
//         else
//         {
//             current.right = newNode;
//         }
//     }

//     public int GetIndexOfClosestVertex()
//     {
//         Vector3_BST_Node current = root;

//         while (current.left != null)
//         {
//             current = current.left;
//         }

//         return current.index;
//     }

//     public Vector3[] GetTwoClosestVertPositions()
//     {
//         Vector3_BST_Node current = root;

//         while (current.left.left != null)
//         {
//             current = current.left;
//         }

//         Vector3[] ret = new Vector3[2];
//         ret[0] = current.left.pos;
//         ret[1] = current.pos;
//         return ret;
//     }
    
//     public void PrintTree(Vector3_BST_Node node)
//     {
//         if (node == null)
//         {
//             return;
//         }

//         PrintTree(node.left);
//         Debug.Log(node.distance + " ");
//         PrintTree(node.right);
//     }

// }
