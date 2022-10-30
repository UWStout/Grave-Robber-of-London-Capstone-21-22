using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameCameraConnector : MonoBehaviour
{
    Camera Camera;

    void Start()
    {
        Camera = GameObject.Find("Camera").GetComponent<Camera>();
            
        //Debug.Log(Camera);
        GetComponent<Canvas>().worldCamera = Camera;
    }
}
