using UnityEngine;

public class TempSFX : MonoBehaviour
{
    [SerializeField] private AudioClip doorSFX;
    [SerializeField] private AudioSource doorSoundObj;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SFX()
    {
        //SoundManager.instance.PlaySFX(doorSFX, transform, 1f);
        doorSoundObj.Play();
    }
}
