using UnityEngine;
using UnityEngine.InputSystem;

public class SizeUI : MonoBehaviour
{
    public static SizeUI instance;
    public bool big = false;

    public GameObject bigPicture;

    public GameObject smallPicture;

    private GameObject playerMesh;

    private InputAction change;
    
    [SerializeField] private AudioClip switchSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        smallPicture.SetActive(true);
        change = InputSystem.actions.FindAction("Change Size");
        big = false;
        playerMesh = GameObject.Find("dnaut5");
        playerMesh.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.Shift))
        // if(GameObject.Find("Big Player").GetComponent<h>()
        //     .isActive = true)
        //Debug.Log(big);
        if(change.WasPressedThisFrame())
        {
            //SoundManager.instance.PlaySFX(switchSound, transform, 1f);
            if (big)
            {
                smallUI();
            }
            else
            {
                bigUI();
            } 
        }
    }

    public void bigUI()
    {
        //Debug.Log("big");
        smallPicture.SetActive(false);
        bigPicture.SetActive(true);

        playerMesh.SetActive(false);

        big = true;
    }
    
    public void smallUI()
    {
        //Debug.Log("small");
        bigPicture.SetActive(false);
        smallPicture.SetActive(true);

        playerMesh.SetActive(true);

        big = false;
    }
}
