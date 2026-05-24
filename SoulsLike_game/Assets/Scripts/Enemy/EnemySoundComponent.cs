using UnityEngine;
using UnityEngine.Audio;

public class EnemySoundComponent : MonoBehaviour
{
    [SerializeField] AudioClip attack_1;
    [SerializeField] AudioClip attack_2;
    [SerializeField] AudioClip attack_3;
    [SerializeField] AudioClip takeDamage;
    [SerializeField] AudioClip die;

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = false;
    }

    public void PlayAttack_1() => PlaySound(attack_1);
    public void PlayAttack_2() => PlaySound(attack_2);
    public void PlayAttack_3() => PlaySound(attack_3);
    public void PlayHurt() => PlaySound(takeDamage);
    public void PlayDeath() => PlaySound(die);

    private void PlaySound(AudioClip sound)
    {
        if (source != null && sound != null)
            source.PlayOneShot(sound);
    }
}
