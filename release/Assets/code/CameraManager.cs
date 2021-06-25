using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager sharedInstance = null;

    public CinemachineVirtualCamera virtualCamera;
    // Start is called before the first frame update
    private void Awake()
    {
        if(sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            sharedInstance = this;
        }

        GameObject vCam = GameObject.FindWithTag("VirtualCamera");
        virtualCamera = vCam.GetComponent<CinemachineVirtualCamera>();
    }
}
