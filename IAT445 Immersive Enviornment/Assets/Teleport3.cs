using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport3 : MonoBehaviour
{

    // [SerializeField] private string playerTag = "Player";
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
             SceneManager.LoadScene("level 3");
        }
    }
}
