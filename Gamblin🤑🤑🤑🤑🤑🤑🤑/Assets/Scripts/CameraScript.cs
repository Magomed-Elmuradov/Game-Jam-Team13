using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private CinemachineCamera vcam;
    [SerializeField]
    private PlayerScript player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isAlive == false || player.stopCamera == true)
        {
            vcam.Follow = null;
        }
        else
        {
            vcam.Follow = player.transform;
        }
    }
}
