using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeAmplitude = 1.2f;
    public float shakeDuration = 2.5f;
    public float shakeFrequency = 2.0f;

    private float shakeElapsedTime = 0f;

    private Vector3 originCamera;
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    public void Shake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCameraNoise = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();

        virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
        virtualCameraNoise.m_FrequencyGain = shakeFrequency;

        StartCoroutine(CoStopShake());
    }

    private IEnumerator CoStopShake()
    {
        shakeElapsedTime = shakeDuration;
        var maxAmplitude = shakeAmplitude;
        var maxFrequency = shakeFrequency;
        while(shakeElapsedTime > 0f)
        {
            shakeElapsedTime -= Time.deltaTime;

            virtualCameraNoise.m_AmplitudeGain = Mathf.Lerp(0f, maxAmplitude, shakeElapsedTime);
            virtualCameraNoise.m_FrequencyGain = Mathf.Lerp(0f, maxAmplitude, shakeElapsedTime);

            yield return null;
        }
    }
}
