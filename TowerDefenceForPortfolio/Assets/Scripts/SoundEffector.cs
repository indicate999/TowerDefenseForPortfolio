using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffector : MonoBehaviour
{
    //public AudioSource audioSource;
    private AudioSource audioSource;
    public AudioClip trackExposionSound;
    //public AudioClip[] turretAttackSounds;
    //public static bool[] isTurretSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        //isTurretSound = new bool[TurretController.towerCollidersArrayLenght];
    }

    public void PLayTrackExposionSound()
    {
        audioSource.PlayOneShot(trackExposionSound);
    }

    //public void PlayTurretAttackSound(int turretNum, int towerSideIndex)
    //{
    //    //audioSource.PlayOneShot(turretAttackSounds[turretNum]);
    //    StartCoroutine(LoopTurretSound(turretNum, towerSideIndex));
    //}

    //IEnumerator LoopTurretSound(int turretNum, int towerSideIndex)
    //{
    //    float length = turretAttackSounds[turretNum].length;
    //
    //    while (isTurretSound[towerSideIndex])//(TurretController.isTurretSound[towerSideIndex])
    //    {
    //        audioSource.PlayOneShot(turretAttackSounds[turretNum]);
    //        yield return new WaitForSeconds(length);
    //    }
    //}
}
