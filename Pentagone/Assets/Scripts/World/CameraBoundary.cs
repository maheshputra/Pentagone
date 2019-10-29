using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBoundary : MonoBehaviour
{
    public static CameraBoundary instance;

    [SerializeField] private CinemachineVirtualCamera currentActiveCamera;
    [SerializeField] private CameraBoundaryTrigger[] cameraBoundaryTrigger;
    [SerializeField] private CinemachineVirtualCamera[] virtualCamera;


    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < cameraBoundaryTrigger.Length; i++)
        {
            cameraBoundaryTrigger[i].boundaryNumber = i;
        }

        for (int i = 0; i < virtualCamera.Length; i++)
        {
            if (virtualCamera[i] == currentActiveCamera)
                continue;
            virtualCamera[i].m_Priority = 0;
        }
    }

    public void SetNewBoundary(int boundaryNumber)
    {
        virtualCamera[boundaryNumber].m_Priority = 1;
        currentActiveCamera.m_Priority = 0;
        currentActiveCamera = virtualCamera[boundaryNumber];
    }
}
