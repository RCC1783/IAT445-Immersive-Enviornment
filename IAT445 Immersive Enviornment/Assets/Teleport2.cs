using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Teleport2 : MonoBehaviour
{

    // [SerializeField] private string playerTag = "Player";

    public string loadScene;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(loadScene);
        }
    }
}
