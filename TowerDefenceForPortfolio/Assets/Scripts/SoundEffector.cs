using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffector : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip trackExposionSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PLayTrackExposionSound()
    {
        audioSource.PlayOneShot(trackExposionSound);
    }
}
