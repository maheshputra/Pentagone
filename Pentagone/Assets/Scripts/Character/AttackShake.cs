using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AttackShake : MonoBehaviour
{
    [SerializeField] private float shakeTime;
    [SerializeField] private float amplitudeGain;
    [SerializeField] private float frequencyGain;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            CinemachineVirtualCamera cm = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
            StopCoroutine("ShakeCamera");
            StartCoroutine(ShakeCamera(cm));
        }
    }

    IEnumerator ShakeCamera(CinemachineVirtualCamera cm)
    {
        cm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitudeGain;
        cm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequencyGain;
        yield return new WaitForSeconds(shakeTime);
        cm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
    }
}
