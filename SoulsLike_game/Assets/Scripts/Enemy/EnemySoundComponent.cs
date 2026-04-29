using UnityEngine;
using UnityEngine.Audio;

public class EnemySoundComponent : MonoBehaviour
{
    [SerializeField] AudioClip hit;
    [SerializeField] AudioClip takeDamage;
    [SerializeField] AudioClip die;

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = false;
    }

    public void PlayHit() => PlaySound(hit);
    public void PlayHurt() => PlaySound(takeDamage);
    public void PlayDeath() => PlaySound(die);

    private void PlaySound(AudioClip sound)
    {
        if (source != null && sound != null)
            source.PlayOneShot(sound);
    }
}
