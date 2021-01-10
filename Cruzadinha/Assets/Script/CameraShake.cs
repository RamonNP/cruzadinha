using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class CameraShake : MonoBehaviour
{

    public static CameraShake _instance{get; private set;}
    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private float shakeTimer;

    private void Awake() {
        _instance =  this;
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float time) {
        CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin = 
        _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }
    private void Update() {
        if(shakeTimer > 0) {
            shakeTimer -= Time.deltaTime;
            if(shakeTimer<= 0f){
                 CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin = 
                _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
        }
    }
}
