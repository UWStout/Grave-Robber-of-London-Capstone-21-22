using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFloat : MonoBehaviour
{
    Vector3 StartPos;
    float FloatTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(StartPos.x, StartPos.y + ((1.0f - Mathf.Cos(FloatTime))*0.1f), StartPos.z);
        FloatTime += 0.05f;
    }
}
