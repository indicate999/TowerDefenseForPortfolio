using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffector : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip trackExposionSound, buildingSound, saleSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PLayTrackExposionSound()
    {
        audioSource.PlayOneShot(trackExposionSound);
    }

    public void PLayBuildingSound()
    {
        audioSource.PlayOneShot(buildingSound);
    }

    public void PLaySaleSound()
    {
        audioSource.PlayOneShot(saleSound);
    }
}
