using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource[0].Play();
    }

    // Update is called once per frame
    void Update()
    {
        audioSource[0].pitch = 0.9f + 0.01f*Mathf.FloorToInt(GameManager.Instance.CalculateGameSpeed()*2);
    }

}
