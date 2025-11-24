using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{

    [SerializeField] private string playerTag = "Player";

    public string scene;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
             SceneManager.LoadScene(scene);
        }
    }
}
