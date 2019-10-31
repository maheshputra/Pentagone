using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBoundary : MonoBehaviour
{
    public static CameraBoundary instance;

    [SerializeField] private CinemachineVirtualCamera currentActiveCamera; //camera yang active
    [SerializeField] private CameraBoundaryTrigger[] cameraBoundaryTrigger; //array dari trigger yang ada
    [SerializeField] private CinemachineVirtualCamera[] virtualCamera; //aray dari camera yang ada


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

    /// <summary>
    /// Function untuk mengubah camera boundary yang baru
    /// public karena dipanggil dari script camera boundary trigger
    /// </summary>
    /// <param name="boundaryNumber">nomor boundary sesuai trigger</param>
    public void SetNewBoundary(int boundaryNumber)
    {
        virtualCamera[boundaryNumber].m_Priority = 1;
        currentActiveCamera.m_Priority = 0;
        currentActiveCamera = virtualCamera[boundaryNumber];
    }
}
