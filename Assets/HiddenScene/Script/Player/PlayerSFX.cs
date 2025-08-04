using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip shootClip;
    public AudioClip hitClip;
    public AudioClip deathClip;
 
    public void PlayShootSFX()
    {
        audioSource.PlayOneShot(shootClip);
    }

    public void PlayHitSFX()
    {
        audioSource.PlayOneShot(hitClip);
    }

    public void PlayDeathSFX()
    {
        audioSource.PlayOneShot(deathClip);
    }
}
