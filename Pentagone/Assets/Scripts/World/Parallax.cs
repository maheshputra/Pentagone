using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Transform[] groundlist; //setiap foreground/background
    private float[] parallaxScales; //proporsi movement camera
    [SerializeField] private float smoothing; //seberapa halusnya pergerakan parallax
    [SerializeField] private Transform mainCam; //main camera
    private Vector3 previousCamPos; //posisi main camera sebelumnya

    private void Awake()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        previousCamPos = mainCam.position;
        parallaxScales = new float[groundlist.Length];
        for (int i = 0; i < groundlist.Length; i++)
        {
            parallaxScales[i] = groundlist[i].position.z * -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < groundlist.Length; i++)
        {
            float parallax = (previousCamPos.x - mainCam.position.x) * parallaxScales[i];
            float groundListTargetPosX = groundlist[i].position.x + parallax;
            Vector3 groundListTargetPos = new Vector3(groundListTargetPosX, groundlist[i].position.y, groundlist[i].position.z);
            groundlist[i].position = Vector3.Lerp(groundlist[i].position, groundListTargetPos, smoothing * Time.deltaTime);
        }
        previousCamPos = mainCam.position;
    }


}
