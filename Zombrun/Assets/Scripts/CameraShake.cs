using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraShake : Singleton<CameraShake>
{
    [SerializeField]
    private CinemachineVirtualCamera VirtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    private void Start()
    {
        virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }

    public IEnumerator Shake(float amplitude, float time)
    {
        virtualCameraNoise.m_AmplitudeGain = amplitude;
        yield return new WaitForSeconds(time);
        virtualCameraNoise.m_AmplitudeGain = 0;
    }
}
