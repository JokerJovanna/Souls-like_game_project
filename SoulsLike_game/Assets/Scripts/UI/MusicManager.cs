using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private float mainVolume = 0.1f;
    [SerializeField] private float pauseVolume = 0.05f;

    public float MainVolume => mainVolume;
    public float PauseVolume => pauseVolume;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = musicClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = mainVolume;
    }

    public void Play() => audioSource.Play();
    public void Stop() => audioSource.Stop();
    public void SetVolume(float v) => audioSource.volume = v;
}