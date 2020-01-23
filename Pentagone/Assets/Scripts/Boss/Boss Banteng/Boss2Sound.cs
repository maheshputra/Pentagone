using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Sound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip charge;
    [SerializeField] private AudioClip steady;
    [SerializeField] private AudioClip breath;
    [SerializeField] private AudioClip crash;

    [Space(10)]
    [Header("Bool")]

    [SerializeField] private bool isCharging;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void ChargeSound() {
        if (!isCharging)
        {
            isCharging = true;
            //audioSource.PlayOneShot(charge);
        }
    }

    public void CrashSound() 
    {
        isCharging = false;
    }

    public void HitSound()
    {
        isCharging = false;
    }

    public void BreathSound() {
        audioSource.PlayOneShot(breath);
    }

    public void SteadySound()
    {
        audioSource.PlayOneShot(steady);
    }
}
