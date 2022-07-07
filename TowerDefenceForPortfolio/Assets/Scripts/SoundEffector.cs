using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffector : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _trackExposionSound, _buildingSound, _saleSound;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PLayTrackExposionSound()
    {
        _audioSource.PlayOneShot(_trackExposionSound);
    }

    public void PLayBuildingSound()
    {
        _audioSource.PlayOneShot(_buildingSound);
    }

    public void PLaySaleSound()
    {
        _audioSource.PlayOneShot(_saleSound);
    }
}
